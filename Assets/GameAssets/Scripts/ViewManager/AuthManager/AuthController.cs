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
    //public GoogleManager aGmailManager;
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


    //---- Google Signin-----//

    public void Start()
    {
        StartCoroutine(prelaodAssets());
        if (DataManager.Instance.IsMealLoaded())
        {
            GameObject uiBlocker = GameObject.Find("UIBlocker"); 
            if (uiBlocker != null)
            {
                uiBlocker.SetActive(false);
            }
        }

        userSessionManager.Instance.mSidebar = false;
        aUsername.text = "";
        GoogleAuth = new GoogleAuth();
        GoogleAuth.TryResume(OnSignIn, OnGetAccessToken);
        FacebookAuth = new FacebookAuth();
        StartCoroutine(WaitAndVerifyFirstLogin());
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

    private IEnumerator WaitAndVerifyFirstLogin()
    {
        while (!DataManager.Instance.IsMealLoaded())
        {
            yield return new WaitForSeconds(0.2f);
        }

        onVerifyFirstLogin();
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
            //GlobalAnimator.Instance.FadeInLoader();
            GoogleAuth.SignIn(OnSignIn, caching: true);
            userSessionManager.Instance.OnInitialize(mAuthType, "");
            print("Saved Gmail LogedIn -" + GoogleAuth.SavedAuth);
            onSignIn();
        }


    }
    void GmailSignIn()
    {

        if (GoogleAuth.SavedAuth != null)
        {
            GoogleAuth.SignIn(OnSignIn, caching: true);
            userSessionManager.Instance.OnInitialize(mAuthType, "");
            print("Saved Gmail LogedIn -" + GoogleAuth.SavedAuth);
            onSignIn();
        }

        else
        {
            GlobalAnimator.Instance.FadeInLoader();
            GoogleAuth.SignIn(OnSignIn, caching: true);
            print("Else Now Fresh SignIn");
        }
        

    }

    public void SignOut()
    {
        GoogleAuth.SignOut(revokeAccessToken: true);
        // Output.text = "Not signed in";
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

       
        print(mAuthType);

    }

    private void OnGetAccessToken(bool success, string error, Assets.SimpleGoogleSignIn.Scripts.TokenResponse tokenResponse)
    {
        //Output.text = success ? $"Access token: {tokenResponse.AccessToken}" : error;

        if (!success) return;

        var jwt = new Assets.SimpleGoogleSignIn.Scripts.JWT(tokenResponse.IdToken);

        Debug.Log($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        jwt.ValidateSignature(GoogleAuth.ClientId, OnValidateSignature);
    }

    private void OnValidateSignature(bool success, string error)
    {
        //Output.text += Environment.NewLine;
        //Output.text += success ? "JWT signature validated" : error;
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
            print("1stTime Gmail log in");
            GlobalAnimator.Instance.FadeOutLoader();
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeSignup}
            };

            //StateManager.Instance.OpenStaticScreen("planCreator", gameObject, "planCreatorScreen", mData);
            //StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", mData);
            StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
        }
        else
        {

            //GlobalAnimator.Instance.FadeOutLoader();
            Dictionary<string, object> mData = new Dictionary<string, object> { };
            //StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", mData);
            StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
            print("2nd time log in");
        }
        userSessionManager.Instance.LoadPlanModel();
    }


    //---------End Gmail---------//


    //-----------------Facbook login---------------------//

    public void FBSignIn()
    {
        FacebookAuth.FBSignIn(FBOnSignIn, caching: true);
    }

    public void FBSignOut()
    {
        FacebookAuth.FBSignOut(revokeAccessToken: true);
        //Output.text = "Not signed in";
    }

    public void FBGetAccessToken()
    {
        FacebookAuth.FBGetAccessToken(FBOnGetAccessToken);
    }

    private void FBOnSignIn(bool success, string error, Assets.SimpleFacebookSignIn.Scripts.UserInfo userInfo)
    {
        //Output.text = success ? $"Hello, {userInfo.name}!" : error;
        if (success)
        {
            GlobalAnimator.Instance.FadeOutLoader();
            onFBSignIn();
            mAuthType = success ? $"{userInfo.name}!" : error;
            userSessionManager.Instance.OnInitialize(mAuthType, "");
            print(mAuthType);
        }
        print("FB Loged In");
    }

    private void FBOnGetAccessToken(bool success, string error, Assets.SimpleFacebookSignIn.Scripts.TokenResponse tokenResponse)
    {
        //Output.text = success ? $"Access token: {tokenResponse.AccessToken}" : error;

        if (!success) return;

        var jwt = new Assets.SimpleFacebookSignIn.Scripts.JWT(tokenResponse.IdToken);

        Debug.Log($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        jwt.ValidateSignature(FacebookAuth.ClientId, OnValidateSignature);
    }

    private void FBOnValidateSignature(bool success, string error)
    {
        //Output.text += Environment.NewLine;
        //Output.text += success ? "JWT signature validated" : error;
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
            print("1stTime log in");
            GlobalAnimator.Instance.FadeOutLoader();
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeSignup}
            };

            StateManager.Instance.OpenStaticScreen("planCreator", gameObject, "planCreatorScreen", mData);
        }
        else
        {

            //GlobalAnimator.Instance.FadeInLoader();
            Dictionary<string, object> mData = new Dictionary<string, object> { };
            StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", mData);
            print("2nd time log in");
        }
        userSessionManager.Instance.LoadPlanModel();
    }

    //-----------------End FB-----------------------//


    public void onInit(Dictionary<string, object> pData)
    {
        this.mAuthType = (string)pData.GetValueOrDefault(AuthKey.sAuthType, AuthConstant.sAuthTypeSignup);
        if (this.mAuthType == AuthConstant.sAuthTypeLogin)
        {
            aTriggerButton.text = "Log In";
            aPageToggleText1.text = "I Don't have an account? ";
            aPageToggleText2.text = "Signup";
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
                    { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
                };
                StateManager.Instance.OpenStaticScreen("auth", gameObject, "authScreen", mData);
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

