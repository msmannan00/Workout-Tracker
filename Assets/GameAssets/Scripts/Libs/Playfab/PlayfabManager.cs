using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Collections;
using PlayFab.SharedModels;




#if !UNITY_IOS
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
#else
#endif

#if UNITY_IOS
using UnityEngine.iOS;
#else
#endif

public class PlayfabManager : GenericSingletonClass<PlayfabManager>
{

    public void OnSaveuser(string pUsername, string pPassword)
    {
        PlayerPrefs.SetString("username", pUsername);
        PlayerPrefs.SetString("password", pPassword);
        PlayerPrefs.Save();
    }



    public void OnServerInitialized()
    {
#if !UNITY_IOS
        //PlayGamesClientConfiguration mConfig = new PlayGamesClientConfiguration.Builder()
        //.AddOauthScope("profile")
        //.RequestServerAuthCode(false)
        //.Build();

        //PlayGamesPlatform.InitializeInstance(mConfig);
        //PlayGamesPlatform.DebugLogEnabled = false;
        //PlayGamesPlatform.Activate();

#else
#endif
    }

    public void OnTryLogin(string pEmail, string pPassword, Action<string, string> pCallbackSuccess, Action<PlayFabError> pCallbackFailure)
    {
        LoginWithEmailAddressRequest mRequest = new LoginWithEmailAddressRequest
        {
            Email = pEmail,
            Password = pPassword
        };

        PlayFabClientAPI.LoginWithEmailAddress(mRequest,
        res =>
        {
            OnSaveuser(pEmail, pPassword);
            StartCoroutine(WaitForCategoriesToInitialize(pEmail, res, pCallbackSuccess));
        },
        err =>
        {
            pCallbackFailure(err);
        });
    }

    IEnumerator WaitForCategoriesToInitialize(string pEmail, LoginResult res, Action<string, string> pCallbackSuccess)
    {
        while (DataManager.Instance.GetCategories() == null)
        {
            yield return null;
        }
        pCallbackSuccess(HelperMethods.Instance.ExtractUsernameFromEmail(pEmail), res.PlayFabId);
    }

    public void OnLogout()
    {
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("password");
    }

    public void OnLogoutForced()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("password");
    }

#if !UNITY_IOS
    //public void OnSignGmail(Action pCallbackSuccess, Action<PlayFabError> pCallbackFailure, Action<string, string> pCallbackSuccessPlayfab, Action<PlayFabError> pCallbackFailurePlayfab)
    //{

    //    Social.localUser.Authenticate((bool pSuccess) => {
    //        if (pSuccess)
    //        {
    //            var mServerAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
    //            PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
    //            {
    //                TitleId = "B9E19",
    //                ServerAuthCode = mServerAuthCode,
    //                CreateAccount = true
    //            },
    //            res =>
    //            {
    //                OnSaveuser("raza@gmail.com", "123456789");
    //                pCallbackSuccess();
    //            },
    //            err =>
    //            {
    //                OnTryLogin(PlayerPrefs.GetString("username"),
    //                PlayerPrefs.GetString("password"), pCallbackSuccessPlayfab, pCallbackFailurePlayfab);
    //                pCallbackSuccess();
    //            });
    //        }
    //        else
    //        {
    //            pCallbackFailure(null);
    //        }
    //    });
    //}
#else
#endif


#if UNITY_IOS
    //public void OnSignIOS(Action pCallbackSuccess, Action<PlayFabError> pCallbackFailure, Action<string, string> pCallbackSuccessPlayfab, Action<PlayFabError> pCallbackFailurePlayfab)
    //    {
    //        Device.RequestStoreReview();
    //        if (Device.systemVersion.StartsWith("10"))
    //        {
    //            NativeAPI.Authorize((success) =>
    //            {
    //                if (success)
    //                {
    //                    OnTryLogin("player@gmail.com", "killprg1", pCallbackSuccessPlayfab, pCallbackFailurePlayfab);
    //                    pCallbackSuccess();
    //                }
    //                else
    //                {
    //                    pCallbackFailure(null);
    //                }
    //            });
    //        }
    //        else
    //        {
    //            pCallbackFailure(null);
    //        }
    //    }

    //    public static class NativeAPI
    //    {
    //        public delegate void SignInCallback(bool pSuccess);

    //        public static void Authorize(SignInCallback pCallback)
    //        {
    //            bool mSuccess = true;
    //            pCallback?.Invoke(mSuccess);
    //        }
    //    }
#else
#endif

    public void OnTryRegisterNewAccount(string pEmail, string pPassword, Action pCallbackSuccess, Action<PlayFabError> pCallbackFailure)
    {
        RegisterPlayFabUserRequest mReqest = new RegisterPlayFabUserRequest
        {
            Email = pEmail,
            Password = pPassword,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(mReqest,
        res =>
        {
            OnSaveuser(pEmail, pPassword);
            pCallbackSuccess();
        },
        err =>
        {
            pCallbackFailure(err);
        });
    }

    public void InitiatePasswordRecovery(string pEmail, Action pCallbackSuccess, Action<PlayFabError> pCallbackFailure)
    {
        SendAccountRecoveryEmailRequest mRequest = new SendAccountRecoveryEmailRequest
        {
            Email = pEmail,
            TitleId = "B9E19"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(mRequest,
        result =>
        {
            Debug.Log("Password reset email sent successfully.");
            pCallbackSuccess();
        },
        error =>
        {
            Debug.LogError("Password reset failed: " + error.ErrorMessage);
            pCallbackFailure(error);
        });
    }
    public void InitiateAccountDeletion(string playFabId, Action callbackSuccess, Action<PlayFabError> callbackFailure)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "RequestAccountDeletionByPlayFabId",
            FunctionParameter = new { PlayFabId = playFabId },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request,
            result =>
            {
                if (result.Error != null)
                {
                    Debug.LogError("Cloud Script Error: " + result.Error.Message);
                    if (callbackFailure != null)
                    {
                        callbackFailure(new PlayFabError { ErrorMessage = result.Error.Message });
                    }
                }
                else
                {
                    Debug.Log("Account deletion requested successfully via Cloud Script.");
                    if (callbackSuccess != null)
                    {
                        callbackSuccess();
                    }
                }
            },
            error =>
            {
                Debug.LogError("Account deletion request failed: " + error.ErrorMessage);
                if (callbackFailure != null)
                {
                    callbackFailure(error);
                }
            });
    }


}
