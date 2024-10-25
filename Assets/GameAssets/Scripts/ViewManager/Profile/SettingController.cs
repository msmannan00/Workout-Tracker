using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour, PageController
{
    public TMP_InputField searchBar;
    public Button backButton;
    public GameObject[] items;

    public RectTransform changeThemeToggle;

    Theme gameTheme;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        
    }
    void Start()
    {
        searchBar.onValueChanged.AddListener(SearchItems);
        backButton.onClick.AddListener(Back);
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
                changeThemeToggle.parent.GetComponent<Image>().color = Color.white;
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
        print("back");
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null, null, false);
    }
    public void ChangeGameTheme()
    {
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
                changeThemeToggle.parent.GetComponent<Image>().color = Color.white;
                changeThemeToggle.GetComponent<Image>().color = new Color32(218, 52, 52, 255);
                ApiDataHandler.Instance.SaveTheme(Theme.Light);
                gameTheme= Theme.Light;
                break;
        }
    }
}
