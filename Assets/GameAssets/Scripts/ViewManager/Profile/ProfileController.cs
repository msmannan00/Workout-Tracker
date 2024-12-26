using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour,PageController
{
    public Image characterImage;
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI achievementText;
    public TextMeshProUGUI joinedText;
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI levelText;
    public Image badgeIamge;
    public Button settingButton;
    //public ProGifPlayerPanel gifPlayer;
    void PageController.onInit(Dictionary<string, object> data, Action<object> callback)
    {
        
    }
    private void Start()
    {
        //gifPlayer.LoadAndPlay(userSessionManager.Instance.gifsPath + ApiDataHandler.Instance.GetClothes() + " front.gif");
        Sprite loadedSprite = Resources.Load<Sprite>("UIAssets/character/gifs/" + ApiDataHandler.Instance.GetClothes() + " front");
        characterImage.sprite = loadedSprite;
        userNameText.text = userSessionManager.Instance.mProfileUsername;
        achievementText.text = ApiDataHandler.Instance.GetCompletedAchievements(ApiDataHandler.Instance.getAchievementData()).ToString() + " / " + ApiDataHandler.Instance.GetTotalAchievements();
        joinedText.text = userSessionManager.Instance.joiningDate.ToString();

        settingButton.onClick.AddListener(Settings);
        streakText.GetComponent<Button>().onClick.AddListener(OpenStreakDetail);
    }
    private void OnEnable()
    {
        levelText.text= "Level "+userSessionManager.Instance.characterLevel.ToString();
        streakText.text = "Streak: " + userSessionManager.Instance.userStreak.ToString();
        goalText.text = userSessionManager.Instance.weeklyGoal.ToString();
        string badgeName = userSessionManager.Instance.badgeName;
        Sprite sprite = Resources.Load<Sprite>("UIAssets/Badge/" + badgeName);
        badgeIamge.sprite= sprite;
    }
    public void Settings()
    {
        AudioController.Instance.OnButtonClick();
        StateManager.Instance.OpenStaticScreen("profile", gameObject, "settingScreen", null,true);
        StateManager.Instance.CloseFooter();
    }
    public void PersonalBest()
    {
        AudioController.Instance.OnButtonClick();
        StateManager.Instance.OpenStaticScreen("profile", gameObject, "personalBestScreen", null, true);
        StateManager.Instance.CloseFooter();
    }
    public void Measurement()
    {
        AudioController.Instance.OnButtonClick();
        StateManager.Instance.OpenStaticScreen("profile", gameObject, "measurementScreen", null,true);
        StateManager.Instance.CloseFooter();
    }
    public void BadgeSelection()
    {
        StateManager.Instance.OpenStaticScreen("profile", gameObject, "ChangeBadgeScreen", null, true);
        StateManager.Instance.CloseFooter();
    }
    public void WeeklyGoal()
    {
        //AudioController.Instance.OnButtonClick();
        //DateTime now = DateTime.Now;
        //print(now + "    " + ApiDataHandler.Instance.GetCurrentWeekStartDate());
        //TimeSpan timeDifference = now - ApiDataHandler.Instance.GetCurrentWeekStartDate();
        //if(timeDifference.TotalDays >= 14 || userSessionManager.Instance.weeklyGoal == 0)
        //{
        //    Dictionary<string, object> mData = new Dictionary<string, object> { { "data", false },{ "text" , goalText } };
        //    StateManager.Instance.OpenStaticScreen("profile", gameObject, "weeklyGoalScreen", mData,true);
        //    StateManager.Instance.CloseFooter();
        //}
        //else
        //{
        //    GlobalAnimator.Instance.ShowTextMessage(messageText, "This can only be changed once every 2  weeks.",2f);
        //}

        
    }
    public void OpenStreakDetail()
    {
        PopupController.Instance.OpenPopup("profile","levelDetailPopup",null,null);
    }
}
