using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : GenericSingletonClass<FirebaseManager>
{
    public  FirebaseAuth auth;
    public FirebaseUser user;
    public DependencyStatus dependencyStatus=DependencyStatus.UnavilableMissing;
    public DatabaseReference databaseReference { get; private set; }
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
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                firebaseInitialized = true;
                user = auth.CurrentUser;
                if (user != null)
                {
                    // User is logged in, proceed to the main screen or home
                    Debug.Log("User is logged in: " + user.UserId);
                }
                onFirebaseInitialized?.Invoke();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
            if (auth == null)
                print("null");
        });
        //if (user == null)
        //{
        //    print(true);
        //    StateManager.Instance.OpenStaticScreen("welcome", null, "welcomeScreen", null);
        //}
        //else
        //{
        //    print(false);
        //    Dictionary<string, object> mData = new Dictionary<string, object>
        //    {
        //        { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
        //    };
        //    StateManager.Instance.OpenStaticScreen("auth", null, "authScreen", mData);
        //}
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
                //OnSaveUser(email, password);
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
                //OnSaveUser(email, password);
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
    public void CheckIfLocationExists(string path, System.Action<bool> callback)
    {
        databaseReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
                print("complete");
            bool exists = task.IsCompleted && task.Result.Exists;
            callback(exists);  // Call the callback with true or false
            return exists;
        });
    }
    //public void SaveDataToFirebase(string path, object value)
    //{
    //    print("saving...");
    //    databaseReference.Child(path).SetValueAsync(value).ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            Debug.Log("Data saved successfully at: " + path);
    //        }
    //        else
    //        {
    //            Debug.LogError("Failed to save data at: " + path);
    //        }
    //    });
    //}
    public void GetDataFromFirebase(string path, System.Action<DataSnapshot> callback)
    {
        databaseReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    // If data exists at the path, pass it to the callback
                    callback(snapshot);
                }
                else
                {
                    // If no data exists at the path
                    print("No data exist"+" "+path);
                }
            }
            else
            {
                //callback("Error: " + task.Exception?.Message);
                print("error"+task.Exception?.Message);
            }
        });
    }
    public void CheckUserUsername()
    {
        string userID = auth.CurrentUser.UserId;

        // Check if the user already has a username stored
        FirebaseDatabase.DefaultInstance.GetReference("users")
            .Child(userID)
            .Child("username")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError("Error retrieving username: " + task.Exception);
                    return;
                }

                if (task.Result.Value != null)
                {
                    string username = task.Result.Value.ToString();
                    Debug.Log("User's username: " + username);
                }
                else
                {
                    Debug.Log("User has not set a username yet.");
                    // Optionally prompt the user to set their username
                }
            });
    }


    

    public void OnServerInitialized()
    {
        // Firebase initialization logic (optional)
    }
}
