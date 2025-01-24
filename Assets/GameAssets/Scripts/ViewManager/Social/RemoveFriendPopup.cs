using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveFriendPopup : MonoBehaviour, IPrefabInitializer
{
    public Button yesButton;
    public Button noButton;
    public Button fade;
    Action<List<object>> callback;

    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        callback = onFinish;
        fade.onClick.AddListener(Closs);
        yesButton.onClick.AddListener(RemoveFriend);
        yesButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        noButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        noButton.onClick.AddListener(Closs);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Closs();
        }
    }
    public void Closs()
    {
        PopupController.Instance.ClosePopup("RemoveFriendPopup");
    }
    public void RemoveFriend()
    {
        callback?.Invoke(null);
        Closs();
    }
}