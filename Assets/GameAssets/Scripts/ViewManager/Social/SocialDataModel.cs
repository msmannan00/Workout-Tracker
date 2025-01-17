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
    public TextMeshProUGUI loadingText;
    public Image badgeImage;
    public Image profileImage;
    public string userID;
    public string clothe;
    public FriendData friendData;
    Sprite profileSprite;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        friendData = (FriendData)data["data"];

        nameText.text = friendData.userName;
        levelText.text = "Lvl. "+friendData.level.ToString();
        badgeImage.sprite = Resources.Load<Sprite>("UIAssets/Badge/" + friendData.badgeName);
        this.GetComponent<Button>().onClick.AddListener(AudioController.Instance.OnButtonClick);
        this.GetComponent<Button>().onClick.AddListener(OpenDetails);

        if (friendData.profileImageUrl != null)
        {
            loadingText.gameObject.SetActive(true);
            string url = friendData.profileImageUrl;
            StartCoroutine(ApiDataHandler.Instance.LoadImageFromUrl(url, (loadedSprite) => {
                // This callback will receive the newly loaded sprite
                profileImage.sprite = loadedSprite;  // Assuming profileImage is your UI Image component
                loadingText.gameObject.SetActive(false);
                profileImage.rectTransform.anchoredPosition = new Vector2(0, 0);
                profileImage.rectTransform.sizeDelta = new Vector2(55, 55);
                profileSprite = loadedSprite;
            }));
        }
        else
            loadingText.gameObject.SetActive(false);
    }
    public void OpenDetails()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "data", friendData }, { "object", this.gameObject },{"profileImage",profileSprite}
            //{ "name", nameText.text }, { "id", userID }, { "object", this.gameObject }, {"clothe",clothe}
        };
        StateManager.Instance.OpenStaticScreen("social", userSessionManager.Instance.currentScreen, "profileScreen", mData, keepState: true);
        StateManager.Instance.CloseFooter();
    }
}
