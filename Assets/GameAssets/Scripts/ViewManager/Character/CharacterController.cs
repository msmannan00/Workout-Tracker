using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour,PageController
{
    public Button shopButton, emotesButton, achievementButton;
    public void onInit(Dictionary<string, object> data, Action<object> callback) 
    {

    }
    private void Start()
    {
        shopButton.onClick.AddListener(ShopeButtonClick);
        emotesButton.onClick.AddListener(EmotesButtonClick);
        achievementButton.onClick.AddListener(AchievementButtonClick);
        shopButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        emotesButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        achievementButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    public void ShopeButtonClick()
    {
        StateManager.Instance.OpenStaticScreen("character", gameObject, "shopScreen", null,true);
        StateManager.Instance.CloseFooter();
    }
    public void EmotesButtonClick()
    {
        StateManager.Instance.OpenStaticScreen("character", gameObject, "emotesScreen", null, true);
        StateManager.Instance.CloseFooter();
    }
    public void AchievementButtonClick()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "onFooter", true },{"backAction",true}
        };
        StateManager.Instance.OpenStaticScreen("character", gameObject, "achievementScreen", mData, true);
        StateManager.Instance.CloseFooter();
    }
}
