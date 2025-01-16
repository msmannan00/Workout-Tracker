using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementCompletePopup : MonoBehaviour, IPrefabInitializer
{
    public GameObject achievementButton,continue1, continue2;
    public GameObject item,content;
    public ParticleSystem particles;
    public bool isAchievement;
    List<AchievementTemplateDataItem> items = new List<AchievementTemplateDataItem>();
    List<string> titles = new List<string>();
    string popupName;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        items = (List<AchievementTemplateDataItem>) data[0];
        titles = (List<string>) data[1];
        popupName = (string)data[2];
        isAchievement = (bool)data[3];
        for (int i = 0; i < items.Count; i++)
        {
            GameObject obj = Instantiate(item, content.transform);
            obj.name = titles[i];
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = titles[i];
            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].description;
        }
        item.SetActive(false);
        if (isAchievement)
        {
            achievementButton.SetActive(true);
            continue1.SetActive(true);
            continue2.SetActive(false);
        }
        else
        {
            achievementButton.SetActive(false);
            continue1.SetActive(false);
            continue2.SetActive(true);
        }
        Invoke("PlayParticle", 0.5f);
        //callback = onFinish;
        //workoutScreen = (GameObject)data[0];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Continue();
        }
    }
    void PlayParticle()
    {
        AudioController.Instance.OnAchievement();
        particles.Play();
    }
    public void SeeAllAchievemens()
    {
        AudioController.Instance.OnButtonClick();
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "onFooter", false },{"backAction",false}
        };
        StateManager.Instance.OpenStaticScreen("character", userSessionManager.Instance.currentScreen, "achievementScreen", mData, true);
        StateManager.Instance.CloseFooter();
        Continue();
    }
    public void Continue()
    {
        userSessionManager.Instance.completedItemsInSingleCheck = new List<AchievementTemplateDataItem>();
        userSessionManager.Instance.completedItemsTitles = new List<string>();
        AudioController.Instance.OnButtonClick();
        PopupController.Instance.ClosePopup(popupName);
    }
}
