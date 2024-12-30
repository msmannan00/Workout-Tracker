using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SocialDataModel : MonoBehaviour,ItemController
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Image badgeImage;
    public string userID;
    public string clothe;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        nameText.text = (string)data["name"];
        levelText.text = "Lvl. "+(string)data["level"];
        userID = (string)data["userID"];
        string badgeName = (string)data["badge"];
        clothe = (string)data["clothe"];
        badgeImage.sprite = Resources.Load<Sprite>("UIAssets/Badge/" + badgeName);
        this.GetComponent<Button>().onClick.AddListener(AudioController.Instance.OnButtonClick);
        this.GetComponent<Button>().onClick.AddListener(OpenDetails);
    }
    public void OpenDetails()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "name", nameText.text }, { "id", userID }, { "object", this.gameObject }, {"clothe",clothe}
        };
        StateManager.Instance.OpenStaticScreen("social", userSessionManager.Instance.currentScreen, "profileScreen", mData, keepState: true);
        StateManager.Instance.CloseFooter();
    }
}
