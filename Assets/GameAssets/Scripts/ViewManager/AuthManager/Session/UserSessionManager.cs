using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.UI;

public class userSessionManager : GenericSingletonClass<userSessionManager>
{
    private const float lbsToKgFactor = 0.453592f;
    private const float kgToLbsFactor = 2.20462f;
    public string mProfileUsername;
    public string mProfileID;
    public bool mSidebar = false;
    public int currentWeight;
    public GameObject currentScreen;
    [Header("Theme Settings")]
    private Theme gameTheme;
    public TMP_FontAsset darkPrimaryFont, darkSecondaryFont;
    public TMP_FontAsset lightPrimaryFont, lightSecondaryFont;

    public Color darkButtonTextColor,lightButtonTextColor;
    public Color darkButtonColor,lightButtonColor;
    public Color darkPlaceholder, lightPlaceholder;
    public Color darkInputFieldColor, lightInputFieldColor;
    public Color darkLineColor;
    public Color darkSwitchTextColor;

    public Color lightXPbutton, darkXPbutton;

    public DefaultTempleteModel selectedTemplete;
    private TemplateData templateData = new TemplateData();
    private HistoryModel historyData = new HistoryModel();
    //public PersonalBest

    private void Start()
    {
        darkPrimaryFont= Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Hoog0555/Hoog0555");
        darkSecondaryFont = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/K2D/K2D");
        lightPrimaryFont = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Alexandria/Alexandria");
        lightSecondaryFont = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Afacad/Afacad");
        darkButtonTextColor = new Color32(51,23,23,255);
        lightButtonTextColor = new Color32(150,0,0,255);
        darkButtonColor = new Color32(255,255,255,255);
        lightButtonColor = new Color32(218,52,52,255);
        darkPlaceholder = new Color32(127,77,77,255);
        lightPlaceholder = new Color32(92, 59, 28, 155);
        darkInputFieldColor = new Color32(81,14,14,255);
        lightInputFieldColor = new Color32(246,236,220,255);
        darkLineColor = new Color32(246, 236, 220, 85);
        darkSwitchTextColor = new Color32(171, 162, 162, 255);
        lightXPbutton = new Color32(167, 200, 33, 255);
        darkXPbutton = new Color32(241, 183, 32, 255);

    }

    public void OnInitialize(string pProfileUsername, string pProfileID)
    {
        this.mProfileUsername = pProfileUsername;
        this.mProfileID = pProfileID;
        PreferenceManager.Instance.SetString("login_username", pProfileUsername);
        mSidebar = false;

        //LoadHistory();
        //gameTheme = LoadTheme();

    }

    public void OnResetSession()
    {
        this.mProfileUsername = null;
        this.mProfileID = null;
    }

    // Sets the current week's attendance dates
    public void AddGymVisit()
    {
        // Store gym visit with the current date
        string visitKey = "GymVisit_" + ApiDataHandler.Instance.GetCurrentWeekStartDate().ToString("yyyy-MM-dd");

        // Get the stored visit dates for the current week
        List<string> visits = PreferenceManager.Instance.GetStringList(visitKey) ?? new List<string>();

        // Get today's date in string format
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        // Add today's date if it's not already recorded
        if (!visits.Contains(today))
        {
            visits.Add(today);
            PreferenceManager.Instance.SetStringList(visitKey, visits);
        }
        UpdateStreak();
        
    }
    public bool HasMetWeeklyGoal()
    {
        // Get weekly goal
        int weeklyGoal = ApiDataHandler.Instance.GetWeeklyGoal();

        // Get the current week's attendance
        string visitKey = "GymVisit_" + ApiDataHandler.Instance.GetCurrentWeekStartDate().ToString("yyyy-MM-dd");
        List<string> visits = PreferenceManager.Instance.GetStringList(visitKey) ?? new List<string>();

        // Check if the user has met their weekly goal
        return visits.Count >= weeklyGoal;
    }
    public void UpdateStreak()
    {
        // Check and update the current week's start date if needed
        ApiDataHandler.Instance.CheckAndUpdateWeekStartDate();

        // Get the stored week start date and the current week start date
        DateTime lastWeekStartDate = ApiDataHandler.Instance.GetCurrentWeekStartDate();
        DateTime currentWeekStartDate = ApiDataHandler.Instance.GetStartOfCurrentWeek();

        // Check if it's a new week
        if (currentWeekStartDate > lastWeekStartDate)
        {
            int level = ApiDataHandler.Instance.GetCharacterLevel();
            // If the user met the weekly goal, increase the streak
            if (HasMetWeeklyGoal())
            {
                int currentStreak = ApiDataHandler.Instance.GetUserStreak();
                ApiDataHandler.Instance.SetUserStreak(currentStreak + 1);
                level++;
                ApiDataHandler.Instance.SetCharacterLevel(level);
            }
            else
            {
                // Reset streak if the user failed to meet the weekly goal
                ApiDataHandler.Instance.SetUserStreak(0);
                if (level > 1)
                {
                    level--;
                    ApiDataHandler.Instance.SetCharacterLevel(level);
                }
            }
            // Update the date when the weekly goal was last set
            ApiDataHandler.Instance.SetCurrentWeekStartDate(currentWeekStartDate);
        }
    }





