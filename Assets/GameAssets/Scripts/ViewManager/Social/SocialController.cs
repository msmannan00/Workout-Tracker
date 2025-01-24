using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SocialController : MonoBehaviour,PageController
{
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI noFriendText;
    public TMP_InputField searchBar;
    public Button addFriendButton;

    public Transform content;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        addFriendButton.onClick.AddListener(AddFriendButton);
        addFriendButton.onClick.AddListener(AudioController.Instance.OnButtonClick);

        foreach(FriendData friendData in ApiDataHandler.Instance.getAllFriendDetails().friendData)
        {
            AddLoadedFriendOnScreen(friendData);
        }
        //StartCoroutine(FetchFriendDetailsForAll(ApiDataHandler.Instance.GetFriendsData()));
    }
    private void OnEnable()
    {
        if (ApiDataHandler.Instance.getAllFriendDetails().friendData.Count == 0)
            noFriendText.gameObject.SetActive(true);
    }
    private void AddFriendButton()
    {
        if (searchBar.text==userSessionManager.Instance.mProfileUsername)
        {
            GlobalAnimator.Instance.ShowTextMessage(messageText, "Entered ID is your, please enter friend now", 2);
            return;
        }
        if (searchBar.text == "")
        {
            GlobalAnimator.Instance.ShowTextMessage(messageText, "Please enter valid id", 2);
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
        userSessionManager.Instance.addedFriends += 1;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(FirebaseManager.Instance.user.UserId).
        Child("addedFriends").SetValueAsync(userSessionManager.Instance.addedFriends);
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
        FriendData friendDetails = new FriendData();

        // Start Firebase request to get friend details
        var dataTask = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(friendId).GetValueAsync();

        // Yield until the task completes
        yield return new WaitUntil(() => dataTask.IsCompleted);

        // Once the task completes, process the result
        if (dataTask.IsCompleted && dataTask.Result != null)
        {
            DataSnapshot snapshot = dataTask.Result;

            friendDetails.userName = friendName;

            string level = snapshot.Child("CharacterLevel").Value.ToString();
            friendDetails.level = int.Parse(level);

            friendDetails.badgeName = snapshot.Child("BadgeName").Value.ToString();

            if (snapshot.HasChild("clothes"))
            {
                friendDetails.clothe = snapshot.Child("clothes").Value.ToString();
            }
            else { friendDetails.clothe = "no clothes"; }

            string streak = snapshot.Child("streak").Value.ToString();
            friendDetails.streak = int.Parse(streak);

            string goal = snapshot.Child("weeklyGoal").Value.ToString();
            friendDetails.goal = int.Parse(goal);

            friendDetails.joiningDate = snapshot.Child("joiningDate").Value.ToString();

            TextAsset achievementJsonFile = Resources.Load<TextAsset>("data/achievement");
            string achievementJson = achievementJsonFile.text;
            friendDetails.achievementData = JsonUtility.FromJson<AchievementData>(achievementJson);
            ApiDataHandler.Instance.CheckCompletedAchievements(snapshot.Child("achievements"), friendDetails.achievementData);

            string personalBest = snapshot.Child("personalBest").GetRawJsonValue();
            friendDetails.personalBestData = (PersonalBestData)ApiDataHandler.Instance.LoadData(personalBest, typeof(PersonalBestData));

            if (snapshot.HasChild("profileImageUrl"))
            {
                friendDetails.profileImageUrl = snapshot.Child("profileImageUrl").Value.ToString();
            }

            ApiDataHandler.Instance.getAllFriendDetails().friendData.Add(friendDetails);
            AddLoadedFriendOnScreen(friendDetails);
            userSessionManager.Instance.CheckAchievementStatus();
        }
        else
        {
            Debug.LogWarning("Failed to fetch data for friend: " + friendName);
        }
    }
    public void AddFriendOnScreen(string name,string id,string level,string badgeName, string clotheName)
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "name", name }, { "userID", id },{ "level", level },
            {"badge",badgeName},{"clothe",clotheName}
        };
        GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/social/friendDataModel");
        GameObject exerciseObject = Instantiate(exercisePrefab, content);
        exerciseObject.GetComponent<SocialDataModel >().onInit(mData, null);
        noFriendText.gameObject.SetActive(false);
    }
    public void AddLoadedFriendOnScreen(FriendData friendData)
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            {"data",friendData}
        };
        GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/social/friendDataModel");
        GameObject exerciseObject = Instantiate(exercisePrefab, content);
        exerciseObject.GetComponent<SocialDataModel>().onInit(mData, null);
        noFriendText.gameObject.SetActive(false);
    }
}
