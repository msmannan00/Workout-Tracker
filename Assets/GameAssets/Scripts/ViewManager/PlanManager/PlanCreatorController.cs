using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class PlanCreatorController : MonoBehaviour, PageController
{
    DateTime mCurrentDate;
    int mSelectedRangeIndex = 3;
    bool mContinuePlan = false;

    public TMP_Text aMonthYearDate;
    public TMP_Text aDateRangeStart;
    public TMP_Text aDateRangeEnd;
    public Image aContinuePlan;
    public GameObject aBackMonth;
    public GameObject aBackDay;
    public GameObject[] aDateRangeList;
    public GameObject[] aDateRangeListTriggers;
    public GameObject aBackNavigation;
    public GameObject aBackNavigationEmpty;
    public GridLayoutGroup gridLayoutGroup;

    public void onInit(Dictionary<string, object> data)
    {
    }

    public void onNextDate()
    {
        mCurrentDate = mCurrentDate.AddDays(1);
        onUpdateDates(3);
    }

    public void onPreviousDate()
    {
        mCurrentDate = mCurrentDate.AddDays(-1);
        onUpdateDates(3);
    }

    public void onNextMonth()
    {
        mCurrentDate = mCurrentDate.AddMonths(1);
        onUpdateDates(3);
    }

    public void onPreviousMonth()
    {
        mCurrentDate = mCurrentDate.AddMonths(-1);
        onUpdateDates(3);
    }

    public void onUpdateSelectedIndex(int pIndex)
    {
        GlobalAnimator.Instance.WobbleObject(aDateRangeList[pIndex]);
        onUpdateDates(pIndex);
        DateTime newStartDate = mCurrentDate.AddDays(mSelectedRangeIndex - 3);
        aDateRangeStart.text = newStartDate.ToString("MMM dd, yyyy");
        aDateRangeEnd.text = newStartDate.AddDays(7).ToString("MMM dd, yyyy");
    }

    private void onUpdateDates(int pSelectedIndex)
    {
        mSelectedRangeIndex = pSelectedIndex;
        aMonthYearDate.text = mCurrentDate.ToString("MMMM yyyy");
        aDateRangeStart.text = mCurrentDate.ToString("MMM dd, yyyy");
        aDateRangeEnd.text = mCurrentDate.AddDays(7).ToString("MMM dd, yyyy");

        for (int i = 0; i < aDateRangeList.Length; i++)
        {
            int offset = i - 3;
            DateTime date = mCurrentDate.AddDays(offset);

            TMP_Text dayText = aDateRangeList[i].transform.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text dateText = aDateRangeList[i].transform.GetChild(0).GetComponent<TMP_Text>();
            Image background = aDateRangeList[i].GetComponent<Image>();

            dayText.text = date.ToString("ddd");
            dateText.text = date.Day.ToString();

            if (i == mSelectedRangeIndex)
            {
                background.color = new Color32(0x09, 0x7E, 0x39, 0xFF);
                dayText.color = Color.white;
                dateText.color = Color.white;
            }
            else
            {
                background.color = Color.white;
                dayText.color = new Color32(0x8C, 0x8C, 0x8C, 0xFF);
                dateText.color = new Color32(0x32, 0x31, 0x36, 0xFF);
            }

            if (date < DateTime.Today)
            {
                aDateRangeListTriggers[i].GetComponent<Image>().raycastTarget  = false;
                background.color = new Color(background.color.r, background.color.g, background.color.b, 0.3f);
            }
            else
            {
                aDateRangeListTriggers[i].GetComponent<Image>().raycastTarget = true;
                background.color = new Color(background.color.r, background.color.g, background.color.b, 1f);
            }
        }

        if (mCurrentDate.AddMonths(-1) < DateTime.Today)
        {
            aBackMonth.GetComponent<Image>().raycastTarget = false;
            aBackMonth.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.3f);
        }
        else
        {
            aBackMonth.GetComponent<Image>().raycastTarget = true;
            aBackMonth.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }

        if (mCurrentDate.AddDays(-1) < DateTime.Today)
        {
            aBackDay.GetComponent<Image>().raycastTarget = false;
            aBackDay.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.3f);
        }
        else
        {
            aBackDay.GetComponent<Image>().raycastTarget = true;
            aBackDay.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void onToggleContinuePlan()
    {
        if (mContinuePlan)
        {
            aContinuePlan.color = Color.white;
        }
        else
        {
            aContinuePlan.color = new Color32(0x09, 0x7E, 0x39, 0xFF);
        }
        mContinuePlan = !mContinuePlan;
    }

    void Start()
    {
        bool isPlanInitialized = (bool)PreferenceManager.Instance.GetBool("FirstTimePlanInitialized_"+ userSessionManager.Instance.mProfileUsername, false);
        if (!isPlanInitialized)
        {
            aBackNavigation.SetActive(false);
            aBackNavigationEmpty.SetActive(true);
        }
        mCurrentDate = DateTime.Now;
        onUpdateDates(3);
        UpdateCellSize();
    }

    public void onStartPlan()
    {
        PreferenceManager.Instance.SetBool("FirstTimePlanInitialized_" + userSessionManager.Instance.mProfileUsername, true);
        this.gameObject.SetActive(false);
        userSessionManager.Instance.RemovePlanModel();
        userSessionManager.Instance.createPlan(mContinuePlan, aDateRangeStart.text, aDateRangeEnd.text);

        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", mData);
    }

    void UpdateCellSize()
    {
        float panelWidth = GetComponent<RectTransform>().rect.width;
        float spacing = gridLayoutGroup.spacing.x * (6 - 1)*1.5f;
        float padding = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float size = (panelWidth - spacing - padding);
        gridLayoutGroup.cellSize = new Vector2(size / 8, size / 5f);
    }

    public void onGoBack()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }

    public void onOpenSideBar()
    {
        StateManager.Instance.openSidebar("sidebar", gameObject, "sidebarScreen");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!userSessionManager.Instance.mSidebar)
            {
                StateManager.Instance.HandleBackAction(gameObject);
            }
        }
    }

}