    public void CheckAchievementStatus()
    {

        foreach(AchievementTemplate _data in ApiDataHandler.Instance.getAchievementData().achievements)
        {
            switch (_data.type)
            {
                case AchievementType.BodyweightMultiplier:
                    CheckBodyWeightAchievements(_data, ApiDataHandler.Instance.getPersonalBestData(), null, null, null);
                    break;
                case AchievementType.WorkoutCount:
                    CheckWorkoutCountAchievements(_data, ApiDataHandler.Instance.getHistoryData().exerciseTempleteModel.Count, null, null, null);
                    break;
                case AchievementType.ExerciseCount:
                    CheckExerciseCountAchievements(_data, GetUniqueExerciseCount(ApiDataHandler.Instance.getHistoryData()), null, null, null);
                    break;
                case AchievementType.Specialist:
                    CheckSpecialistAchievements(_data, ApiDataHandler.Instance.getHistoryData(), null, null, null);
                    break;
                case AchievementType.CardioTime:
                    CheckCardioTimeAchievements(_data, ApiDataHandler.Instance.getHistoryData(), null, null, null);
                    break;
                case AchievementType.Streak:
                    CheckStreakAndLevelAchievements(_data, ApiDataHandler.Instance.GetUserStreak(), null, null, null);
                    break;
                case AchievementType.LevelUp:
                    CheckStreakAndLevelAchievements(_data, ApiDataHandler.Instance.GetCompletedAchievements(), null, null, null);
                    break;
            }
        }
        ApiDataHandler.Instance.SaveAchievementData();
        
    }

