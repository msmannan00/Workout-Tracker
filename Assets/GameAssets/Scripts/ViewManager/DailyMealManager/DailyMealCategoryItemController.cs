using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DailyMealCategoryItemController : MonoBehaviour
{
    public TMP_Text mTitle;
    public TMP_Text mDescription;
    public TMP_Text aCounter;
    public Image aCountPlus;
    public Image aCountMinus;
    double aMealServingCount = 0.5f;
    DateTime mDate;
    int mDayState;
    Action mOnReloadData;

    void Start()
    {
        
    }

    public void countIncrement()
    {
        if (aMealServingCount < 99.5)
        {
            aMealServingCount = aMealServingCount + 0.5;
            aCounter.text = aMealServingCount.ToString();
            initMeal();
        }
        mOnReloadData.Invoke();
    }
    public void countDecrement()
    {
        if (aMealServingCount > 0.5)
        {
            aMealServingCount = aMealServingCount - 0.5;
            aCounter.text = aMealServingCount.ToString();
            initMeal();
        }
        mOnReloadData.Invoke();
    }

    public void onRemoveMeal()
    {
        userSessionManager.Instance.mPlanModel.Meals[mDate][mDayState].Details[mTitle.text] = null;
        userSessionManager.Instance.SavePlanModel();
        mOnReloadData.Invoke();
        GameObject.Destroy(this);
    }

    void initMeal()
    {
        if (userSessionManager.Instance.mPlanModel.Meals[mDate][mDayState].Details[mTitle.text] == null)
        {
            userSessionManager.Instance.mPlanModel.Meals[mDate][mDayState].Details[mTitle.text] = new MealDetail();
        }
        MealDetail mMealDetail = userSessionManager.Instance.mPlanModel.Meals[mDate][mDayState].Details[mTitle.text];
        mMealDetail.ServingAmount = aMealServingCount;
        userSessionManager.Instance.SavePlanModel();
    }


    public void initCategory(string pTitle, MealDetail pDetail, int pDayState, DateTime pDate, Action pOnReloadData)
    {
        mOnReloadData = pOnReloadData; 
        mDate = pDate;
        mDayState = pDayState;
        mTitle.text = pTitle;
        aCounter.text = userSessionManager.Instance.mPlanModel.Meals[mDate][mDayState].Details[mTitle.text].ServingAmount.ToString();
        aMealServingCount = userSessionManager.Instance.mPlanModel.Meals[mDate][mDayState].Details[mTitle.text].ServingAmount;
        mDescription.text = (pDetail.Kcals * pDetail.ServingAmount).ToString() + " kcals | " + (pDetail.Carbs * pDetail.ServingAmount).ToString() + "g carbs | " + (pDetail.Proteins * pDetail.ServingAmount).ToString() + "g protiens | " + (pDetail.Fats * pDetail.ServingAmount).ToString() + "g fats";
    }

    void Update()
    {
        
    }
}
