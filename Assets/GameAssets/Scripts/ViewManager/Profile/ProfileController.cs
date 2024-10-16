using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour,PageController
{
    public TextMeshProUGUI achievementText;
    public TextMeshProUGUI joinedText;
    public TextMeshProUGUI goalText;
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
    public void WeeklyGoal()
    {
        DateTime now = DateTime.Now;
        TimeSpan timeDifference = now - ApiDataHandler.Instance.GetWeeklyGoalSetedDate();
        if(timeDifference.TotalDays >= 30)
        {
            Dictionary<string, object> mData = new Dictionary<string, object> { { "data", false } };
            StateManager.Instance.OpenStaticScreen("profile", gameObject, "weeklyGoalScreen", mData);
        }

        
    }
}
