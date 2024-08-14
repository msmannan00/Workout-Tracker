using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.SimpleFacebookSignIn.Scripts
{
    /// <summary>
    /// https://developers.facebook.com/docs/facebook-login/limited-login/token/
    /// </summary>
    public partial class FacebookAuth
    {
        public SavedAuth SavedAuth { get; private set; }
        public TokenResponse TokenResponse { get; private set; }
        public string ClientId => _settings.ClientId;
        public bool DebugLog = true;

        private const string AuthorizationEndpoint = "https://www.facebook.com/v11.0/dialog/oauth";
        private const string TokenEndpoint = "https://graph.facebook.com/v11.0/oauth/access_token";
        private const string UserInfoEndpoint = "https://graph.facebook.com/v17.0/me?fields=id,name,email";

        private readonly FacebookAuthSettings _settings;
        private Implementation _implementation;
        private string _redirectUri, _state, _codeVerifier;
        private Action<bool, string, UserInfo> _callbackU;
        private Action<bool, string, TokenResponse> _callbackT;

        public FacebookAuth(FacebookAuthSettings settings = null)
        {
            _settings = settings == null ? Resources.Load<FacebookAuthSettings>("FacebookAuthSettings") : settings;

            if (_settings == null) throw new NullReferenceException(nameof(_settings));

            SavedAuth = SavedAuth.GetInstance(_settings.ClientId);
            Application.deepLinkActivated += FBOnDeepLinkActivated;

            #if UNITY_IOS && !UNITY_EDITOR

            SafariViewController.DidCompleteInitialLoad += DidCompleteInitialLoad;
            SafariViewController.DidFinish += UserCancelledHook;

            #endif

            #if UNITY_STANDALONE_WIN && !UNITY_EDITOR

            WindowsDeepLinking.Initialize(_settings.CustomUriScheme, OnDeepLinkActivated);

            #endif
        }

        ~FacebookAuth()
        {
            Application.deepLinkActivated -= FBOnDeepLinkActivated;

            #if UNITY_IOS && !UNITY_EDITOR

            SafariViewController.DidCompleteInitialLoad -= DidCompleteInitialLoad;
            SafariViewController.DidFinish -= UserCancelledHook;

            #endif
        }

        public void FBSignIn(Action<bool, string, UserInfo> callback, bool caching = true)
        {
            _callbackU = callback;
            _callbackT = null;

            FBInitialize();

            if (SavedAuth == null)
            {
                FBAuth();
            }
            else if (caching && SavedAuth.UserInfo != null)
            {
                callback(true, null, SavedAuth.UserInfo);
            }
            else
            {
                FBUseSavedToken();
            }
        }

        public void FBGetAccessToken(Action<bool, string, TokenResponse> callback)
        {
            _callbackU = null;
            _callbackT = callback;

            FBInitialize();

            if (SavedAuth == null || SavedAuth.TokenResponse.Expired)
            {
                FBAuth();
            }
            else
            {
                callback(true, null, SavedAuth.TokenResponse);
            }
        }

        public void FBSignOut(bool revokeAccessToken = false)
        {
            TokenResponse = null;

            if (SavedAuth != null)
            {
                SavedAuth.Delete();
                SavedAuth = null;
            }
        }

        private void FBInitialize()
        {
            #if UNITY_EDITOR

            _implementation = Implementation.LoopbackFlow;
            _redirectUri = $"http://localhost:{Helpers.GetRandomUnusedPort()}/";
            
            #elif UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE || UNITY_WSA

            _implementation = Implementation.AuthorizationMiddleware;
            _redirectUri = $"{_settings.CustomUriScheme}:/oauth2/facebook";

            #elif UNITY_WEBGL

            _implementation = Implementation.AuthorizationMiddleware;
            _redirectUri = "";

            #endif

            if (SavedAuth != null && SavedAuth.ClientId != _settings.ClientId)
            {
                SavedAuth.Delete();
                SavedAuth = null;
            }
        }

        private void FBAuth()
        {
            _state = Guid.NewGuid().ToString("N");
            _codeVerifier = $"{Guid.NewGuid():N}{Guid.NewGuid():N}";

            #if UNITY_IOS && !UNITY_EDITOR

            if (!_settings.UseSafariViewController) ApplicationFocusHook.Create(UserCancelledHook);

            #else

            ApplicationFocusHook.Create(FBUserCancelledHook);

            #endif

            var codeChallenge = Helpers.CreateCodeChallenge(_codeVerifier);
            var redirectUri = _implementation == Implementation.AuthorizationMiddleware ? AuthorizationMiddleware.Endpoint + "/redirect" : _redirectUri;
            var scope = string.Join(" ", _settings.AccessScopes);
            var authorizationRequest = $"{AuthorizationEndpoint}?client_id={_settings.ClientId}&scope={Uri.EscapeDataString(scope)}&response_type=code&redirect_uri={Uri.EscapeDataString(redirectUri)}&state={_state}&code_challenge={codeChallenge}&code_challenge_method=S256&nonce={Guid.NewGuid()}";

            if (_implementation == Implementation.AuthorizationMiddleware)
            {
                #if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE || UNITY_WSA

                AuthorizationMiddleware.Auth(_redirectUri, _state, (success, error) =>
                {
                    if (success)
                    {
                        FBAuthorizationRequest(authorizationRequest);
                    }
                    else
                    {
                        _callbackU?.Invoke(false, error, null);
                        _callbackT?.Invoke(false, error, null);
                    }
                });

                #elif UNITY_WEBGL

                AuthorizationMiddleware.Auth(_redirectUri, _state, authorizationRequest, (success, error, code) =>
                {
                    if (success)
                    {
                        PerformCodeExchange(code);
                    }
                    else
                    {
                        _callbackU?.Invoke(false, error, null);
                        _callbackT?.Invoke(false, error, null);
                    }
                });

                #endif
            }
            else
            {
                FBAuthorizationRequest(authorizationRequest);
                
                switch (_implementation)
                {
                    case Implementation.LoopbackFlow:
                        LoopbackFlow.Initialize(_redirectUri, FBOnDeepLinkActivated);
                        break;
                }
            }
        }

        private void FBAuthorizationRequest(string url)
        {
            FBLog($"Authorization: {url}");

            #if UNITY_IOS && !UNITY_EDITOR

            if (_settings.UseSafariViewController)
            {
                SafariViewController.OpenURL(url);
            }
            else
            {
                Application.OpenURL(url);
            }

            #else

            Application.OpenURL(url);

            #endif
        }

        private void FBDidCompleteInitialLoad(bool loaded)
        {
            if (loaded) return;

            const string error = "Failed to load auth screen.";

            _callbackT?.Invoke(false, error, null);
            _callbackU?.Invoke(false, error, null);
        }

        private async void FBUserCancelledHook()
        {
            var time = Time.time;

            while (Time.time < time + 1)
            {
                await Task.Yield();
            }

            if (_codeVerifier == null) return;

            _codeVerifier = null;

            const string error = "User cancelled.";

            _callbackT?.Invoke(false, error, null);
            _callbackU?.Invoke(false, error, null);
        }

        private void FBUseSavedToken()
        {
            if (SavedAuth == null || SavedAuth.ClientId != _settings.ClientId || SavedAuth.TokenResponse.Expired)
            {
                FBSignOut();
                FBSignIn(_callbackU);
            }
            else if (!SavedAuth.TokenResponse.Expired)
            {
                FBLog("Using saved access token...");
                FBRequestUserInfo(SavedAuth.TokenResponse.AccessToken, (success, _, userInfo) =>
                {
                    if (success)
                    {
                        _callbackU(true, null, userInfo);
                    }
                    else
                    {
                        FBSignOut();
                        FBSignIn(_callbackU);
                    }
                });
            }
        }

        private void FBOnDeepLinkActivated(string deepLink)
        {
            FBLog($"Deep link activated: {deepLink}");

            deepLink = deepLink.Replace(":///", ":/"); // Some browsers may add extra slashes.

            if (_redirectUri == null || !deepLink.StartsWith(_redirectUri) || _codeVerifier == null)
            {
                FBLog("Unexpected deep link.");
                return;
            }

            #if UNITY_IOS && !UNITY_EDITOR

            if (_settings.UseSafariViewController)
            {
                Log($"Closing SafariViewController");
                SafariViewController.Close();
            }

            #endif

            var parameters = Helpers.ParseQueryString(deepLink);
            var error = parameters.Get("error");

            if (error != null)
            {
                _callbackU?.Invoke(false, error, null);
                _callbackT?.Invoke(false, error, null);
                return;
            }

            var state = parameters.Get("state");
            var code = parameters.Get("code");
            
            if (state == null || code == null) return;

            if (state == _state)
            {
                FBPerformCodeExchange(code);
            }
            else
            {
                FBLog("Unexpected response.");
            }
        }

        private void FBPerformCodeExchange(string code)
        {
            var redirectUri = _implementation == Implementation.AuthorizationMiddleware ? AuthorizationMiddleware.Endpoint + "/redirect" : _redirectUri;
            var exchangeRequest = $"{TokenEndpoint}?client_id={_settings.ClientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&code_verifier={_codeVerifier}&code={code}";
            var request = UnityWebRequest.Get(exchangeRequest);

            _codeVerifier = null;

            FBLog($"Exchanging code for access token: {request.url}");

            request.SendWebRequest().completed += _ =>
            {
                if (request.error == null)
                {
                    FBLog($"TokenExchangeResponse={request.downloadHandler.text}");

                    TokenResponse = TokenResponse.Parse(request.downloadHandler.text);
                    SavedAuth = new SavedAuth(_settings.ClientId, TokenResponse);
                    SavedAuth.Save();

                    if (_callbackT != null)
                    {
                        _callbackT(true, null, TokenResponse);
                    }

                    if (_callbackU != null)
                    {
                        FBRequestUserInfo(TokenResponse.AccessToken, _callbackU);
                    }
                }
                else
                {
                    _callbackU?.Invoke(false, request.GetError(), null);
                    _callbackT?.Invoke(false, request.GetError(), null);
                }
            };
        }

        /// <summary>
        /// You can move this function to your backend for more security.
        /// </summary>
        public void FBRequestUserInfo(string accessToken, Action<bool, string, UserInfo> callback)
        {
            var request = UnityWebRequest.Get(UserInfoEndpoint);

            FBLog($"Requesting user info: {request.url}");

            request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
            request.SendWebRequest().completed += _ =>
            {
                if (request.error == null)
                {
                    FBLog($"UserInfo={request.downloadHandler.text}");
                    SavedAuth.UserInfo = JsonUtility.FromJson<UserInfo>(request.downloadHandler.text);
                    SavedAuth.Save();
                    callback(true, null, SavedAuth.UserInfo);
                }
                else
                {
                    callback(false, request.GetError(), null);
                }
            };
        }

        private void FBLog(string message)
        {
            if (DebugLog)
            {
                Debug.Log(message); // TODO: Remove in Release.
            }
        }
    }
}