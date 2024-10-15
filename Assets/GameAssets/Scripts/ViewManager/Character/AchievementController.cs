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
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        trophysText.text = "Trophies " + DataManager.Instance.GetCompletedTrophys().ToString() + " / " + DataManager.Instance.GetTotalTrophys().ToString();
        completedText.text = "Completed " + DataManager.Instance.GetCompletedAchievements().ToString() + " / " + DataManager.Instance.GetTotalAchievements().ToString();
        achievementData = DataManager.Instance.getAchievementData();
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
    }

    public void Back()
    {
        DataManager.Instance.SaveAchievementData();
        StateManager.Instance.HandleBackAction(gameObject);
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