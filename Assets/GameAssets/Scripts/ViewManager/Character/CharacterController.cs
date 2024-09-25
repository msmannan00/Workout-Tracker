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
    }
    public void ShopeButtonClick()
    {
        StateManager.Instance.OpenStaticScreen("character", gameObject, "shopScreen", null);
        StateManager.Instance.CloseFooter();
    }
    public void EmotesButtonClick()
    {
        StateManager.Instance.OpenStaticScreen("character", gameObject, "emotesScreen", null);
        StateManager.Instance.CloseFooter();
    }
    public void AchievementButtonClick()
    {
        StateManager.Instance.OpenStaticScreen("character", gameObject, "achievementScreen", null);
        StateManager.Instance.CloseFooter();
    }
}
