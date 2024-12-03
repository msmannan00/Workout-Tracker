using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour,PageController
{
    public TextMeshProUGUI levelText,coinText,messageText;
    public Button shopButton, emotesButton, achievementButton;
    public void onInit(Dictionary<string, object> data, Action<object> callback) 
    {
        
    }
    private void Start()
    {
        levelText.text = "Level " + ApiDataHandler.Instance.GetCharacterLevel().ToString();
        shopButton.onClick.AddListener(ShopeButtonClick);
        emotesButton.onClick.AddListener(EmotesButtonClick);
        achievementButton.onClick.AddListener(AchievementButtonClick);
        shopButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        emotesButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        achievementButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    private void OnEnable()
    {
        coinText.text=ApiDataHandler.Instance.GetCoins().ToString();
    }
    public void ShopeButtonClick()
    {
        StateManager.Instance.OpenStaticScreen("character", gameObject, "shopScreen", null,true);
        StateManager.Instance.CloseFooter();
    }
    public void EmotesButtonClick()
    {
        GlobalAnimator.Instance.ShowTextMessage(messageText, "Comming Soon...", 2);
        //StateManager.Instance.OpenStaticScreen("character", gameObject, "emotesScreen", null, true);
        //StateManager.Instance.CloseFooter();
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
