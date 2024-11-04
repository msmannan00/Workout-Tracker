using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDataItem : MonoBehaviour,ItemController
{
    public List<Image> trophyImages = new List<Image>();
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI coinText;
    public AchievementTemplate _data;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        _data = (AchievementTemplate)data["data"];
        //print(_data.type);
        bool isRank = (bool)data["rank"];
        titleText.text = _data.title;
        CheckAchievement();
    }
    void CheckAchievement()
    {
        switch (_data.type)
        {
            case AchievementType.BodyweightMultiplier:
                userSessionManager.Instance.CheckBodyWeightAchievements(_data, ApiDataHandler.Instance.getPersonalBestData(), trophyImages, progressText, descriptionText);
                break;
            case AchievementType.WorkoutCount:
                userSessionManager.Instance.CheckWorkoutCountAchievements(_data, ApiDataHandler.Instance.getHistoryData().exerciseTempleteModel.Count, trophyImages, progressText, descriptionText);
                break;
            case AchievementType.ExerciseCount:
                userSessionManager.Instance.CheckExerciseCountAchievements(_data, userSessionManager.Instance.GetUniqueExerciseCount(ApiDataHandler.Instance.getHistoryData()), trophyImages, progressText, descriptionText);
                break;
            case AchievementType.Specialist:
                userSessionManager.Instance.CheckSpecialistAchievements(_data, ApiDataHandler.Instance.getHistoryData(), trophyImages, progressText, descriptionText);
                break;
            case AchievementType.CardioTime:
                userSessionManager.Instance.CheckCardioTimeAchievements(_data, ApiDataHandler.Instance.getHistoryData(), trophyImages, progressText, descriptionText);
                break;
            case AchievementType.Streak:
                userSessionManager.Instance.CheckStreakAndLevelAchievements(_data, ApiDataHandler.Instance.GetUserStreak(), trophyImages, progressText, descriptionText);
                break;
            case AchievementType.LevelUp:
                userSessionManager.Instance.CheckStreakAndLevelAchievements(_data, ApiDataHandler.Instance.GetUserLevel(), trophyImages, progressText, descriptionText);
                break;
        }
    }

    public void CheckBodyWeightAchievements(AchievementTemplate data, PersonalBestData personalBest)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];

            if (achievementDataItem.isCompleted)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            int totalWeight = 0;
            foreach (string exerciseName in data.category_exercise)
            {
                PersonalBestDataItem matchingExercise = personalBest.exercises.Find(exercise => exercise.exerciseName.ToLower() == exerciseName.ToLower());
                if (matchingExercise != null)
                {
                    totalWeight += matchingExercise.weight;
                }
            }
            float value = achievementDataItem.value * userSessionManager.Instance.currentWeight;
            if (totalWeight >= (int)value)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                achievementDataItem.isCompleted = true;
            }
            else
            {
                progressText.text = totalWeight.ToString() + " / " + value.ToString();
                descriptionText.text = achievementDataItem.description;
                return;
            }
        }
        progressText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
        //descriptionText.text = _data.achievementData[_data.achievementData.Count-1].description;
    }

    public void CheckWorkoutCountAchievements(AchievementTemplate data, int performedWorkouts)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];

            if (achievementDataItem.isCompleted)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (performedWorkouts >= achievementDataItem.value)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                achievementDataItem.isCompleted = true;
            }
            else
            {
                progressText.text = performedWorkouts.ToString() + " / " + achievementDataItem.value.ToString();
                descriptionText.text = achievementDataItem.description;
                return;
            }
        }
        descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckExerciseCountAchievements(AchievementTemplate data, int performedExercises)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];

            if (achievementDataItem.isCompleted)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (performedExercises >= achievementDataItem.value)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                achievementDataItem.isCompleted = true;
            }
            else
            {
                progressText.text = performedExercises.ToString() + " / " + achievementDataItem.value.ToString();
                descriptionText.text = achievementDataItem.description;
                return;
            }
        }
        descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckSpecialistAchievements(AchievementTemplate data, HistoryModel historyModel)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];
            int performed = userSessionManager.Instance.GetPerformancedExercisesByCategory(historyModel, data.category_exercise);
            if (achievementDataItem.isCompleted)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if ( performed >= achievementDataItem.value)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                achievementDataItem.isCompleted = true;
            }
            else
            {
                progressText.text = performed.ToString() + " / " + achievementDataItem.value.ToString();
                descriptionText.text = achievementDataItem.description;
                return;
            }
        }
        descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckCardioTimeAchievements(AchievementTemplate data, HistoryModel historyModel)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];
            int performed = userSessionManager.Instance.GetTotalPerformancedExerciseTime(historyModel, data.category_exercise);
            float performedTime = (float)performed / 60;
            if (achievementDataItem.isCompleted)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (performedTime >= (float)achievementDataItem.value/60)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                achievementDataItem.isCompleted = true;
            }
            else
            {
                progressText.text = performedTime.ToString() + " / " + ((float)achievementDataItem.value / 60).ToString();
                descriptionText.text = achievementDataItem.description;
                return;
            }
        }
        descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }
    public void CheckStreakAchievements(AchievementTemplate data, int streak)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];
            if (achievementDataItem.isCompleted)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (streak >= achievementDataItem.value)
            {
                trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                achievementDataItem.isCompleted = true;
            }
            else
            {
                progressText.text = streak.ToString() + " / " + (achievementDataItem.value).ToString();
                descriptionText.text = achievementDataItem.description;
                return;
            }
        }
        descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public static int GetUniqueExerciseCount(HistoryModel historyModel)
    {
        HashSet<string> uniqueExercises = new HashSet<string>();
        foreach (var template in historyModel.exerciseTempleteModel)
        {
            foreach (var exerciseType in template.exerciseTypeModel)
            {
                uniqueExercises.Add(exerciseType.exerciseName);
            }
        }
        return uniqueExercises.Count;
    }

    public int GetPerformancedExercisesByCategory(HistoryModel myHistory, List<string> categoryNames)
    {
        int totalCount = 0;
        foreach (var item in categoryNames)
        {
            item.ToLower();
        }
        foreach (HistoryTempleteModel historyTemplate in myHistory.exerciseTempleteModel)
        {
            foreach (HistoryExerciseTypeModel exerciseType in historyTemplate.exerciseTypeModel)
            {
                if (categoryNames.Contains(userSessionManager.Instance.SplitString(exerciseType.categoryName).ToLower()))
                {
                    totalCount++;
                }
            }
        }

        return totalCount;
    }

    public int GetTotalPerformancedExerciseTime(HistoryModel myHistory, List<string> categoryNames)
    {
        int totalTime = 0;
        foreach (var item in categoryNames)
        {
            item.ToLower();
        }
        // Loop through each exercise template in the history
        foreach (HistoryTempleteModel historyTemplate in myHistory.exerciseTempleteModel)
        {
            // Loop through each exercise type in the template
            foreach (HistoryExerciseTypeModel exerciseType in historyTemplate.exerciseTypeModel)
            {
                // Check if the exercise category matches any category in the achievement template
                if (categoryNames.Contains(exerciseType.categoryName.ToLower()))
                {
                    // Loop through each exercise model and sum its time
                    foreach (HistoryExerciseModel exercise in exerciseType.exerciseModel)
                    {
                        totalTime += exercise.time;
                    }
                }
            }
        }

        return totalTime;
    }
    public static string SplitString(string inputList)
    {
        string resultList;

        if (inputList.Contains('/'))
        {
            // Split the string and add the parts to the result list
            string[] splitParts = inputList.Split('/');
            resultList = splitParts[0];
            return resultList;
        }
        else
        {
            resultList = inputList;
        }

        return resultList;
    }
}
