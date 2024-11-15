using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private void Start()
    {
        /*Initiating app pages*/
        //#if UNITY_ANDROID && !UNITY_EDITOR
        //    Screen.fullScreen = false;
        //    AndroidUtility.ShowStatusBar(new Color32(9, 126, 57, 255));
        //#endif

        userSessionManager.Instance.LoadExcerciseData();
        Application.targetFrameRate = 60;
        FirebaseManager.Instance.Load(null);
        FirebaseExerciseManager.Instance.Load();
        DataManager.Instance.loadData();
        if (!PreferenceManager.Instance.GetBool("WelcomeScreensShown_v3"))
        {
            StateManager.Instance.OpenStaticScreen("welcome", null, "welcomeScreen", null);
        }
        else
        {
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
            };
            StateManager.Instance.OpenStaticScreen("auth", null, "authScreen", mData);
        }

    }

}
