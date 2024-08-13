using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMealCategoryController : MonoBehaviour
{
    public Image aCategoryIcon;
    public TMP_Text aHeader;
    GameObject mParent;
    int mDayState = 0;
    DateTime mDate;
    public GameObject aEditText;

    public void initCategory(int state, DateTime pDate, GameObject pParent)
    {
        DateTime currentDate = DateTime.Now;
        DateTime endDate = userSessionManager.Instance.mUserStatsModel.sEndingDate;
        if (!userSessionManager.Instance.mUserStatsModel.sContinueWeeklyPlan && currentDate > endDate)
        {
            aEditText.SetActive(false);
        }

        string imageName = "";
        mDayState = state;
        mDate = pDate;
        mParent = pParent;

        if (state == 0)
        {
            aHeader.text = "Breakfast";
            imageName = "breakfastIcon";
        }
        else if (state == 1)
        {
            aHeader.text = "Lunch";
            imageName = "lunchIcon";
        }
        else if (state == 2)
        {
            aHeader.text = "Dinner";
            imageName = "dinnerIcon";
        }

        LoadImage(imageName);
    }

    public void openMealExplorer()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        mData["state"] = mDayState;
        mData["date"] = mDate;
        StateManager.Instance.OpenStaticScreen("addMeal", mParent, "addMealScreen", mData, true);
    }

    private void LoadImage(string imageName)
    {
        string imagePath = $"UIAssets/PlanDashboard/Images/{imageName}";
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        aCategoryIcon.sprite = sprite;
    }
}
