using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class SocialController : MonoBehaviour,PageController
{
    public TextMeshProUGUI messageText;
    public TMP_InputField searchBar;
    public Button addFriendButton;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        addFriendButton.onClick.AddListener(AddFriendButton);
        addFriendButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    private void AddFriendButton()
    {
        if (searchBar.text==userSessionManager.Instance.mProfileUsername)
        {
            GlobalAnimator.Instance.ShowTextMessage(messageText, "Entered ID is your, please enter friend now", 2);
            return;
        }
        if(ApiDataHandler.Instance.GetFriendsData().ContainsKey(searchBar.text))
        {
            GlobalAnimator.Instance.ShowTextMessage(messageText, "This user is already on your friends list.", 2);
            return;
        }
        StartCoroutine(GetUserIDFromUsername(searchBar.text));
        GlobalAnimator.Instance.FadeInLoader();
    }
    private IEnumerator GetUserIDFromUsername(string username)
    {
        yield return null;
        var usernameRef = FirebaseDatabase.DefaultInstance.GetReference("usernames");

        // Check if the username exists in the database
        print(username);
        var getUserIDTask = usernameRef.Child(username).GetValueAsync();

        yield return new WaitUntil(() => getUserIDTask.IsCompleted);

        if (getUserIDTask.Exception != null)
        {
            Debug.LogError("Error retrieving userID: " + getUserIDTask.Exception);
            GlobalAnimator.Instance.FadeOutLoader();
            yield break;
        }

        if (getUserIDTask.Result.Exists)
        {
            string userID = getUserIDTask.Result.Value.ToString();
            Debug.Log($"UserID for username {username} is: {userID}");
            GlobalAnimator.Instance.ShowTextMessage(messageText, "User add as friend", 2);
            AddFriendToFirebase(username, userID);
            GlobalAnimator.Instance.FadeOutLoader();
            // Do something with the userID (e.g., load user profile)
        }
        else
        {
            GlobalAnimator.Instance.ShowTextMessage(messageText, "User not found", 2);
            Debug.LogWarning($"Username {username} does not exist.");
            GlobalAnimator.Instance.FadeOutLoader();
        }
    }
    public void AddFriendToFirebase(string friendUserName,string friendID)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(FirebaseManager.Instance.user.UserId).
        Child("friend").Child(friendUserName).SetValueAsync(friendID);

    }
}
