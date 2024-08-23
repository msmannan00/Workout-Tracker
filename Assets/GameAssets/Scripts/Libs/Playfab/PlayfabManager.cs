using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;
using System.Collections;

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
