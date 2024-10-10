using UnityEngine;
using TMPro;
using System;
using PlayFab;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.SimpleGoogleSignIn.Scripts;
using Assets.SimpleFacebookSignIn.Scripts;
using System.Collections;
using UnityEditor.PackageManager;
using Firebase;
using Firebase.Auth;

public class AuthController : MonoBehaviour, PageController
{
    [Header("Managers")]
    public PlayfabManager aPlayFabManager;

    [Header("Utilities")]
    public TMP_Text aError;
    public TMP_Text aPageToggleText1;
    public TMP_Text aPageToggleText2;

    [Header("Auth Fields")]
    public TMP_InputField aUsername;
    public TMP_InputField aPassword;
    public TextMeshProUGUI aTriggerButton;

    private string mAuthType;


    public GoogleAuth GoogleAuth;
    public Text Log;
    public Text Output;

    public FacebookAuth FacebookAuth;


    public void Start()
    {
        StartCoroutine(prelaodAssets());
        userSessionManager.Instance.mSidebar = false;
        aUsername.text = "";
        GoogleAuth = new GoogleAuth();
        GoogleAuth.TryResume(OnSignIn, OnGetAccessToken);
        FacebookAuth = new FacebookAuth();
        onVerifyFirstLogin();
    }

    IEnumerator prelaodAssets()
    {
        UnityEngine.Object[] prefabs = Resources.LoadAll("Prefabs", typeof(GameObject));
        List<GameObject> instantiatedObjects = new List<GameObject>();

        foreach (UnityEngine.Object prefab in prefabs)
        {
            if (!prefab.name.Contains("auth") && prefab.name.Contains("screen"))
            {
                prefab.name = "cached";
                GameObject instantiatedPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform.parent.parent.parent) as GameObject;
                instantiatedObjects.Add(instantiatedPrefab);
                instantiatedPrefab.transform.SetAsFirstSibling();
            }
            yield return null;
        }

        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }

    }

    public void onVerifyFirstLogin()
    {
        string mUsername = PreferenceManager.Instance.GetString("login_username", "");
        if (mUsername.Length > 2)
        {
            if (mUsername.Contains("@"))
            {
                mUsername = HelperMethods.Instance.ExtractUsernameFromEmail(mUsername);
            }
            userSessionManager.Instance.OnInitialize(mUsername, mUsername);
            onSignIn();
        }
    }

    IEnumerator CallSavedlogins()
    {


        if (GoogleAuth.SavedAuth != null)
        {
            GlobalAnimator.Instance.FadeInLoader();
            yield return new WaitForSeconds(2);
            GoogleAuth.SignIn(OnSignIn, caching: true);
            userSessionManager.Instance.OnInitialize(mAuthType, "");
            onSignIn();
        }


    }
    void GmailSignIn()
    {

        if (GoogleAuth.SavedAuth != null)
        {
            GoogleAuth.SignIn(OnSignIn, caching: true);
            userSessionManager.Instance.OnInitialize(mAuthType, "");
            onSignIn();
        }

        else
        {
            GlobalAnimator.Instance.FadeInLoader();
            GoogleAuth.SignIn(OnSignIn, caching: true);
        }
        

    }

    public void SignOut()
    {
        GoogleAuth.SignOut(revokeAccessToken: true);
    }

    public void GetAccessToken()
    {
        GoogleAuth.GetAccessToken(OnGetAccessToken);
    }

    private void OnSignIn(bool success, string error, Assets.SimpleGoogleSignIn.Scripts.UserInfo userInfo)
    {
        if (success)
        {
            GlobalAnimator.Instance.FadeOutLoader();
            mAuthType = success ? $"{userInfo.name}" : error;
            userSessionManager.Instance.OnInitialize(mAuthType, "");
            onSignIn();
        }

    }

    private void OnGetAccessToken(bool success, string error, Assets.SimpleGoogleSignIn.Scripts.TokenResponse tokenResponse)
    {
        if (!success) return;

        var jwt = new Assets.SimpleGoogleSignIn.Scripts.JWT(tokenResponse.IdToken);

        Debug.Log($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        jwt.ValidateSignature(GoogleAuth.ClientId, OnValidateSignature);
    }

    private void OnValidateSignature(bool success, string error)
    {
    }

    public void Navigate(string url)
    {
        Application.OpenURL(url);
    }


    public void onSignIn()
    {
        gameObject.transform.parent.SetSiblingIndex(1);

        bool mFirsTimePlanInitialized = PreferenceManager.Instance.GetBool("FirstTimePlanInitialized_" + userSessionManager.Instance.mProfileUsername, false);
        if (!mFirsTimePlanInitialized)
        {
            GlobalAnimator.Instance.FadeOutLoader();
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeSignup}
            };
            StateManager.Instance.OpenStaticScreen("weight", gameObject, "weightScreen", mData);
            //StateManager.Instance.OpenFooter("shared", gameObject, "footer");
        }
        else
        {
            Dictionary<string, object> mData = new Dictionary<string, object> { };
            StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", mData);
            //StateManager.Instance.OpenFooter("shared", gameObject, "footer");
        }
    }



    public void onInit(Dictionary<string, object> pData, Action<object> callback)
    {
        this.mAuthType = (string)pData.GetValueOrDefault(AuthKey.sAuthType, AuthConstant.sAuthTypeSignup);
        if (this.mAuthType == AuthConstant.sAuthTypeLogin)
        {
            aTriggerButton.text = "Log In";
            aPageToggleText1.text = "New user?";
            aPageToggleText2.text = "Create an account.";
        }
        else if (this.mAuthType == AuthConstant.sAuthTypeSignup)
        {
            aTriggerButton.text = "Sign Up";
            aPageToggleText1.text = "Already have an acount?";
            aPageToggleText2.text = "Log In";
        }

    }

    public void OnPrivacyPolicy()
    {
        Application.OpenURL("");
    }

    public void OnOpenWebsite()
    {
        Application.OpenURL("");
    }



    public void OnTrigger()
    {
        if (this.mAuthType == AuthConstant.sAuthTypeLogin)
        {
            Action<string, string> mCallbackSuccess = (string pResult1, string pResult2) =>
            {
                GlobalAnimator.Instance.FadeOutLoader();
                userSessionManager.Instance.OnInitialize(pResult1, pResult2);
                onSignIn();
            };
            Action<FirebaseException> callbackFailure = (pError) =>
            {
                GlobalAnimator.Instance.FadeOutLoader();
                GlobalAnimator.Instance.FadeIn(aError.gameObject);
                //var errorMessage = pError.InnerException != null
                //    ? pError.InnerException.Message
                //    : pError.Message;
                //aError.text = ErrorManager.Instance.getTranslateError(errorMessage);

                AuthError errorCode = (AuthError)pError.ErrorCode;
                string message = "Login Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;
                    case AuthError.UserNotFound:
                        message = "Account does not exist";
                        break;
                }
                aError.text = message;
            };

            GlobalAnimator.Instance.FadeInLoader();
            print("login");
            FirebaseManager.Instance.OnTryLogin(this.aUsername.text, this.aPassword.text, mCallbackSuccess, callbackFailure);
        }
        else if (this.mAuthType == AuthConstant.sAuthTypeSignup)
        {
            Action callbackSuccess = () =>
            {
                GlobalAnimator.Instance.FadeOutLoader();
                GameObject alertPrefab = Resources.Load<GameObject>("Prefabs/alerts/alertSuccess");
                GameObject alertsContainer = GameObject.FindGameObjectWithTag("alerts");
                GameObject instantiatedAlert = Instantiate(alertPrefab, alertsContainer.transform);
                AlertController alertController = instantiatedAlert.GetComponent<AlertController>();
                alertController.InitController("Account Created Successfully", pTrigger: "Continue Login");
                GlobalAnimator.Instance.AnimateAlpha(instantiatedAlert, true);

                Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin }
                };
                StateManager.Instance.OpenStaticScreen("auth", gameObject, "authScreen", mData);
            };

            Action<AggregateException> callbackFailure = (pError) =>
            {
                //GlobalAnimator.Instance.FadeOutLoader();
                //GlobalAnimator.Instance.FadeIn(aError.gameObject);
                //aError.text = ErrorManager.Instance.getTranslateError(pError.Error.ToString());
                GlobalAnimator.Instance.FadeOutLoader();
                GlobalAnimator.Instance.FadeIn(aError.gameObject);
                var errorMessage = pError.InnerException != null
                    ? pError.InnerException.Message
                    : pError.Message;
                aError.text = ErrorManager.Instance.getTranslateError(errorMessage);
            };

            GlobalAnimator.Instance.FadeInLoader();
            FirebaseManager.Instance.OnTryRegisterNewAccount(this.aUsername.text, this.aPassword.text, callbackSuccess, callbackFailure);
        }
    }
    private void OnSignInSuccess(string userId)
    {
        GlobalAnimator.Instance.FadeOutLoader();
        userSessionManager.Instance.OnInitialize(userId, aUsername.text);
        onSignIn();
    }

    private void OnSignInFailure(string error)
    {
        GlobalAnimator.Instance.FadeOutLoader();
        GlobalAnimator.Instance.FadeIn(aError.gameObject);
        aError.text = error; // You can customize the error message here.
    }

    public void OnForgotPassword()
    {
        Action callbackSuccess = () =>
        {
            Action callbackSuccess = () =>
            {
                Application.OpenURL("mailto:");
            };

            GlobalAnimator.Instance.FadeOutLoader();
            GameObject alertPrefab = Resources.Load<GameObject>("Prefabs/alerts/alertSuccess");
            GameObject alertsContainer = GameObject.FindGameObjectWithTag("alerts");
            GameObject instantiatedAlert = Instantiate(alertPrefab, alertsContainer.transform);
            AlertController alertController = instantiatedAlert.GetComponent<AlertController>();
            alertController.InitController("Reset password instructions have been sent to your email address", pCallbackSuccess: callbackSuccess, pTrigger: "Open Mail");
            GlobalAnimator.Instance.AnimateAlpha(instantiatedAlert, true);
        };

        Action<PlayFabError> callbackFailure = (pError) =>
        {
            GlobalAnimator.Instance.FadeOutLoader();
            GlobalAnimator.Instance.FadeIn(aError.gameObject);
            aError.text = ErrorManager.Instance.getTranslateError(pError.Error.ToString());

            GameObject alertPrefab = Resources.Load<GameObject>("Prefabs/alerts/alertFailure");
            GameObject alertsContainer = GameObject.FindGameObjectWithTag("alerts");
            GameObject instantiatedAlert = Instantiate(alertPrefab, alertsContainer.transform);
            AlertController alertController = instantiatedAlert.GetComponent<AlertController>();
            alertController.InitController("Email address was not found in out database", pTrigger: "Continue", pHeader: "Request Error");
            GlobalAnimator.Instance.AnimateAlpha(instantiatedAlert, true);
        };
        GlobalAnimator.Instance.FadeInLoader();
        aPlayFabManager.InitiatePasswordRecovery(aUsername.text, callbackSuccess, callbackFailure);
    }



    public void OnSignGmail()
    {
        GmailSignIn();
      
    }


    public void OnResetErrors()
    {
        GlobalAnimator.Instance.FadeOut(aUsername.gameObject);
        GlobalAnimator.Instance.FadeOut(aPassword.gameObject);
    }

    public void OnToogleAuth()
    {
        if (this.mAuthType == AuthConstant.sAuthTypeLogin)
        {
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeSignup}
            };
            StateManager.Instance.OpenStaticScreen("auth", gameObject, "authScreen", mData);
        }
        else if (this.mAuthType == AuthConstant.sAuthTypeSignup)
        {
            OnResetErrors();
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
            };
            StateManager.Instance.OpenStaticScreen("auth", gameObject, "authScreen", mData);
        }
    }

}

