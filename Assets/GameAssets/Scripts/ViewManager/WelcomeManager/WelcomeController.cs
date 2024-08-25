using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Import for the Image component

public class WelcomeController : MonoBehaviour, PageController
{
    private int mPageNumber = 0;
    public GameObject mPage;
    public GameObject mFood;
    public TMP_Text mTitle;
    public TMP_Text mDescription;

    private string[] mPointerImage = { "pagePointer1", "pagePointer2", "pagePointer3" };
    private string[] mFoodImage = { "welcome1", "welcome2", "welcome3" };
    private string[] mTitleList = { "Dedication", "Tracking", "Traditional Exploration" };
    private string[] mDescriptionList = {
        "Achieve your fitness goals with mindful workout routines",
        "Track your progress and performance effortlessly",
        "Discover challenging and effective workout plans"
    };

    public void onInit(Dictionary<string, object> pData, Action<object> callback)
    {
        if (pData != null)
        {
            mPageNumber = (int)pData[WelcomeKeys.sPageNumber];
            mTitle.text = mTitleList[mPageNumber];
            mDescription.text = mDescriptionList[mPageNumber];

            Sprite pagePointer = Resources.Load<Sprite>("UIAssets/Welcome/Images/" + mPointerImage[mPageNumber]);
            mPage.GetComponent<Image>().sprite = pagePointer;

            Sprite foodImage = Resources.Load<Sprite>("UIAssets/Welcome/Images/" + mFoodImage[mPageNumber]);
            mFood.GetComponent<Image>().sprite = foodImage;
        }
    }

    public void getStarted()
    {
        if (mPageNumber == 2)
        {
            PreferenceManager.Instance.SetBool("WelcomeScreensShown_v3", true);
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
            };
            StateManager.Instance.OpenStaticScreen("auth", gameObject, "authScreen", mData);
        }
        else
        {
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { WelcomeKeys.sPageNumber, mPageNumber + 1 }
            };
            StateManager.Instance.OpenStaticScreen("welcome", gameObject, "welcomeScreen", mData);
        }
    }

    public void OnLogin()
    {
        PreferenceManager.Instance.SetBool("WelcomeScreensShown_v3", true);
        Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin}
            };
        StateManager.Instance.OpenStaticScreen("auth", gameObject, "authScreen", mData);
    }

}
