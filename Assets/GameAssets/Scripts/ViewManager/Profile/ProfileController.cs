using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour,PageController
{
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI achievementText;
    public TextMeshProUGUI joinedText;
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI levelText;
    public Image badgeIamge;
    public Button settingButton;
    void PageController.onInit(Dictionary<string, object> data, Action<object> callback)
    {
        
    }
    private void Start()
    {
        

        achievementText.text = ApiDataHandler.Instance.GetCompletedAchievements().ToString() + " / " + ApiDataHandler.Instance.GetTotalAchievements();
        joinedText.text = ApiDataHandler.Instance.GetJoiningDate();
        
        settingButton.onClick.AddListener(Settings);
    }
    private void OnEnable()
    {
        levelText.text= "level "+ApiDataHandler.Instance.GetCharacterLevel().ToString();
        streakText.text = "Streak: " + ApiDataHandler.Instance.GetUserStreak().ToString();
        goalText.text = ApiDataHandler.Instance.GetWeeklyGoal().ToString();
        string badgeName = ApiDataHandler.Instance.GetBadgeName();
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
    public void WeeklyGoal()
    {
        AudioController.Instance.OnButtonClick();
        DateTime now = DateTime.Now;
        print(now + "    " + ApiDataHandler.Instance.GetCurrentWeekStartDate());
        TimeSpan timeDifference = now - ApiDataHandler.Instance.GetCurrentWeekStartDate();
        if(timeDifference.TotalDays >= 14 || ApiDataHandler.Instance.GetWeeklyGoal()==0)
        {
            Dictionary<string, object> mData = new Dictionary<string, object> { { "data", false },{ "text" , goalText } };
            StateManager.Instance.OpenStaticScreen("profile", gameObject, "weeklyGoalScreen", mData,true);
            StateManager.Instance.CloseFooter();
        }
        else
        {
            GlobalAnimator.Instance.ShowTextMessage(messageText, "This can only be changed once every 2  weeks.",2f);
        }

        
    }
}