    public void CheckBodyWeightAchievements(AchievementTemplate data, PersonalBestData personalBest,List<Image> trophyImages,TextMeshProUGUI progressText,TextMeshProUGUI descriptionText)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];

            if (achievementDataItem.isCompleted)
            {
                if(trophyImages != null)
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
            float value = achievementDataItem.value * ApiDataHandler.Instance.getMeasurementData().weight;
            if (totalWeight >= (int)value)
            {
                achievementDataItem.isCompleted = true;
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                else
                {
                    List<object> initialData = new List<object> { data.title, achievementDataItem.description, true };
                    PopupController.Instance.OpenPopup("shared", "AchievementCompletePopup", null, initialData);
                }
            }
            else
            {
                if (progressText != null && descriptionText != null)
                {
                    progressText.text = totalWeight.ToString() + "kg / " + value.ToString()+"kg";
                    descriptionText.text = achievementDataItem.description;
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count-1].description;
    }

    public void CheckWorkoutCountAchievements(AchievementTemplate data, int performedWorkouts, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];

            if (achievementDataItem.isCompleted)
            {
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (performedWorkouts >= achievementDataItem.value)
            {
                achievementDataItem.isCompleted = true;
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                else
                {
                    List<object> initialData = new List<object> { data.title, achievementDataItem.description, false };
                    PopupController.Instance.OpenPopup("shared", "AchievementCompletePopup", null, initialData);
                }
            }
            else
            {
                if (progressText != null && descriptionText != null)
                {
                    progressText.text = performedWorkouts.ToString() + " / " + achievementDataItem.value.ToString();
                    descriptionText.text = achievementDataItem.description;
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            progressText.gameObject.SetActive(false);
            descriptionText.text= "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckExerciseCountAchievements(AchievementTemplate data, int performedExercises, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];

            if (achievementDataItem.isCompleted)
            {
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (performedExercises >= achievementDataItem.value)
            {
                achievementDataItem.isCompleted = true;
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                else
                {
                    List<object> initialData = new List<object> { data.title, achievementDataItem.description, false };
                    PopupController.Instance.OpenPopup("shared", "AchievementCompletePopup", null, initialData);
                }
            }
            else
            {
                if (progressText != null && descriptionText != null)
                {
                    progressText.text = performedExercises.ToString() + " / " + achievementDataItem.value.ToString();
                    descriptionText.text = achievementDataItem.description;
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckSpecialistAchievements(AchievementTemplate data, HistoryModel historyModel, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];
            int performed = GetPerformancedExercisesByCategory(historyModel, data.category_exercise);
            if (achievementDataItem.isCompleted)
            {
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (performed >= achievementDataItem.value)
            {
                achievementDataItem.isCompleted = true;
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                else
                {
                    List<object> initialData = new List<object> { data.title, achievementDataItem.description, false };
                    PopupController.Instance.OpenPopup("shared", "AchievementCompletePopup", null, initialData);
                }
            }
            else
            {
                if (progressText != null && descriptionText != null)
                {
                    progressText.text = performed.ToString() + " / " + achievementDataItem.value.ToString();
                    descriptionText.text = achievementDataItem.description;
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckCardioTimeAchievements(AchievementTemplate data, HistoryModel historyModel, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];
            int performed = GetTotalPerformancedExerciseTime(historyModel, data.category_exercise);
            float performedTime = (float)performed / 60;
            if (achievementDataItem.isCompleted)
            {
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (performedTime >= (float)achievementDataItem.value / 60)
            {
                achievementDataItem.isCompleted = true;
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                else
                {
                    List<object> initialData = new List<object> { data.title, achievementDataItem.description, false };
                    PopupController.Instance.OpenPopup("shared", "AchievementCompletePopup", null, initialData);
                }
            }
            else
            {
                if (progressText != null && descriptionText != null)
                {
                    progressText.text = performedTime.ToString() + " / " + ((float)achievementDataItem.value / 60).ToString();
                    descriptionText.text = achievementDataItem.description;
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }
    public void CheckStreakAndLevelAchievements(AchievementTemplate data, int streak, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText)
    {
        for (int i = 0; i < data.achievementData.Count; i++)
        {
            AchievementTemplateDataItem achievementDataItem = data.achievementData[i];
            if (achievementDataItem.isCompleted)
            {
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                continue; // Skip if it's already completed
            }
            if (streak >= achievementDataItem.value)
            {
                achievementDataItem.isCompleted = true;
                if (trophyImages != null)
                    trophyImages[i].transform.GetChild(0).gameObject.SetActive(true);
                else
                {
                    List<object> initialData = new List<object> { data.title, achievementDataItem.description, false };
                    PopupController.Instance.OpenPopup("shared", "AchievementCompletePopup", null, initialData);
                }
            }
            else
            {
                if (progressText != null && descriptionText != null)
                {
                    progressText.text = streak.ToString() + " / " + (achievementDataItem.value).ToString();
                    descriptionText.text = achievementDataItem.description;
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public int GetUniqueExerciseCount(HistoryModel historyModel)
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
                if (categoryNames.Contains(SplitString(exerciseType.categoryName).ToLower()))
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
    public string SplitString(string inputList)
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

    public float ConvertLbsToKg(float pounds)
    {
        return pounds * lbsToKgFactor;
    }
    public float ConvertKgToLbs(float kilograms)
    {
        return kilograms * kgToLbsFactor;
    }

}
