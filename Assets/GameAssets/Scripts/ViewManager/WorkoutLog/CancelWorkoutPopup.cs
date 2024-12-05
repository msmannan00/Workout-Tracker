using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CancelWorkoutPopup : MonoBehaviour,IPrefabInitializer
{
    public Button resumeButton, cancelButton,fade;
    private GameObject workoutScreen;
    Action<List<object>> callback;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        callback = onFinish;
        workoutScreen = (GameObject)data[0];
    }
    private void Start()
    {
        fade.onClick.AddListener(Resume);
        cancelButton.onClick.AddListener(Cancel);
        cancelButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        resumeButton.onClick.AddListener(Resume);
        resumeButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }
    void Cancel()
    {
        PopupController.Instance.ClosePopup("CancelWorkoutPopup");
        StateManager.Instance.HandleBackAction(workoutScreen);
        StateManager.Instance.OpenFooter(null, null, false);
    }
    void Resume()
    {
        callback?.Invoke(null);
        PopupController.Instance.ClosePopup("CancelWorkoutPopup");
    }
}
