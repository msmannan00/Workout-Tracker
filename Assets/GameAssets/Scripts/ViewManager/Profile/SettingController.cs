using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour, PageController
{
    public TMP_InputField searchBar;
    //public TextMeshProUGUI messageText;
    public Button backButton;
    public GameObject[] items;

    public RectTransform changeThemeToggle;

    Theme gameTheme;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    void Start()
    {
        searchBar.onValueChanged.AddListener(SearchItems);
        backButton.onClick.AddListener(Back);
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        changeThemeToggle.gameObject.GetComponent<Button>().onClick.AddListener(ChangeGameTheme);
        gameTheme=ApiDataHandler.Instance.LoadTheme();
        switch (gameTheme)
        {
            case Theme.Dark:
                GlobalAnimator.Instance.AnimateRectTransformX(changeThemeToggle, -13, 0.25f);
                changeThemeToggle.parent.GetComponent<Image>().color = new Color32(81, 14, 14, 255);
                changeThemeToggle.GetComponent<Image>().color = Color.white;
                break;
            case Theme.Light:
                GlobalAnimator.Instance.AnimateRectTransformX(changeThemeToggle, 13, 0.25f);
                changeThemeToggle.parent.GetComponent<Image>().color = new Color32(236, 219, 190, 255);
                changeThemeToggle.GetComponent<Image>().color = new Color32(218, 52, 52, 255);
                break;
        }
    }
    void SearchItems(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            foreach (var item in items)
            {
                item.SetActive(true);
            }
            return;
        }

        searchTerm = searchTerm.ToLower();

        foreach (var item in items)
        {
            TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
            bool matchFound = false;

            foreach (var text in texts)
            {
                string itemText = text.text.ToLower();
                if (itemText.Contains(searchTerm))
                {
                    matchFound = true;
                    break;
                }
            }
            item.SetActive(matchFound);
        }
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null, null, false);
    }
    public void ChangeGameTheme()
    {
        AudioController.Instance.OnButtonClick();
        switch (gameTheme)
        {
            case Theme.Light:
                GlobalAnimator.Instance.AnimateRectTransformX(changeThemeToggle, -13, 0.25f);
                changeThemeToggle.parent.GetComponent<Image>().color = new Color32(81,14,14,255);
                changeThemeToggle.GetComponent<Image>().color = Color.white;
                ApiDataHandler.Instance.SaveTheme(Theme.Dark);
                gameTheme= Theme.Dark;
                break;
            case Theme.Dark:
                GlobalAnimator.Instance.AnimateRectTransformX(changeThemeToggle, 13, 0.25f);
                changeThemeToggle.parent.GetComponent<Image>().color = new Color32(236, 219, 190, 255);
                changeThemeToggle.GetComponent<Image>().color = new Color32(218, 52, 52, 255);
                ApiDataHandler.Instance.SaveTheme(Theme.Light);
                gameTheme= Theme.Light;
                break;
        }
        GUISetting[] scripts = FindObjectsOfType<GUISetting>();
        foreach (GUISetting script in scripts)
        {
            script.SetTheme();
        }
    }
    public void ChangeWeightUnit()
    {
        PopupController.Instance.OpenPopup("profile", "ChangeUnitPopup", null, null);
    }
    public void ChangeBadge()
    {
        PopupController.Instance.OpenPopup("profile", "ChangeBadgePopup", null, null);
    }
    public void ChangeWeeklyGoal()
    {
        AudioController.Instance.OnButtonClick();
        DateTime now = DateTime.Now;
        print(now + "    " + ApiDataHandler.Instance.GetCurrentWeekStartDate());
        TimeSpan timeDifference = now - ApiDataHandler.Instance.GetCurrentWeekStartDate();
        if (timeDifference.TotalDays >= 14 || ApiDataHandler.Instance.GetWeeklyGoal() == 0)
        {
            Dictionary<string, object> mData = new Dictionary<string, object> { { "data", false }};
            StateManager.Instance.OpenStaticScreen("profile", gameObject, "weeklyGoalScreen", mData, true);
            StateManager.Instance.CloseFooter();
        }
        else
        {
            //GlobalAnimator.Instance.ShowTextMessage(messageText, "This can only be changed once every 2  weeks.", 2f);
        }


    }
    public void OnButtonClick()
    {
        AudioController.Instance.OnButtonClick();
    }
}
