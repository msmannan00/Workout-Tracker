using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FriendProfileController : MonoBehaviour, PageController
{
    public Image characterImage;
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI achievementText;
    public TextMeshProUGUI joinedText;
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI loadingText;
    public Image badgeIamge;
    public Image profileImage;
    public Button backButton;
    public Button removeButton;
    //[SerializeField]
    //private AchievementData achievementData = new AchievementData();
    //[SerializeField]
    //private PersonalBestData personalBestData = new PersonalBestData();

    private GameObject friendObject;
    private string clothe;
    public FriendData friendData;
    void PageController.onInit(Dictionary<string, object> data, Action<object> callback)
    {
        friendData = (FriendData)data["data"];
        friendObject = (GameObject)data["object"];
        Sprite profile = (Sprite)data["profileImage"];
        if(profile != null)
        {
            profileImage.sprite = profile;  
            profileImage.rectTransform.anchoredPosition = new Vector2(0, 0);
            //profileImage.rectTransform.sizeDelta = new Vector2(90, 90);

            RectTransform mask = profileImage.transform.parent.GetComponent<RectTransform>();
            userSessionManager.Instance.FitImage(profileImage, mask);
        }
        //StartCoroutine(FetchFriendDetails((string)data["id"], (string)data["name"]));

        streakText.GetComponent<Button>().onClick.AddListener(OpenStreakDetail);
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        backButton.onClick.AddListener(Back);
        removeButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        removeButton.onClick.AddListener(RemoveFriend);

        //TextAsset achievementJsonFile = Resources.Load<TextAsset>("data/achievement");
        //string achievementJson = achievementJsonFile.text;
        //this.achievementData = JsonUtility.FromJson<AchievementData>(achievementJson);

        userNameText.text = friendData.userName;
        levelText.text = "Level " + friendData.level.ToString();

        string clothName = friendData.clothe + " front";
        string path = ($"{userSessionManager.Instance.gifsPath}{userSessionManager.Instance.GetGifFolder(friendData.level)}{clothName}/{clothName}_1");
        Sprite loadedSprite = Resources.Load<Sprite>(path);
        characterImage.sprite = loadedSprite;

        Sprite sprite = Resources.Load<Sprite>("UIAssets/Badge/" + friendData.badgeName);
        badgeIamge.sprite = sprite;

        streakText.text = "Streak: " + friendData.streak.ToString();

        joinedText.text = friendData.joiningDate;

        goalText.text = friendData.goal.ToString();

        achievementText.text = ApiDataHandler.Instance.GetCompletedAchievements(friendData.achievementData).ToString() + " / " + friendData.achievementData.achievements.Count.ToString();
        
        //if (friendData.profileImageUrl!=null)
        //{
        //    loadingText.gameObject.SetActive(true);
        //    string url = friendData.profileImageUrl;
        //    StartCoroutine(ApiDataHandler.Instance.LoadImageFromUrl(url, (loadedSprite) => {
        //        // This callback will receive the newly loaded sprite
        //        profileImage.sprite = loadedSprite;  // Assuming profileImage is your UI Image component
        //        loadingText.gameObject.SetActive(false);
        //        profileImage.rectTransform.anchoredPosition = new Vector2(0, 0);
        //        profileImage.rectTransform.sizeDelta = new Vector2(90, 90);
        //    }));
        //}
        //else
        //    loadingText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    private IEnumerator FetchFriendDetails(string friendId, string friendName)
    {
        yield return null;
        //GlobalAnimator.Instance.FadeInLoader();
        //// Start Firebase request to get friend details
        //var dataTask = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(friendId).GetValueAsync();

        //// Yield until the task completes
        //yield return new WaitUntil(() => dataTask.IsCompleted);

        //// Once the task completes, process the result
        //if (dataTask.IsCompleted && dataTask.Result != null)
        //{
        //    DataSnapshot snapshot = dataTask.Result;

        //    userNameText.text = friendName;

        //    string level = snapshot.Child("CharacterLevel").Value.ToString();
        //    levelText.text = "Level " + level.ToString();

        //    int lv = int.Parse(level);
        //    string spritePath = userSessionManager.Instance.gifSpritePath + userSessionManager.Instance.GetGifFolder(lv) + clothe + " front";
        //    print(spritePath);
        //    Sprite loadedSprite = Resources.Load<Sprite>(spritePath);
        //    characterImage.sprite = loadedSprite;

        //    string badgeName = snapshot.Child("BadgeName").Value.ToString();
        //    Sprite sprite = Resources.Load<Sprite>("UIAssets/Badge/" + badgeName);
        //    badgeIamge.sprite = sprite;

        //    streakText.text = "Streak: " + snapshot.Child("streak").Value.ToString();

        //    joinedText.text= snapshot.Child("joiningDate").Value.ToString();

        //    goalText.text= snapshot.Child("weeklyGoal").Value.ToString();

        //    if (snapshot.HasChild("achievement"))
        //    {
                
        //        ApiDataHandler.Instance.CheckCompletedAchievements(snapshot.Child("achievements"), achievementData);
        //        achievementText.text = ApiDataHandler.Instance.GetCompletedAchievements(achievementData).ToString() + " / " + achievementData.achievements.Count.ToString();
        //    }
        //    else
        //    {
        //        achievementText.text="0 / " + achievementData.achievements.Count.ToString();
        //    }

        //    string personalBest = snapshot.Child("personalBest").GetRawJsonValue();
        //    personalBestData = (PersonalBestData)ApiDataHandler.Instance.LoadData(personalBest, typeof(PersonalBestData));

        //    if (snapshot.HasChild("profileImageUrl"))
        //    {
        //        loadingText.gameObject.SetActive(true);
        //        string url= snapshot.Child("profileImageUrl").Value.ToString();
        //        StartCoroutine(ApiDataHandler.Instance.LoadImageFromUrl(url, (loadedSprite) => {
        //            // This callback will receive the newly loaded sprite
        //            profileImage.sprite = loadedSprite;  // Assuming profileImage is your UI Image component
        //            loadingText.gameObject.SetActive(false);
        //        }));
        //    }
        //    else
        //        loadingText.gameObject.SetActive(false);


        //    GlobalAnimator.Instance.FadeOutLoader();
        //}
        //else
        //{
        //    Debug.LogWarning("Failed to fetch data for friend: " + friendName);
        //    GlobalAnimator.Instance.FadeOutLoader();
        //    Back();
        //}
    }
    public void PersonalBestScreen()
    {
        AudioController.Instance.OnButtonClick();
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "data", friendData.personalBestData }
        };
        StateManager.Instance.OpenStaticScreen("social", gameObject, "personalBestScreen", mData, true);
        StateManager.Instance.CloseFooter();
    }
    public void RemoveFriend()
    {
        PopupController.Instance.OpenPopup("social", "RemoveFriendPopup", DeleteFriend, null);
    }
    public void DeleteFriend(object data)
    {
        StartCoroutine(DeletingFriendValue());
    }
    public IEnumerator DeletingFriendValue()
    {
        GlobalAnimator.Instance.FadeInLoader();
        // Build the reference path for the 'friends' node
        string path = $"users/{FirebaseManager.Instance.user.UserId}/friend/{userNameText.text}";

        // Start deleting the friend from Firebase
        var deleteTask = FirebaseDatabase.DefaultInstance.RootReference.Child(path).RemoveValueAsync();

        // Wait until the task completes
        yield return new WaitUntil(() => deleteTask.IsCompleted);

        // Check for errors
        if (deleteTask.Exception != null)
        {
            Debug.LogError("Error deleting friend: " + deleteTask.Exception);
        }
        else
        {
            userSessionManager.Instance.removedFriends += 1;
            string path2 = $"users/{FirebaseManager.Instance.user.UserId}/removedFriends";
            var removedFriend = FirebaseDatabase.DefaultInstance.RootReference.Child(path2)
                .SetValueAsync(userSessionManager.Instance.removedFriends);
            yield return new WaitUntil(() => removedFriend.IsCompleted);
            userSessionManager.Instance.CheckAchievementStatus();
            ApiDataHandler.Instance.GetFriendsData().Remove(userNameText.text);
            ApiDataHandler.Instance.getAllFriendDetails().friendData.Remove(friendData);
            Destroy(friendObject);
            StateManager.Instance.HandleBackAction(this.gameObject);
            StateManager.Instance.OpenFooter(null,null,false);
            // need to implement delay for remove friends
        }
        GlobalAnimator.Instance.FadeOutLoader();
    }
    public void OpenStreakDetail()
    {
        PopupController.Instance.OpenPopup("profile", "levelDetailPopup", null, null);
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(this.gameObject);
        StateManager.Instance.OpenFooter(null,null,false);
    }
}