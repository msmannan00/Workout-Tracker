using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour, PageController
{
    public TextMeshProUGUI trophysText;
    public TextMeshProUGUI completedText;
    public RectTransform selectionLine;
    public Transform content;
    AchievementData achievementData;
    List<GameObject> rankAchievement = new List<GameObject>();
    List<GameObject> milestoneAchievement = new List<GameObject>();
    bool onFooter;
    bool backAction;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        onFooter = (bool)data["onFooter"];
        backAction = (bool)data["backAction"];
        achievementData = ApiDataHandler.Instance.getAchievementData();
        SetCompleteAndTrophiesTextForRank();
        Rank();
    }

    public void Rank()
    {
        GlobalAnimator.Instance.AnimateRectTransformX(selectionLine, -100f, 0.25f);
        if (milestoneAchievement.Count != 0)
        {
            foreach (GameObject obj in milestoneAchievement) { obj.SetActive(false); }
        }
        if (rankAchievement.Count != 0)
        {
            foreach(GameObject obj in rankAchievement) { obj.SetActive(true); }
            SetCompleteAndTrophiesTextForRank();
            return;
        }
        List<AchievementTemplate> rankAchievements = GetAchievementsForRank();
        foreach (AchievementTemplate item in rankAchievements)
        {
            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/character/multipleAchievementDataItem");
            GameObject newItem = Instantiate(exercisePrefab, content);
            Dictionary<string, object> initData = new Dictionary<string, object>
            {
                {  "data", item},{"rank",true   }
            };
            newItem.GetComponent<ItemController>().onInit(initData, null);
            rankAchievement.Add(newItem);
        }
        SetCompleteAndTrophiesTextForRank();
    }
    public void Milestone()
    {
        GlobalAnimator.Instance.AnimateRectTransformX(selectionLine, 100f, 0.25f);
        if (rankAchievement.Count != 0)
        {
            foreach (GameObject obj in rankAchievement) { obj.SetActive(false); }
        }
        if (milestoneAchievement.Count != 0)
        {
            foreach (GameObject obj in milestoneAchievement) { obj.SetActive(true); }
            SetCompleteAndTrophiesForMilestone();
            return;
        }
        List<AchievementTemplate> rankAchievements = GetAchievementsForMilestone();
        foreach (AchievementTemplate item in rankAchievements)
        {
            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/character/singleAchievementDataItem");
            GameObject newItem = Instantiate(exercisePrefab, content);
            Dictionary<string, object> initData = new Dictionary<string, object>
            {
                {  "data", item},{"rank",true   }
            };
            newItem.GetComponent<ItemController>().onInit(initData, null);
            milestoneAchievement.Add(newItem);
        }
        SetCompleteAndTrophiesForMilestone();
    }
    void SetCompleteAndTrophiesForMilestone()
    {
        (int completeTrophies, int totalTrophies) = ApiDataHandler.Instance.GetMilestoneCompletedTrophys();
        trophysText.text = "Trophies " + completeTrophies.ToString() + " / " + totalTrophies.ToString();
        (int completeAchievemet, int totalAchievement) = ApiDataHandler.Instance.GetMilestoneCompletedAchievements();
        completedText.text = "Completed " + completeAchievemet.ToString() + " / " + totalAchievement.ToString();
    }
    void SetCompleteAndTrophiesTextForRank()
    {
        (int completeTrophies, int totalTrophies) = ApiDataHandler.Instance.GetRankedCompletedTrophys();
        trophysText.text = "Trophies " + completeTrophies.ToString() + " / " + totalTrophies.ToString();
        (int completeAchievemet, int totalAchievement) = ApiDataHandler.Instance.GetRankedCompletedAchievements();
        completedText.text = "Completed " + completeAchievemet.ToString() + " / " + totalAchievement.ToString();
    }
    public void Back()
    {
        ApiDataHandler.Instance.SaveAchievementData();
        StateManager.Instance.HandleBackAction(gameObject);
        if(onFooter)
            StateManager.Instance.OpenFooter(null, null, false);
    }
    public List<AchievementTemplate> GetAchievementsForMilestone()
    {
        return achievementData.achievements.Where(achievement => achievement.achievementData.Count == 1).ToList();
    }

    public List<AchievementTemplate> GetAchievementsForRank()
    {
        return achievementData.achievements.Where(achievement => achievement.achievementData.Count > 1).ToList();
    }
}