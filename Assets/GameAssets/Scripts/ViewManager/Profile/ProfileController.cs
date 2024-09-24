using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour,PageController
{
    public Button settingButton;
    void PageController.onInit(Dictionary<string, object> data, Action<object> callback)
    {
        
    }
    private void Start()
    {
        settingButton.onClick.AddListener(Settings);
    }
    public void Settings()
    {
        StateManager.Instance.OpenStaticScreen("profile", gameObject, "settingScreen", null);
        StateManager.Instance.CloseFooter();
    }
}
