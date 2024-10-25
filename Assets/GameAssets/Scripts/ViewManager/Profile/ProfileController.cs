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
    public Button settingButton;
    void PageController.onInit(Dictionary<string, object> data, Action<object> callback)
    {
        
    }
    private void Start()
    {
        achievementText.text = ApiDataHandler.Instance.GetCompletedAchievements().ToString() + " / " + ApiDataHandler.Instance.GetTotalAchievements();
        joinedText.text = ApiDataHandler.Instance.GetJoiningDate();
        streakText.text = "Streak: "+ApiDataHandler.Instance.GetUserStreak().ToString();
        goalText.text = ApiDataHandler.Instance.GetWeeklyGoal().ToString();
        settingButton.onClick.AddListener(Settings);
    }
    public void Settings()
    {
        StateManager.Instance.OpenStaticScreen("profile", gameObject, "settingScreen", null,true);
        StateManager.Instance.CloseFooter();
    }
    public void PersonalBest()
    {
        StateManager.Instance.OpenStaticScreen("profile", gameObject, "personalBestScreen", null, true);
        StateManager.Instance.CloseFooter();
    }
    public void Measurement()
    {
        StateManager.Instance.OpenStaticScreen("profile", gameObject, "measurementScreen", null,true);
        StateManager.Instance.CloseFooter();
    }
    public void WeeklyGoal()
    {
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
