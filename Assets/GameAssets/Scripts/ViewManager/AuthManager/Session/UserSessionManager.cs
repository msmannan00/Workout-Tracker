using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

public class userSessionManager : GenericSingletonClass<userSessionManager>
{
    private const float lbsToKgFactor = 0.453592f;
    private const float kgToLbsFactor = 2.20462f;
    public string mProfileUsername;
    public string mProfileID;
    public bool mSidebar = false;
    public GameObject currentScreen;
    public int weeklyGoal;
    public string joiningDate;
    public string badgeName;
    public int currentCoins;
    public int userStreak;
    public int characterLevel;
    public string gifsPath = "gifs/";

    [Header("Theme Settings")]
    private Theme gameTheme;
    public TMP_FontAsset darkPrimaryFont, darkSecondaryFont;
    public TMP_FontAsset lightPrimaryFontBold, lightPrimaryFontMediumBold, lightPrimaryFontSemiBold, lightSecondaryFont;

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
        lightPrimaryFontBold = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Alexandria/AlexandriaBold");
        lightPrimaryFontMediumBold = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Alexandria/AlexandriaMediumBold");
        lightPrimaryFontSemiBold = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Alexandria/AlexandriaSemiBold");
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
    public void CheckAchievementStatus(List<Image> trophyImages=null, TextMeshProUGUI progressText=null, TextMeshProUGUI descriptionText = null, TextMeshProUGUI coinText=null)
    {
        foreach(AchievementTemplate _data in ApiDataHandler.Instance.getAchievementData().achievements)
        {
            switch (_data.type)
            {
                case AchievementType.BodyweightMultiplier:
                    CheckBodyWeightAchievements(_data, ApiDataHandler.Instance.getPersonalBestData(), trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.WorkoutCount:
                    CheckWorkoutCountAchievements(_data, ApiDataHandler.Instance.getHistoryData().exerciseTempleteModel.Count, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.ExerciseCount:
                    CheckExerciseCountAchievements(_data, GetUniqueExerciseCount(ApiDataHandler.Instance.getHistoryData()), trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.Specialist:
                    CheckSpecialistAchievements(_data, ApiDataHandler.Instance.getHistoryData(), trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.CardioTime:
                    CheckCardioTimeAchievements(_data, ApiDataHandler.Instance.getHistoryData(), trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.Streak:
                    CheckStreakAndLevelAchievements(_data, userSessionManager.Instance.userStreak, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.LevelUp:
                    CheckStreakAndLevelAchievements(_data, userSessionManager.Instance.characterLevel, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.CompleteAllAchievements:
                    CheckCompleteAllAchivements(_data, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.LongSession:
                    CheckLongSessionAchivements(_data, GetHighestCompletedTimeInHours(ApiDataHandler.Instance.getHistoryData()), trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.ExercisesInSingleSession:
                    CheckAchievements(_data, GetHighestExerciseInSingleSession(ApiDataHandler.Instance.getHistoryData()), AchievementType.ExercisesInSingleSession, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.ChangeTrainingBadge:
                    ChangeTrainingBadgeAchievements(_data, badgeName, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.AddFriends:
                    CheckAchievements(_data, ApiDataHandler.Instance.GetAddFriendCount(), AchievementType.AddFriends, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.RemoveFriend:
                    CheckAchievements(_data, ApiDataHandler.Instance.GetRemoveFriendCount(), AchievementType.RemoveFriend, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.AddPersonBest:
                    CheckAchievements(_data, GetExercisesWithWeightGreaterThanZero(ApiDataHandler.Instance.getPersonalBestData()), AchievementType.AddPersonBest, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.CreateWorkout:
                    CheckAchievements(_data, ApiDataHandler.Instance.GetCreatedWorkoutTempleteCount(), AchievementType.CreateWorkout, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.WorkoutInactivity:
                    CheckAchievements(_data, GetGapBetweenLatestTwoWorkoutsInDays(ApiDataHandler.Instance.getHistoryData()), AchievementType.WorkoutInactivity, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.BuyItems:
                    CheckAchievements(_data, ApiDataHandler.Instance.GetBuyedCloths(), AchievementType.BuyItems, trophyImages, progressText, descriptionText, coinText);
                    break;
                case AchievementType.WeightLifted:
                    CheckAchievements(_data, GetTotalWeightInTons(ApiDataHandler.Instance.getHistoryData()), AchievementType.WeightLifted, trophyImages, progressText, descriptionText, coinText);
                    break;
            }
        }
        //ApiDataHandler.Instance.SaveAchievementData();
        
    }
    
    
    public void CheckIndiviualAchievementStatus(AchievementTemplate _data, List<Image> trophyImages = null, TextMeshProUGUI progressText = null, TextMeshProUGUI descriptionText = null,TextMeshProUGUI coinText=null)
    {
        switch (_data.type)
        {
            case AchievementType.BodyweightMultiplier:
                CheckBodyWeightAchievements(_data, ApiDataHandler.Instance.getPersonalBestData(), trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.WorkoutCount:
                CheckWorkoutCountAchievements(_data, ApiDataHandler.Instance.getHistoryData().exerciseTempleteModel.Count, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.ExerciseCount:
                CheckExerciseCountAchievements(_data, GetUniqueExerciseCount(ApiDataHandler.Instance.getHistoryData()), trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.Specialist:
                CheckSpecialistAchievements(_data, ApiDataHandler.Instance.getHistoryData(), trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.CardioTime:
                CheckCardioTimeAchievements(_data, ApiDataHandler.Instance.getHistoryData(), trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.Streak:
                CheckStreakAndLevelAchievements(_data, userStreak, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.LevelUp:
                CheckStreakAndLevelAchievements(_data, characterLevel, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.CompleteAllAchievements:
                CheckCompleteAllAchivements(_data, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.LongSession:
                CheckLongSessionAchivements(_data, GetHighestCompletedTimeInHours(ApiDataHandler.Instance.getHistoryData()), trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.ExercisesInSingleSession:
                CheckAchievements(_data, GetHighestExerciseInSingleSession(ApiDataHandler.Instance.getHistoryData()), AchievementType.ExercisesInSingleSession, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.ChangeTrainingBadge:
                ChangeTrainingBadgeAchievements(_data, badgeName, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.AddFriends:
                CheckAchievements(_data, ApiDataHandler.Instance.GetAddFriendCount(), AchievementType.AddFriends, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.RemoveFriend:
                CheckAchievements(_data, ApiDataHandler.Instance.GetRemoveFriendCount(), AchievementType.RemoveFriend, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.AddPersonBest:
                CheckAchievements(_data, GetGapBetweenLatestTwoWorkoutsInDays(ApiDataHandler.Instance.getHistoryData()), AchievementType.AddPersonBest, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.CreateWorkout:
                CheckAchievements(_data, ApiDataHandler.Instance.GetCreatedWorkoutTempleteCount(), AchievementType.CreateWorkout, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.WorkoutInactivity:
                CheckAchievements(_data, GetGapBetweenLatestTwoWorkoutsInDays(ApiDataHandler.Instance.getHistoryData()), AchievementType.WorkoutInactivity, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.BuyItems:
                CheckAchievements(_data, ApiDataHandler.Instance.GetBuyedCloths(), AchievementType.BuyItems, trophyImages, progressText, descriptionText, coinText);
                break;
            case AchievementType.WeightLifted:
                CheckAchievements(_data, GetTotalWeightInTons(ApiDataHandler.Instance.getHistoryData()), AchievementType.WeightLifted, trophyImages, progressText, descriptionText, coinText);
                break;
        }
     
        //ApiDataHandler.Instance.SaveAchievementData();

    }

    //----------------------------------------------------------------------------------------------------------------------------------

    public void CheckBodyWeightAchievements(AchievementTemplate data, PersonalBestData personalBest,List<Image> trophyImages,TextMeshProUGUI progressText,TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
                AddCoins(achievementDataItem.coins);
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
                    coinText.text=achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count-1].description;
    }

    public void CheckWorkoutCountAchievements(AchievementTemplate data, int performedWorkouts, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
                AddCoins(achievementDataItem.coins);
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
                    coinText.text=achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text= "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckExerciseCountAchievements(AchievementTemplate data, int performedExercises, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
                AddCoins(achievementDataItem.coins);
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
                    coinText.text=achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckSpecialistAchievements(AchievementTemplate data, HistoryModel historyModel, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
                AddCoins(achievementDataItem.coins);
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
                    coinText.text = achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    public void CheckCardioTimeAchievements(AchievementTemplate data, HistoryModel historyModel, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
                AddCoins(achievementDataItem.coins);
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
                    coinText.text=achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }
    public void CheckStreakAndLevelAchievements(AchievementTemplate data, int streak, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
                AddCoins(achievementDataItem.coins);
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
                    coinText.text = achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }
    public void CheckCompleteAllAchivements(AchievementTemplate data, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
            int totalAchievements = ApiDataHandler.Instance.GetTotalAchievements() - 1;
            int completeAchievements = ApiDataHandler.Instance.GetCompletedAchievements(ApiDataHandler.Instance.getAchievementData());
            if (completeAchievements==totalAchievements)
            {
                achievementDataItem.isCompleted = true;
                AddCoins(achievementDataItem.coins);
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
                    progressText.text = completeAchievements + " / " + totalAchievements.ToString();
                    descriptionText.text = achievementDataItem.description;
                    coinText.text = achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
    }

    public void CheckLongSessionAchivements(AchievementTemplate data, int longestSession, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
            if (longestSession >= achievementDataItem.value)
            {
                achievementDataItem.isCompleted = true;
                AddCoins(achievementDataItem.coins);
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
                    progressText.text = longestSession + "H / " + achievementDataItem.value.ToString()+"H";
                    descriptionText.text = achievementDataItem.description;
                    coinText.text = achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
    }
    public void ChangeTrainingBadgeAchievements(AchievementTemplate data, string currentBadgeName, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
            if (currentBadgeName != "TheGorillaBadge")
            {
                achievementDataItem.isCompleted = true;
                AddCoins(achievementDataItem.coins);
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
                    progressText.text = "0 / " + achievementDataItem.value.ToString();
                    descriptionText.text = achievementDataItem.description;
                    coinText.text = achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
    }
    public void CheckAchievements(AchievementTemplate data, int performedExercises, AchievementType type, List<Image> trophyImages, TextMeshProUGUI progressText, TextMeshProUGUI descriptionText, TextMeshProUGUI coinText)
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
                AddCoins(achievementDataItem.coins);
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
                    switch (type)
                    {
                        case AchievementType.WeightLifted:
                            progressText.text = performedExercises.ToString() + "T / " + achievementDataItem.value.ToString()+"T";                          
                            break;
                        default:
                            progressText.text = performedExercises.ToString() + " / " + achievementDataItem.value.ToString();                         
                            break;
                    }
                    descriptionText.text = achievementDataItem.description;
                    coinText.text = achievementDataItem.coins.ToString();
                }
                return;
            }
        }
        if (progressText != null && descriptionText != null)
        {
            coinText.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            descriptionText.text = "Congratulations! You've reached peak performance – keep the momentum going!";
        }
        //descriptionText.text = _data.achievementData[_data.achievementData.Count - 1].description;
    }

    //------------------------------------------------------Helper Functions----------------------------------------------------------------------------------

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

    public int GetHighestCompletedTimeInHours(HistoryModel history)
    {
        if (history.exerciseTempleteModel.Count < 2)
            return 0;
        int highestCompletedTimeInHours = history.exerciseTempleteModel
            .Max(template => template.completedTime / 3600); // Divide seconds by 3600 to get hours

        return highestCompletedTimeInHours;
    }
    public int GetHighestExerciseInSingleSession(HistoryModel history)
    {
        if (history.exerciseTempleteModel.Count < 2)
            return 0;
        // Get the highest count of exerciseTypeModel in the list
        int highestCount = history.exerciseTempleteModel
            .Max(template => template.exerciseTypeModel.Count);

        return highestCount;
    }
    public int GetTotalWeightInTons(HistoryModel history)
    {
        float totalWeightInTons = history.exerciseTempleteModel
            .Sum(template => template.totalWeight) / 1000f;

        return (int)totalWeightInTons;
    }
    public int GetGapBetweenLatestTwoWorkoutsInDays(HistoryModel history)
    {
        try
        {
            // Parse and sort the dateTime strings into DateTime objects
            var sortedDates = history.exerciseTempleteModel
                .Where(template => DateTime.TryParse(template.dateTime, out _))
                .Select(template => DateTime.Parse(template.dateTime))
                .OrderByDescending(date => date)
                .ToList();

            // Ensure there are at least two valid dates
            if (sortedDates.Count < 2)
            {
                Debug.LogWarning("Not enough valid dates to calculate the gap.");
                return 0;
            }

            // Calculate the gap in days between the two latest dates
            int gapInDays = (sortedDates[0] - sortedDates[1]).Days;

            return gapInDays;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error calculating gap: {ex.Message}");
            return 0; // Return -1 to indicate an error
        }
    }
    public int GetExercisesWithWeightGreaterThanZero(PersonalBestData personalBestData)
    {
        if(personalBestData.exercises.Count>0)
            return personalBestData.exercises.Count(exercise => exercise.weight > 0);
        else 
            return 0;
    }
    public void AddCoins(int coins)
    {
        int currentCoins = this.currentCoins;
        currentCoins += coins;
        ApiDataHandler.Instance.SetCoinsToFirebase(currentCoins);
    }

    public float ConvertLbsToKg(float pounds)
    {
        return pounds * lbsToKgFactor;
    }
    public float ConvertKgToLbs(float kilograms)
    {
        return kilograms * kgToLbsFactor;
    }
    public void ActiveInput(TMP_InputField input)
    {
        input.Select();
        input.ActivateInputField();
    }
    public string FormatStringAbc(string input)
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;

        string normalizedInput = input.Replace("(", " (").Replace(")", ") ");

        string[] words = normalizedInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = textInfo.ToTitleCase(words[i].ToLower());
        }
        string result = string.Join(" ", words).Trim();
        return result.Replace(" (", " (").Replace(") ", ")");
    }
    public string ShowFormattedNumber(float number)
    {
        // Format the number
        string formattedNumber = (number % 1 == 0) ? number.ToString("0") : number.ToString("0.##");

        return formattedNumber;
    }
}
