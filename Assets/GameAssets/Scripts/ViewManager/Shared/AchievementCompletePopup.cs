using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementCompletePopup : MonoBehaviour, IPrefabInitializer
{
    public GameObject achievementButton,continue1, continue2;
    public TextMeshProUGUI achievementNameText;
    public ParticleSystem particles;
    public bool isAchievement;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        achievementNameText.text = (string)data[0];
        isAchievement = (bool)data[1];
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
    void PlayParticle()
    {
        particles.Play();
    }
    public void SeeAllAchievemens()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "onFooter", false },{"backAction",false}
        };
        StateManager.Instance.OpenStaticScreen("character", gameObject, "achievementScreen", mData, true);
        StateManager.Instance.CloseFooter();
        Continue();
    }
    public void Continue()
    {
        PopupController.Instance.ClosePopup("AchievementCompletePopup");
    }
}
