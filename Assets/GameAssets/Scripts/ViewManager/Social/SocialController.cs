using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class SocialController : MonoBehaviour,PageController
{
    public TextMeshProUGUI messageText;
    public TMP_InputField searchBar;
    public Button addFriendButton;

    public Transform content;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        addFriendButton.onClick.AddListener(AddFriendButton);
        addFriendButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        StartCoroutine(FetchFriendDetailsForAll(ApiDataHandler.Instance.GetFriendsData()));
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
        ApiDataHandler.Instance.GetFriendsData().Add(friendUserName, friendID);

        //(string level, string badgeName) = FirebaseManager.Instance.FetchFriendDetails(friendID);
        //AddFriendOnScreen(friendUserName, friendID, level, badgeName);
        Dictionary<string, string> newFriend= new Dictionary<string, string>(){{ friendUserName, friendID } };
        StartCoroutine(FetchFriendDetailsForAll(newFriend));
    }
    private IEnumerator FetchFriendDetailsForAll(Dictionary<string, string> friendsData)
    {
        GlobalAnimator.Instance.FadeInLoader();
        // Iterate through each friend and fetch their data asynchronously
        foreach (KeyValuePair<string, string> key in friendsData)
        {
            // Call the coroutine to fetch data for each friend
            yield return StartCoroutine(FetchFriendDetails(key.Value, key.Key));
        }
        GlobalAnimator.Instance.FadeOutLoader();
    }

    private IEnumerator FetchFriendDetails(string friendId, string friendName)
    {
        string level = "";
        string badgeName = "";

        // Start Firebase request to get friend details
        var dataTask = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(friendId).GetValueAsync();

        // Yield until the task completes
        yield return new WaitUntil(() => dataTask.IsCompleted);

        // Once the task completes, process the result
        if (dataTask.IsCompleted && dataTask.Result != null)
        {
            DataSnapshot snapshot = dataTask.Result;
            level = snapshot.Child("CharacterLevel").Value.ToString();
            badgeName = snapshot.Child("BadgeName").Value.ToString();

            // Output the fetched data
            Debug.Log("Friend Name: " + friendName + ", Level: " + level + ", Badge: " + badgeName);
            print(level + "  " + badgeName);
            // Check if we have valid data before adding the friend
            if (!string.IsNullOrEmpty(level) && !string.IsNullOrEmpty(badgeName))
            {
                // Add friend to screen
                AddFriendOnScreen(friendName, friendId, level, badgeName);
            }
        }
        else
        {
            Debug.LogWarning("Failed to fetch data for friend: " + friendName);
        }
    }
    public void AddFriendOnScreen(string name,string id,string level,string badgeName)
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "name", name }, { "userID", id },{ "level", level },
            {"badge",badgeName}
        };
        GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/social/friendDataModel");
        GameObject exerciseObject = Instantiate(exercisePrefab, content);
        exerciseObject.GetComponent<SocialDataModel >().onInit(mData, null);
    }
}
