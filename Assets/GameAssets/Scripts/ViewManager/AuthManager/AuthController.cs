using UnityEngine;
using TMPro;
using System;
using PlayFab;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.SimpleGoogleSignIn.Scripts;
using Assets.SimpleFacebookSignIn.Scripts;
using System.Collections;

public class AuthController : MonoBehaviour, PageController
{
    [Header("Managers")]
    public PlayfabManager aPlayFabManager;

    [Header("Utilities")]
    public TMP_Text aError;
    public TMP_Text aHeading;
    public TMP_Text aPageToggleText1;
    public TMP_Text aPageToggleText2;

    [Header("Auth Fields")]
    public TMP_InputField aUsername;
    public TMP_InputField aPassword;
    public TMP_InputField aReEnterPassword;
    public TextMeshProUGUI aTriggerButton;
    public GameObject aForgetPassword;
    public RectTransform aLineDevider;
    public RectTransform agoogle;
    public RectTransform aApple;

    private string mAuthType;


    public GoogleAuth GoogleAuth;
    public Text Log;
    public Text Output;
    public bool isRegistering;
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
        else if (mUsername == "")
        {
            PreferenceManager.Instance.SetBool("FirstTimePlanInitialized_" /*+ userSessionManager.Instance.mProfileUsername*/, true);
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

        bool mFirsTimePlanInitialized = PreferenceManager.Instance.GetBool("FirstTimePlanInitialized_" /*+ userSessionManager.Instance.mProfileUsername*/, false);
        if (!mFirsTimePlanInitialized)
        {
            GlobalAnimator.Instance.FadeOutLoader();
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeSignup}
            };
            StateManager.Instance.OpenStaticScreen("loading", gameObject, "loadingScreen", null);
            //StateManager.Instance.OpenStaticScreen("weight", gameObject, "weightScreen", mData);
            //userSessionManager.Instance.AddGymVisit();
            //if (ApiDataHandler.Instance.GetWeight() > 0)
            //{
            //    GlobalAnimator.Instance.FadeOutLoader();
            //    Dictionary<string, object> mData = new Dictionary<string, object>
            //    {
            //        { AuthKey.sAuthType, AuthConstant.sAuthTypeSignup}
            //    };
            //    StateManager.Instance.OpenStaticScreen("loading", gameObject, "loadingScreen", null);
            //    //StateManager.Instance.OpenStaticScreen("weight", gameObject, "weightScreen", mData);
            //    userSessionManager.Instance.AddGymVisit();
            //}
            //else
            //{
            //    Dictionary<string, object> mData = new Dictionary<string, object>
            //    {
            //        { "isFirstTime", true }
            //    };
            //    StateManager.Instance.OpenStaticScreen("weight", gameObject, "weightScreen", mData);
            //}
        }
        else
        {
            print("panel weekly goal");
            Dictionary<string, object> mData = new Dictionary<string, object> { { "data", true } };
            StateManager.Instance.OpenStaticScreen("profile", gameObject, "weeklyGoalScreen", mData);

            PreferenceManager.Instance.SetBool("FirstTimePlanInitialized_" /*+ userSessionManager.Instance.mProfileUsername*/, false);
        }


        //if (ApiDataHandler.Instance.GetWeeklyGoal() > 0)
        //{
        //    if (ApiDataHandler.Instance.GetWeight() > 0)
        //    {
        //        GlobalAnimator.Instance.FadeOutLoader();
        //        Dictionary<string, object> mData = new Dictionary<string, object>
        //        {
        //            { AuthKey.sAuthType, AuthConstant.sAuthTypeSignup}
        //        };
        //        StateManager.Instance.OpenStaticScreen("loading", gameObject, "loadingScreen", null);
        //        //StateManager.Instance.OpenStaticScreen("weight", gameObject, "weightScreen", mData);
        //        userSessionManager.Instance.AddGymVisit();
        //    }
        //    else
        //    {
        //        Dictionary<string, object> mData = new Dictionary<string, object>
        //        {
        //            { "isFirstTime", true }
        //        };
        //        StateManager.Instance.OpenStaticScreen("weight", gameObject, "weightScreen", mData);
        //    }
        //}
        //else
        //{
        //    Dictionary<string, object> mData = new Dictionary<string, object> { { "data", true } };
        //    StateManager.Instance.OpenStaticScreen("profile", gameObject, "weeklyGoalScreen", mData);

        //}
    }

    public void FBSignIn()
    {
        FacebookAuth.FBSignIn(FBOnSignIn, caching: true);
    }

    public void FBSignOut()
    {
        FacebookAuth.FBSignOut(revokeAccessToken: true);
    }

    public void FBGetAccessToken()
    {
        FacebookAuth.FBGetAccessToken(FBOnGetAccessToken);
    }

    private void FBOnSignIn(bool success, string error, Assets.SimpleFacebookSignIn.Scripts.UserInfo userInfo)
    {
        if (success)
        {
            GlobalAnimator.Instance.FadeOutLoader();
            onFBSignIn();
            mAuthType = success ? $"{userInfo.name}!" : error;
            userSessionManager.Instance.OnInitialize(mAuthType, "");
        }
    }

    private void FBOnGetAccessToken(bool success, string error, Assets.SimpleFacebookSignIn.Scripts.TokenResponse tokenResponse)
    {
        if (!success) return;

        var jwt = new Assets.SimpleFacebookSignIn.Scripts.JWT(tokenResponse.IdToken);

        Debug.Log($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        jwt.ValidateSignature(FacebookAuth.ClientId, OnValidateSignature);
    }

    private void FBOnValidateSignature(bool success, string error)
    {
    }

    public void FBNavigate(string url)
    {
        Application.OpenURL(url);
    }

    public void onFBSignIn()
    {
        gameObject.transform.parent.SetSiblingIndex(1);

        bool mFBFirsTimePlanInitialized = PreferenceManager.Instance.GetBool("FBFirstTimePlanInitialized_" + userSessionManager.Instance.mProfileUsername, false);
        if (!mFBFirsTimePlanInitialized)
        {
            GlobalAnimator.Instance.FadeOutLoader();
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeSignup}
            };

            StateManager.Instance.OpenStaticScreen("planCreator", gameObject, "planCreatorScreen", mData);
        }
        else
        {

            Dictionary<string, object> mData = new Dictionary<string, object> { };
            StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", mData);
        }
    }

    public void onInit(Dictionary<string, object> pData, Action<object> callback)
    {
        this.mAuthType = (string)pData.GetValueOrDefault(AuthKey.sAuthType, AuthConstant.sAuthTypeSignup);
        if (this.mAuthType == AuthConstant.sAuthTypeLogin)
        {
            aHeading.text = "login";
            aTriggerButton.text = "Continue";
            aPageToggleText1.text = "New user?";
            aPageToggleText2.text = "Create an account.";
            aForgetPassword.SetActive(true);
            aReEnterPassword.gameObject.SetActive(false);
            ChangeYPosition(aError.gameObject.GetComponent<RectTransform>(), 35);
            ChangeYPosition(aLineDevider, -16.5f);
            ChangeYPosition(agoogle, -86.6f);
            ChangeYPosition(aApple, -168.3f);
        }
        else if (this.mAuthType == AuthConstant.sAuthTypeSignup)
        {
            aHeading.text = "create account";
            aTriggerButton.text = "next";
            aPageToggleText1.text = "Already have an acount?";
            aPageToggleText2.text = "Log In";
            aForgetPassword.SetActive(false);
            aReEnterPassword.gameObject.SetActive(true);
            ChangeYPosition(aError.gameObject.GetComponent<RectTransform>(), -38);
            ChangeYPosition(aLineDevider, -68);
            ChangeYPosition(agoogle, -138);
            ChangeYPosition(aApple, -220);
            aPageToggleText1.gameObject.SetActive(false);
            aPageToggleText2.gameObject.SetActive(false);

        }

    }
    void ChangeYPosition(RectTransform rectTransform,float yPos)
    {
        Vector2 newPosition = rectTransform.anchoredPosition;
        newPosition.y = yPos;
        rectTransform.anchoredPosition = newPosition;
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
        AudioController.Instance.OnButtonClick();
        if (this.mAuthType == AuthConstant.sAuthTypeLogin)
        {
            Action<string, string> mCallbackSuccess = (string pResult1, string pResult2) =>
            {
                GlobalAnimator.Instance.FadeOutLoader();
                userSessionManager.Instance.OnInitialize(pResult1, pResult2);
                onSignIn();
            };
            Action<PlayFabError> callbackFailure = (pError) =>
            {
                GlobalAnimator.Instance.FadeOutLoader();
                GlobalAnimator.Instance.FadeIn(aError.gameObject);
                aError.text = ErrorManager.Instance.getTranslateError(pError.Error.ToString());
            };

            GlobalAnimator.Instance.FadeInLoader();
            aPlayFabManager.OnTryLogin(this.aUsername.text, this.aPassword.text, mCallbackSuccess, callbackFailure);
        }
        else if (this.mAuthType == AuthConstant.sAuthTypeSignup)
        {
            if (aPassword.text != aReEnterPassword.text)
            {
                aError.text = "Password does't match";
                aError.gameObject.SetActive(true);
                return;
            }
            Action callbackSuccess = () =>
            {
                GlobalAnimator.Instance.FadeOutLoader();
                print("true");
                //PreferenceManager.Instance.SetBool("FirstTimePlanInitialized_" + userSessionManager.Instance.mProfileUsername, true);

                GameObject alertPrefab = Resources.Load<GameObject>("Prefabs/alerts/alertSuccess");
                GameObject alertsContainer = GameObject.FindGameObjectWithTag("alerts");
                GameObject instantiatedAlert = Instantiate(alertPrefab, alertsContainer.transform);
                AlertController alertController = instantiatedAlert.GetComponent<AlertController>();
                alertController.InitController("Account Created Successfully", pTrigger: "Continue Login");
                GlobalAnimator.Instance.AnimateAlpha(instantiatedAlert, true);

                Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
                };
                StateManager.Instance.OpenStaticScreen("auth", gameObject, "authScreen", mData);
                ApiDataHandler.Instance.SetJoiningDate(DateTime.Now);
            };

            Action<PlayFabError> callbackFailure = (pError) =>
            {
                GlobalAnimator.Instance.FadeOutLoader();
                GlobalAnimator.Instance.FadeIn(aError.gameObject);
                aError.text = ErrorManager.Instance.getTranslateError(pError.Error.ToString());
            };

            GlobalAnimator.Instance.FadeInLoader();
            aPlayFabManager.OnTryRegisterNewAccount(this.aUsername.text, this.aPassword.text, callbackSuccess, callbackFailure);
        }
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
            alertsContainer.transform.SetAsLastSibling();
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
            alertsContainer.transform.SetAsLastSibling();
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

    public void FacebookSignIn()
    {
        FBSignIn();
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

