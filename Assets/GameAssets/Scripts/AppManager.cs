using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private void Start()
    {

        #if UNITY_ANDROID && !UNITY_EDITOR
            Screen.fullScreen = false;
            AndroidUtility.ShowStatusBar(new Color32(0, 0, 0, 255));
        #endif

        //userSessionManager.Instance.LoadExcerciseData();
        Application.targetFrameRate = 60;

        //StateManager.Instance.OpenStaticScreen("welcome", null, "welcomeScreen", null);
        FirebaseManager.Instance.Load(OpenScreen);
    }
    public void OpenScreen()
    {
        StartCoroutine(StartWait());
    }
    public void OnLogin()
    {
        PreferenceManager.Instance.SetBool("WelcomeScreensShown_v3", true);
        Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
            };
        StateManager.Instance.OpenStaticScreen("auth", null, "authScreen", mData);
    }
    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(1);
        OnLogin();
    }

}
