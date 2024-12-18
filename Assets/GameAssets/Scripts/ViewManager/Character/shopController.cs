using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class shopController : MonoBehaviour, PageController
{
    public TMP_InputField searchBar;
    public Button backButton;
    public GameObject[] items;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {

    }
    private void Start()
    {
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        searchBar.onValueChanged.AddListener(SearchItems);
        ShopModel shopModel = new ShopModel();
        foreach(GameObject item in items)
        {
            TextMeshProUGUI priceText = item.transform.Find("price text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = item.transform.Find("item name").GetComponent<TextMeshProUGUI>();
            ShopItem itemData = GetShopItemByName(shopModel, nameText.text);
            if (itemData.buyed)
            {
                priceText.text = "Buyed";
            }
            else
            {
                priceText.text=itemData.price.ToString();
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public void BuyItemButton(bool buyed)
    {

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
    public ShopItem GetShopItemByName(ShopModel shopModel, string itemName)
    {
        // Use LINQ to find the item
        ShopItem item = shopModel.items.FirstOrDefault(i => i.itemName == itemName);

        if (item == null)
        {
            Console.WriteLine($"Item with name '{itemName}' not found.");
        }

        return item;
    }
}