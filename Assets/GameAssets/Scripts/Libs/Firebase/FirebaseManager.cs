using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : GenericSingletonClass<FirebaseManager>
{
    public  FirebaseAuth auth;
    private FirebaseUser user;
    public DependencyStatus dependencyStatus=DependencyStatus.UnavilableMissing;
    public bool firebaseInitialized;

    private void Start()
    {

    }

    public void Load(Action onFirebaseInitialized)
    {
        print("Load");
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                // Firebase is ready
                dependencyStatus = DependencyStatus.Available;
                auth = FirebaseAuth.DefaultInstance;
                print(task.Result);
                firebaseInitialized = true;
                onFirebaseInitialized?.Invoke();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
            if (auth == null)
                print("null");
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
        if (!firebaseInitialized)
        {
            print("Firebase not initialized. Loading...");
            Load(() => OnTryLogin(email, password, onSuccess, onFailure));
            return;
        }
        Credential credential =
         EmailAuthProvider.GetCredential(email, password);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            print("in");
            
            if (task.IsCanceled || task.IsFaulted)
            {
                print("if");
                onFailure?.Invoke(task.Exception.GetBaseException() as FirebaseException);
            }
            else
            {
                print("else");
                user = task.Result.User;
                OnSaveUser(email, password);
                string username = HelperMethods.Instance.ExtractUsernameFromEmail(email);
                string userId = user.UserId;
                print(username + "  " + userId);
                onSuccess?.Invoke(username, userId);
            }
        });
    }

    public void OnTryRegisterNewAccount(string email, string password, Action pCallbackSuccess, Action<AggregateException> onFailure)
    {
        print("try to create");
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            print("checking");
            if (task.IsCanceled)
            {
                print("cancel");
                onFailure(task.Exception);
            }
            else if (task.IsFaulted)
            {
                print("fault");
                onFailure(task.Exception);
            }
            else
            {
                print("create");
                user = task.Result.User;
                OnSaveUser(email, password);
                pCallbackSuccess.Invoke();
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
