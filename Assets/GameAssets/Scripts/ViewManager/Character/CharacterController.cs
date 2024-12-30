using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour,PageController
{
    public TextMeshProUGUI levelText,coinText,messageText;
    public CharacterSide currentSide = CharacterSide.Front;
    public Button shopButton, emotesButton, achievementButton;
    public Button leftButton;
    public Button rightButton;
    public ProGifPlayerPanel gifPlayer;
    public void onInit(Dictionary<string, object> data, Action<object> callback) 
    {
        levelText.GetComponent<Button>().onClick.AddListener(LevelDetailPopup);
    }
    private void Start()
    {
        levelText.text = "Level " + userSessionManager.Instance.characterLevel.ToString();
        shopButton.onClick.AddListener(ShopeButtonClick);
        emotesButton.onClick.AddListener(EmotesButtonClick);
        achievementButton.onClick.AddListener(AchievementButtonClick);
        shopButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        emotesButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        achievementButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        leftButton.onClick.AddListener(OnLeftButtonPressed);
        rightButton.onClick.AddListener(OnRightButtonPressed);
        leftButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        rightButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    private void OnEnable()
    {
        coinText.text=userSessionManager.Instance.currentCoins.ToString();
        currentSide = CharacterSide.Front;
        UpdateCharacterView();
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
    public void LevelDetailPopup()
    {
        PopupController.Instance.OpenPopup("character", "levelDetailPopup", null, null);
    }

    public void OnLeftButtonPressed()
    {
        if (currentSide == CharacterSide.Back)
        {
            currentSide = CharacterSide.Front;
        }
        else if (currentSide == CharacterSide.Front)
        {
            currentSide = CharacterSide.Side;
        }
        UpdateCharacterView();
    }

    public void OnRightButtonPressed()
    {
        if (currentSide == CharacterSide.Front)
        {
            currentSide = CharacterSide.Back;
        }
        else if (currentSide == CharacterSide.Side)
        {
            currentSide = CharacterSide.Front;
        }
        UpdateCharacterView();
    }
    private void UpdateCharacterView()
    {
        switch (currentSide)
        {
            case CharacterSide.Front:
                gifPlayer.LoadAndPlay(userSessionManager.Instance.gifsPath + userSessionManager.Instance.GetGifFolder() + userSessionManager.Instance.clotheName + " front.gif");
                break;
            case CharacterSide.Side:
                gifPlayer.LoadAndPlay(userSessionManager.Instance.gifsPath + userSessionManager.Instance.GetGifFolder() + userSessionManager.Instance.clotheName + " side.gif");
                break;
            case CharacterSide.Back:
                gifPlayer.LoadAndPlay(userSessionManager.Instance.gifsPath + userSessionManager.Instance.GetGifFolder() + userSessionManager.Instance.clotheName + " back.gif");
                break;
        }
    }
}
