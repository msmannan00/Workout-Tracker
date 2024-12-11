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

        FirebaseManager.Instance.Load(OpenScreen);
        //FirebaseExerciseManager.Instance.Load();
        //DataManager.Instance.loadData();

        //ApiDataHandler.Instance.loadData();


        //if (FirebaseManager.Instance.user == null)
        //{
        //    StateManager.Instance.OpenStaticScreen("welcome", null, "welcomeScreen", null);
        //}
        //else
        //{
        //    Dictionary<string, object> mData = new Dictionary<string, object>
        //    {
        //        { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
        //    };
        //    StateManager.Instance.OpenStaticScreen("auth", null, "authScreen", mData);
        //}

        

    }
    public void OpenScreen()
    {
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
