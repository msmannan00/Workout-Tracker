//using PlayFab;
//using System;

//#if !UNITY_IOS
//	//using GooglePlayGames;
//	using PlayFab.ClientModels;
//	using UnityEngine;
//	//using GooglePlayGames.BasicApi;
//#else
//#endif

//public class GoogleManager
//{

//#if !UNITY_IOS
//    public GoogleManager()
//    {
//        PlayGamesClientConfiguration mConfig = new PlayGamesClientConfiguration.Builder()
//        .AddOauthScope("profile")
//        .RequestServerAuthCode(false)
//        .Build();

//        PlayGamesPlatform.InitializeInstance(mConfig);
//        PlayGamesPlatform.DebugLogEnabled = true;
//        PlayGamesPlatform.Activate();
//    }

//    public void OnSignGmail(Action pCallbackSuccess, Action<PlayFabError> pCallbackFailure)
//    {
//        Social.localUser.Authenticate((bool pSuccess) => {
//            if (pSuccess)
//            {
//                var mServerAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
//                PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
//                {
//                    TitleId = "B9E19",
//                    ServerAuthCode = mServerAuthCode, 
//                    CreateAccount = true
//                },
//                res =>
//                {
//                    pCallbackSuccess();
//                },
//                err =>
//                {
//                    pCallbackFailure(err);
//                });
//            }
//            else
//            {
//                pCallbackFailure(null);
//            }
//        });
//    }
//#else
//#endif
//}
