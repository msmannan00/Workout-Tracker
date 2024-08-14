using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DashboardMealCategoryController : MonoBehaviour, IPointerClickHandler
{
    [Header("Utilities")]
    public TMP_Text aName;
    public TMP_Text aItemCount;
    public Image aImage;
    public List<SubCategory> mSubCategory;
    public GameObject loader;
    GameObject mParent;

    public void initCategory(string pTitle, List<SubCategory> pSubCategory, string pImagePath, GameObject pParent)
    {
        aName.text = pTitle;
        mSubCategory = pSubCategory;
        aItemCount.text = pSubCategory.Count.ToString() + " Items";
        if (pImagePath.StartsWith("http://") || pImagePath.StartsWith("https://"))
        {
            StartCoroutine(HelperMethods.Instance.LoadImageFromURL(pImagePath, aImage, loader));
        }
        else
        {
            HelperMethods.Instance.LoadImageFromResources("UIAssets/Dashboard/Categories/" + pImagePath, aImage);
            loader.SetActive(false);
        }

        mParent = pParent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //GlobalAnimator.Instance.WobbleObject(gameObject);
        //openExplorerScreen();
    }

    public void openExplorerScreen()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        mData["data"] = mSubCategory;
        mData["title"] = aName.text;
        StateManager.Instance.OpenStaticScreen("mealExplorer", mParent, "mealExplorerScreen", mData, true);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
