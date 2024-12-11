using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDataItem : MonoBehaviour,ItemController
{
    public List<Image> trophyImages = new List<Image>();
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI coinText;
    public AchievementTemplate _data;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        _data = (AchievementTemplate)data["data"];
        //print(_data.type);
        bool isRank = (bool)data["rank"];
        titleText.text = _data.title;
        //CheckAchievement();
        userSessionManager.Instance.CheckIndiviualAchievementStatus(_data,trophyImages, progressText, descriptionText,coinText);
    }

    public void ShowStarData(int index)
    {
        List<object> initialData = new List<object> { _data.achievementData[index].description, _data.achievementData[index].coins.ToString() };
        PopupController.Instance.OpenPopup("character", "StarDetailPopup", null, initialData);
    }
   

}
