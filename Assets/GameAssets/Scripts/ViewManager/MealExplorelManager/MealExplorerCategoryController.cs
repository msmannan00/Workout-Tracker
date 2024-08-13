using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MealExplorerCategoryController : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text aName;
    public TMP_Text aDescription;
    public Image aImage;
    public GameObject loader;

    GameObject mParent;
    string mTitle;
    ServingInfo mEachServing;
    MealItem mDishes;
    string mImagePath;

    void Start()
    {
        if (mImagePath.StartsWith("http://") || mImagePath.StartsWith("https://"))
        {
            StartCoroutine(HelperMethods.Instance.LoadImageFromURL(mImagePath, aImage, loader));
        }
        else
        {
            HelperMethods.Instance.LoadImageFromResources("UIAssets/mealExplorer/Categories/" + mImagePath, aImage);
            loader.SetActive(false);
        }
    }

    public void InitCategory(string pTitle, string description, MealItem pDish, ServingInfo pServing, string pImagePath, GameObject pParent)
    {
        aName.text = pTitle;
        aDescription.text = pDish.Measure + " Cups";
        mTitle = pTitle;
        mEachServing = pServing;
        mDishes = pDish;
        mImagePath = pImagePath;
        mParent = pParent;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalAnimator.Instance.WobbleObject(gameObject);
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        mData["title"] = mTitle;
        mData["eachServing"] = mEachServing;
        mData["dish"] = mDishes;
        StateManager.Instance.OpenStaticScreen("mealDetail", mParent, "mealDetailScreen", mData, true);
    }
}
