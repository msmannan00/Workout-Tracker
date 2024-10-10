using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : GenericSingletonClass<FirebaseManager>
{
    public  FirebaseAuth auth;
    private FirebaseUser user;
    public DependencyStatus dependencyStatus;

    private void Start()
    {
        //auth = FirebaseAuth.DefaultInstance;
        Load();
        //auth = FirebaseAuth.DefaultInstance;
    }

    public void Load()
    {
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result == Firebase.DependencyStatus.Available)
            {
                // Firebase is ready
                auth = FirebaseAuth.DefaultInstance;
                print(task.Result);
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }

    public void OnSaveUser(string email, string password)
    {
        PlayerPrefs.SetString("email", email);
        PlayerPrefs.SetString("password", password);
        PlayerPrefs.Save();
    }

    public void OnTryLogin(string email, string password, Action<string, string> onSuccess, Action<FirebaseException> onFailure)
    {
        print("out");
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            print("in");
            if (task.IsCanceled || task.IsFaulted)
            {
                print("if");
                onFailure(task.Exception.GetBaseException() as FirebaseException);
            }
            else
            {
                print("else");
                user = task.Result.User;
                OnSaveUser(email, password);
                string username = HelperMethods.Instance.ExtractUsernameFromEmail(email);
                string userId = user.UserId;
                print(username + "  " + userId);
                onSuccess(username, userId);
            }
        });
    }

    public void OnTryRegisterNewAccount(string email, string password, Action pCallbackSuccess, Action<AggregateException> onFailure)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                onFailure(task.Exception);
            }
            else
            {
                user = task.Result.User;
                OnSaveUser(email, password);
                pCallbackSuccess();
            }
        });
    }

    public void OnLogout()
    {
        auth.SignOut();
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("password");
    }

    public void OnLogoutForced()
    {
        auth.SignOut();
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("password");
    }

    public void InitiatePasswordRecovery(string email, Action onSuccess, Action<AggregateException> onFailure)
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                onFailure(task.Exception);
            }
            else
            {
                Debug.Log("Password reset email sent successfully.");
                onSuccess();
            }
        });
    }

    public void InitiateAccountDeletion(Action onSuccess, Action<AggregateException> onFailure)
    {
        if (user != null)
        {
            user.DeleteAsync().ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    onFailure(task.Exception);
                }
                else
                {
                    Debug.Log("Account deleted successfully.");
                    onSuccess();
                }
            });
        }
        else
        {
            Debug.LogError("No user is signed in.");
            onFailure(new AggregateException(new Exception("No user signed in.")));
        }
    }

    public void OnServerInitialized()
    {
        // Firebase initialization logic (optional)
    }
}
