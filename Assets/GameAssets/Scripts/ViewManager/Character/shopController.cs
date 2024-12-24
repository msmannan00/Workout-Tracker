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
    public TextMeshProUGUI messageText;
    public Button backButton;
    public GameObject[] items;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {

    }
    private void Start()
    {
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        searchBar.onValueChanged.AddListener(SearchItems);
        ShopModel shopModel = ApiDataHandler.Instance.getShopData();
        foreach(GameObject item in items)
        {
            TextMeshProUGUI priceText = item.transform.Find("price text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = item.transform.Find("item name").GetComponent<TextMeshProUGUI>();
            ShopItem itemData = GetShopItemByName(shopModel, nameText.text);
            if (itemData.buyed)
            {
                priceText.text = "Bought";
            }
            else
            {
                priceText.text=itemData.price.ToString();
            }
            item.GetComponent<Button>().onClick.AddListener(() => BuyItemButton(itemData, priceText,nameText));
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public void BuyItemButton(ShopItem shopItem,TextMeshProUGUI priceText,TextMeshProUGUI nameText)
    {
        if (shopItem.buyed)
        {
            ApiDataHandler.Instance.SetCloths(shopItem.itemName.ToLower());
            GlobalAnimator.Instance.ShowTextMessage(messageText, nameText.text+" Selected", 2);
        }
        else
        {
            if (userSessionManager.Instance.currentCoins >= shopItem.price)
            {
                //GlobalAnimator.Instance.FadeInLoader();
                List<object> initialData = new List<object> { shopItem,priceText,nameText};
                Action<List<object>> onFinish = Bought;
                PopupController.Instance.OpenPopup("character", "BoughtPopup", onFinish,initialData);

            }
            else
            {
                // insufficient coins
                GlobalAnimator.Instance.ShowTextMessage(messageText, "Insufficient coins", 2);
            }
        }
    }
    public void Bought(List<object> list)
    {
        ShopItem shopItem = (ShopItem)list[0];
        TextMeshProUGUI priceText = (TextMeshProUGUI)list[1];
        TextMeshProUGUI nameText = (TextMeshProUGUI)list[2];
        int newCoins = userSessionManager.Instance.currentCoins - shopItem.price;
        ApiDataHandler.Instance.SetCoinsToFirebase(newCoins);
        GlobalAnimator.Instance.ShowTextMessage(messageText, nameText.text + " Selected", 2);
        SuccessfullyBuy(shopItem, priceText, nameText);
    }
    public void SuccessfullyBuy(ShopItem shopItem, TextMeshProUGUI priceText, TextMeshProUGUI nameText)
    {
        //GlobalAnimator.Instance.FadeOutLoader();
        shopItem.buyed = true;
        ApiDataHandler.Instance.SetCloths(shopItem.itemName.ToLower());
        string jsonString = JsonUtility.ToJson(ApiDataHandler.Instance.getShopData());
        ApiDataHandler.Instance.SetShopDataToFirebase(jsonString);
        priceText.text = "Bought";
        print("success end");
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
        ShopItem item = shopModel.items.FirstOrDefault(i => i.itemName.ToLower() == itemName.ToLower());

        if (item == null)
        {
            Console.WriteLine($"Item with name '{itemName}' not found.");
        }

        return item;
    }
}