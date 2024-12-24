using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoughtPopup : MonoBehaviour, IPrefabInitializer
{
    ShopItem item;
    TextMeshProUGUI priceText;
    TextMeshProUGUI nameText;
    public Button yesButton,noButton, fadeButton;
    Action<List<object>> callBack;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        item= (ShopItem)data[0];
        priceText= (TextMeshProUGUI)data[1];
        nameText= (TextMeshProUGUI)data[2];
        callBack = onFinish;
        yesButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        noButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        yesButton.onClick.AddListener(BoughtItem);
        noButton.onClick.AddListener(ClossPopup);
        fadeButton.onClick.AddListener(ClossPopup);
    }
    public void BoughtItem()
    {
        List<object> initialData = new List<object> { item,priceText,nameText };
        callBack?.Invoke(initialData);
        ClossPopup();
    }
    public void ClossPopup()
    {
        PopupController.Instance.ClosePopup("BoughtPopup");
    }
}
