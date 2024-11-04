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
    public Button infoButton;
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
        continueButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        //dropDown.gameObject.AddComponent<Button>();
        //dropDown.gameObject.GetComponent<Button>().onClick.AddListener(AudioController.Instance.OnButtonClick);
        infoButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    public void OnDropdownClick()
    {
        AudioController.Instance.OnButtonClick();
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
