using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeeklyGoalController : MonoBehaviour, PageController
{
    public TMP_Dropdown dropDown;
    bool firstTime;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        firstTime = (bool)data["data"];
        List<object> list = new List<object>();
        dropDown.onValueChanged.AddListener((index) =>
        {
            ApiDataHandler.Instance.SetWeeklyGoal(index + 2);
            ApiDataHandler.Instance.SetWeeklyGoalSetedDate(DateTime.Now);
            Invoke("Back", 0.25f);
        });
    }
    public void Back()
    {
        if (firstTime)
        {
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
            };
            StateManager.Instance.OpenStaticScreen("weight", gameObject, "weightScreen", mData);
        }
        else
        {
            StateManager.Instance.HandleBackAction(gameObject);
            StateManager.Instance.OpenFooter(null, null, false);
        }
    }
}
