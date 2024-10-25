using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeeklyGoalController : MonoBehaviour, PageController
{
    public TMP_Dropdown dropDown;
    public TextMeshProUGUI messageText;
    public Button continueButton;
    bool firstTime;
    TextMeshProUGUI goalText = null;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        firstTime = (bool)data["data"];
        if (data.ContainsKey("text"))
        {
            goalText = (TextMeshProUGUI)data["text"];
        }
        continueButton.onClick.AddListener(Continue);
        ////dropDown.onValueChanged.AddListener((index) =>
        ////{
        ////    lastSelectedValue = index;
        ////    ApiDataHandler.Instance.SetWeeklyGoal(index + 2);
        ////    ApiDataHandler.Instance.SetCurrentWeekStartDate(DateTime.Now);
        ////    if(goalText != null) { goalText.text = ApiDataHandler.Instance.GetWeeklyGoal().ToString(); }
        ////    Invoke("Back", 0.25f);
        ////});
    }
    public void OnDropdownClick()
    {
        ApiDataHandler.Instance.SetWeeklyGoal(dropDown.value + 2);
        ApiDataHandler.Instance.SetCurrentWeekStartDate(DateTime.Now);
        if (goalText != null) { goalText.text = ApiDataHandler.Instance.GetWeeklyGoal().ToString(); }
    }
    public void Continue()
    {
        if (firstTime)
        {
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
            };
            StateManager.Instance.OpenStaticScreen("weight", gameObject, "weightScreen", mData);
            userSessionManager.Instance.AddGymVisit();
        }
        else
        {
            StateManager.Instance.HandleBackAction(gameObject);
            StateManager.Instance.OpenFooter(null, null, false);
        }
    }

    public void ShowDetails()
    {
        GlobalAnimator.Instance.ShowTextMessage(messageText, "This can only be changed once every 2  weeks.",2f);
    }
}
