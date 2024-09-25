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
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        
    }
    void Start()
    {
        searchBar.onValueChanged.AddListener(SearchItems);
        backButton.onClick.AddListener(Back);
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
}
