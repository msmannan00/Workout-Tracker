using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CancelWorkoutPopup : MonoBehaviour,IPrefabInitializer
{
    public Button resumeButton, cancelButton;
    private GameObject workoutScreen;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        workoutScreen = (GameObject)data[0];
    }
    private void Start()
    {
        cancelButton.onClick.AddListener(Cancel);
        resumeButton.onClick.AddListener(Resume);
    }
    void Cancel()
    {
        PopupController.Instance.ClosePopup("CancelWorkoutPopup");
        StateManager.Instance.HandleBackAction(workoutScreen);
    }
    void Resume()
    {
        PopupController.Instance.ClosePopup("CancelWorkoutPopup");
    }
}
