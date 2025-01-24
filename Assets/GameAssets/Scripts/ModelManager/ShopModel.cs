using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ShopModel
{
    public List<ShopItem> items = new List<ShopItem>();
}

[Serializable]
public class ShopItem
{
    public string id;
    public string itemName;
    public ShopItemType itemType;
    public int price;
    public bool buyed;
}
