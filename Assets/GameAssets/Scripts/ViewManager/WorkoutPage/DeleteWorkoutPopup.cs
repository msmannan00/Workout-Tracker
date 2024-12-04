using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteWorkoutPopup : MonoBehaviour, IPrefabInitializer
{
    public Button yesButton, noButton, fade;
    private GameObject workoutScreen;
    private DefaultTempleteModel templeteModel;
    Action<List<object>> callback;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        callback = onFinish;
        workoutScreen = (GameObject)data[0];
        templeteModel = (DefaultTempleteModel)data[1];
    }
    private void Start()
    {
        yesButton.onClick.AddListener(YesButton);
        noButton.onClick.AddListener(NoButton);
        fade.onClick.AddListener(NoButton);
        yesButton.onClick.AddListener(AudioController.Instance.OnDelete);
        noButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    public void YesButton()
    {
        ApiDataHandler.Instance.getTemplateData().exerciseTemplete.Remove(templeteModel);
        ApiDataHandler.Instance.SaveTemplateData();
        PopupController.Instance.ClosePopup("DeleteWorkoutPopup");
        StateManager.Instance.OpenStaticScreen("dashboard", workoutScreen, "dashboardScreen", null);
        StateManager.Instance.OpenFooter(null, null, false);
        //AudioController.Instance.OnDelete();
    }
    public void NoButton()
    {
        PopupController.Instance.ClosePopup("DeleteWorkoutPopup");
    }
}