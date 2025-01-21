using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactUsPopup : MonoBehaviour, IPrefabInitializer
{
    public Button fadeButton;
    Action<List<object>> onFinish;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        this.onFinish = onFinish;
        fadeButton.onClick.AddListener(OffButton);
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            OffButton();
        }
    }
    public void OffButton()
    {
        onFinish?.Invoke(null);
        PopupController.Instance.ClosePopup("ContactUsPopup");
    }
}
