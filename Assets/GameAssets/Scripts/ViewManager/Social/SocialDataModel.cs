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

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        nameText.text = (string)data["name"];
        levelText.text = "Lvl. "+(string)data["level"];
        userID = (string)data["userID"];
        string badgeName = (string)data["badge"];
        badgeImage.sprite = Resources.Load<Sprite>("UIAssets/Badge/" + badgeName);
    }
}
