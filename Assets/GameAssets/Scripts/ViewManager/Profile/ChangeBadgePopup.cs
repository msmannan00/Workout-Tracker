using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBadgePopup : MonoBehaviour,IPrefabInitializer
{
    public Button continuButton;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {

    }
    private void Start()
    {
        continuButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        continuButton.onClick.AddListener(Continu);
    }
    public void Continu()
    {
        PopupController.Instance.ClosePopup("ChangeBadgePopup");
    }
}
