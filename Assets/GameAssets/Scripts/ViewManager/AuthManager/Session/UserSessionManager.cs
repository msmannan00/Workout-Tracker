using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class userSessionManager : GenericSingletonClass<userSessionManager>
{
    public string mProfileUsername;
    public string mProfileID;
    public userStatsModel mUserStatsModel;
    public PlanModel mPlanModel = new PlanModel();
    public bool mSidebar = false;

    public void OnInitialize(string pProfileUsername, string pProfileID)
    {
        mSidebar = false;
        this.mProfileUsername = pProfileUsername;
        this.mProfileID = pProfileID;
        this.mUserStatsModel = new userStatsModel();
        LoadPlanModel();

        PreferenceManager.Instance.SetString("login_username", pProfileUsername);
        bool mContinueWeeklyPlan = PreferenceManager.Instance.GetBool("ContinuePlan", false);
        string mStartingDate = PreferenceManager.Instance.GetString("DateRangeStart", DateTime.Now.ToString());
        string mEndingDate = PreferenceManager.Instance.GetString("DateRangeEnd", DateTime.Now.ToString());
        this.mUserStatsModel.OnInitialize(pContinueWeeklyPlan: mContinueWeeklyPlan, pStartingDate: HelperMethods.Instance.ParseDateString(mStartingDate), pEndingDate: HelperMethods.Instance.ParseDateString(mEndingDate));
    }

    public void OnResetSession()
    {
        this.mProfileUsername = null;
        this.mProfileID = null;

    }

    public void createPlan(bool pContinuePlan, string pDateRangeStartText, string pDateRangeEndText)
    {
        PreferenceManager.Instance.SetBool("FirstTimePlanInitialized_" + userSessionManager.Instance.mProfileUsername, true);
        PreferenceManager.Instance.SetBool("ContinuePlan", pContinuePlan);
        PreferenceManager.Instance.SetString("DateRangeStart", pDateRangeStartText);
        PreferenceManager.Instance.SetString("DateRangeEnd", pDateRangeEndText);

        string mStartingDate = PreferenceManager.Instance.GetString("DateRangeStart", DateTime.Now.ToString());
        string mEndingDate = PreferenceManager.Instance.GetString("DateRangeEnd", DateTime.Now.ToString());

        userSessionManager.Instance.mUserStatsModel.OnInitialize(pContinueWeeklyPlan: pContinuePlan, pStartingDate: HelperMethods.Instance.ParseDateString(mStartingDate), pEndingDate: HelperMethods.Instance.ParseDateString(mEndingDate));
    }

    public void SavePlanModel()
    {
        string json = JsonConvert.SerializeObject(mPlanModel, Formatting.Indented);
        PlayerPrefs.SetString(mProfileUsername, json);
        PlayerPrefs.Save();
    }

    public void LoadPlanModel()
    {
        string json = PlayerPrefs.GetString(mProfileUsername);
        if (!string.IsNullOrEmpty(json))
        {
            mPlanModel = JsonConvert.DeserializeObject<PlanModel>(json);
        }
        else
        {
            mPlanModel = new PlanModel();
        }
    }

    public void RemovePlanModel()
    {
        if (PlayerPrefs.HasKey(mProfileUsername))
        {
            PlayerPrefs.DeleteKey(mProfileUsername);
            PlayerPrefs.Save();
            mPlanModel = new PlanModel();
        }
    }
}

