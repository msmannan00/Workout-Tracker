using System.Linq;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Data;

public class ApiDataHandler : GenericSingletonClass<ApiDataHandler>
{
    private ExerciseData exerciseData = new ExerciseData();
    private AchievementData achievementData = new AchievementData();
    private PersonalBestData personalBestData = new PersonalBestData();
    private TemplateData templateData = new TemplateData();
    private HistoryModel historyData = new HistoryModel();
    private MeasurementModel measurementData = new MeasurementModel();

    [Header("Theme Settings")]
    public Theme gameTheme;


    public void SaveTheme(Theme theme)
    {
        PreferenceManager.Instance.SetInt("SelectedTheme", (int)theme);
        PreferenceManager.Instance.Save();
    }
    public Theme LoadTheme()
    {
        int savedTheme = PlayerPrefs.GetInt("SelectedTheme", (int)Theme.Dark);
        return (Theme)savedTheme;
    }

    public void SaveTemplateData()
    {
        string json = JsonUtility.ToJson(templateData);
        print(json);
        PreferenceManager.Instance.SetString("excerciseData", json);
        PreferenceManager.Instance.Save();
    }

    public void LoadTemplateData()
    {
        if (PreferenceManager.Instance.HasKey("excerciseData"))
        {
            string json = PreferenceManager.Instance.GetString("excerciseData");
            templateData = JsonUtility.FromJson<TemplateData>(json);
            print(json);
        }
        else
        {
            templateData = new TemplateData();
            CreateRandomDefaultEntry();
        }
    }

    public void CreateRandomDefaultEntry()
    {
        ExerciseTypeModel back1 = new ExerciseTypeModel
        {
            index = 0,
            name = "Deadlift (Barbell)",
            categoryName = "Glutes",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel back2 = new ExerciseTypeModel
        {
            index = 0,
            name = "Seated narrow grip row (cable)",
            categoryName = "Lats",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel chest = new ExerciseTypeModel
        {
            index = 0,
            name = "Bench Press (Barbell)",
            categoryName = "Chest",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel running = new ExerciseTypeModel
        {
            index = 0,
            name = "Running",
            categoryName = "Cardio",
            exerciseType = ExerciseType.TimeAndMiles,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel jumpRope = new ExerciseTypeModel
        {
            index = 0,
            name = "Jump Rope",
            categoryName = "Cardio",
            exerciseType = ExerciseType.RepsOnly,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel spiderCurls = new ExerciseTypeModel
        {
            index = 0,
            name = "Spider Curls",
            categoryName = "Biceps",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel bicepCulDumbbell = new ExerciseTypeModel
        {
            index = 0,
            name = "Bicep Curl (Dumbbell)",
            categoryName = "Biceps",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel bicepCurlMachine = new ExerciseTypeModel
        {
            index = 0,
            name = "Bicep Curl (Machine)",
            categoryName = "Biceps",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseModel defaultExerciseModel1 = new ExerciseModel
        {
            setID = 1,
            previous = "-",
            weight = 0,
            rir = 0,
            reps = 0
        };
        back1.exerciseModel.Add(defaultExerciseModel1);
        back1.exerciseModel.Add(defaultExerciseModel1);
        back1.exerciseModel.Add(defaultExerciseModel1);
        back2.exerciseModel.Add(defaultExerciseModel1);
        chest.exerciseModel.Add(defaultExerciseModel1);
        running.exerciseModel.Add(defaultExerciseModel1);
        running.exerciseModel.Add(defaultExerciseModel1);
        jumpRope.exerciseModel.Add(defaultExerciseModel1);
        jumpRope.exerciseModel.Add(defaultExerciseModel1);
        jumpRope.exerciseModel.Add(defaultExerciseModel1);
        spiderCurls.exerciseModel.Add(defaultExerciseModel1);
        spiderCurls.exerciseModel.Add(defaultExerciseModel1);
        bicepCulDumbbell.exerciseModel.Add(defaultExerciseModel1);
        bicepCulDumbbell.exerciseModel.Add(defaultExerciseModel1);
        bicepCurlMachine.exerciseModel.Add(defaultExerciseModel1);
        bicepCurlMachine.exerciseModel.Add(defaultExerciseModel1);


        DefaultTempleteModel chestAndBack = new DefaultTempleteModel
        {
            templeteName = "Chest And Back",
            exerciseTemplete = new List<ExerciseTypeModel> { back1, back2, chest }
        };
        DefaultTempleteModel runingAndJumpRope = new DefaultTempleteModel
        {
            templeteName = "Runing And Jump Rope",
            exerciseTemplete = new List<ExerciseTypeModel> { running, jumpRope }
        };
        DefaultTempleteModel bicep = new DefaultTempleteModel
        {
            templeteName = "Biceps",
            exerciseTemplete = new List<ExerciseTypeModel> { bicepCulDumbbell, bicepCulDumbbell, spiderCurls }
        };
        templateData.exerciseTemplete.Clear();
        templateData.exerciseTemplete.Add(chestAndBack);
        templateData.exerciseTemplete.Add(runingAndJumpRope);
        templateData.exerciseTemplete.Add(bicep);

        SaveTemplateData();
    }

    public void SaveHistory()
    {
        string json = JsonUtility.ToJson(historyData);
        PreferenceManager.Instance.SetString("historyData", json);
        PreferenceManager.Instance.Save();
    }

    public void SaveMeasurementData()
    {
        string json = JsonUtility.ToJson(measurementData);
        PreferenceManager.Instance.SetString("measurementData", json);
        PreferenceManager.Instance.Save();
    }

    public void LoadHistory()
    {
        if (PreferenceManager.Instance.HasKey("historyData"))
        {
            string json = PreferenceManager.Instance.GetString("historyData");
            historyData = JsonUtility.FromJson<HistoryModel>(json);
        }
        else
        {
            historyData = new HistoryModel();
        }
    }

    public void LoadMeasurementData()
    {
        if (PreferenceManager.Instance.HasKey("measurementData"))
        {
            string json = PreferenceManager.Instance.GetString("measurementData");
            measurementData = JsonUtility.FromJson<MeasurementModel>(json);
        }
        else
        {
            measurementData = new MeasurementModel();
        }
    }

    public MeasurementModel getMeasurementData()
    {
        return measurementData;
    }





    public void loadData()
    {
        TextAsset exerciseJsonFile = Resources.Load<TextAsset>("data/exercise");
        this.exerciseData = JsonUtility.FromJson<ExerciseData>(exerciseJsonFile.text);

        TextAsset achievementJsonFile = Resources.Load<TextAsset>("data/achievement");
        this.achievementData = JsonUtility.FromJson<AchievementData>(achievementJsonFile.text);

        TextAsset personBestJsonFile = Resources.Load<TextAsset>("data/personalBest");
        this.personalBestData = JsonUtility.FromJson<PersonalBestData>(personBestJsonFile.text);

        LoadHistory();

        LoadTemplateData();

        LoadMeasurementData();

        gameTheme = LoadTheme();
    }

    public ExerciseData getExerciseData()
    {
        return this.exerciseData;
    }
    public AchievementData getAchievementData()
    {
        return this.achievementData;
    }
    public PersonalBestData getPersonalBestData()
    {
        return this.personalBestData;
    }
    public HistoryModel getHistoryData()
    {
        return this.historyData;
    }
    public TemplateData getTemplateData()
    {
        return this.templateData;
    }
    public void SaveExerciseData(ExerciseDataItem exercise)
    {
        this.exerciseData.exercises.Add(exercise);
        string json = JsonUtility.ToJson(exerciseData, true);
        string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/exercise.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
        File.WriteAllText(filePath, json);
    }
    public void SaveAchievementData()
    {
        string json = JsonUtility.ToJson(achievementData, true);
        string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/achievement.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
        File.WriteAllText(filePath, json);
    }
    public void SavePersonalBestData()
    {
        string json = JsonUtility.ToJson(personalBestData, true);
        string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/personalBest.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
        File.WriteAllText(filePath, json);
    }
    public void SetJoiningDate(DateTime date)
    {
        string dateInString= date.ToString("MMM dd, yyyy");
        PreferenceManager.Instance.SetString("JoiningDate", dateInString);
    }
    public void SetWeeklyGoal(int goal)
    {
        PreferenceManager.Instance.SetInt("WeeklyGoal", goal);
    }
    //public void SetWeeklyGoalSetedDate(DateTime date)
    //{
    //    string dateInString = date.ToString("MMM dd, yyyy");
    //    PreferenceManager.Instance.SetString("WeeklyGoalSetedDate", dateInString);
    //}
    public void AddItemToHistoryData(HistoryTempleteModel item)
    {
        historyData.exerciseTempleteModel.Add(item);
    }
    public void AddItemToTemplateData(DefaultTempleteModel item)
    {
        templateData.exerciseTemplete.Add(item);
    }
    public void InsertItemToTemplateData(int index, DefaultTempleteModel item)
    {
        templateData.exerciseTemplete.Insert(index,item);
    }
    public void RemoveItemFromTempleteData(int index)
    {
        templateData.exerciseTemplete.RemoveAt(index);
    }
    public string GetJoiningDate()
    {
        DateTime parsedDateTime = DateTime.Parse(PreferenceManager.Instance.GetString("JoiningDate",DateTime.Now.ToString("MMM dd, yyyy")));
        string formattedDate = parsedDateTime.ToString("MMM dd, yyyy");
        return formattedDate;
    }
    public int GetWeeklyGoal()
    {
        return PreferenceManager.Instance.GetInt("WeeklyGoal");
    }
    //public DateTime GetWeeklyGoalSetedDate()
    //{
    //    DateTime parsedDateTime = DateTime.Parse(PreferenceManager.Instance.GetString("JoiningDate"));
    //    return parsedDateTime;
    //}
    public int GetCompletedAchievements()
    {
        int completedCount = 0;
        foreach (AchievementTemplate achievement in achievementData.achievements)
        {
            bool isAchievementCompleted = achievement.achievementData.All(item => item.isCompleted);
            if (isAchievementCompleted)
            {
                completedCount++;
            }
        }

        return completedCount;
    }
    public int GetTotalAchievements()
    {
        return achievementData.achievements.Count;
    }
    public int GetTotalTrophys()
    {
        int count = 0;
        foreach (AchievementTemplate achievement in achievementData.achievements)
        {
            count = count + achievement.achievementData.Count;
        }

        return count;
    }
    public int GetCompletedTrophys()
    {
        int count = 0;
        foreach (AchievementTemplate achievement in achievementData.achievements)
        {
            foreach (AchievementTemplateDataItem item in achievement.achievementData)
            {
                if (item.isCompleted)
                {
                    count++;
                }
            }
        }

        return count;
    }



    public void SetCurrentWeekStartDate(DateTime startDate)
    {
        string startDateString = startDate.ToString("yyyy-MM-dd");
        PlayerPrefs.SetString("CurrentWeekStartDate", startDateString);
        PlayerPrefs.Save();
    }

    // Method to get the start date of the current week
    public DateTime GetCurrentWeekStartDate()
    {
        string startDateString = PlayerPrefs.GetString("CurrentWeekStartDate", "");

        if (!string.IsNullOrEmpty(startDateString))
        {
            return DateTime.Parse(startDateString);
        }

        // If no start date is set, return the start of this week as a default
        DateTime today = DateTime.Now;
        int daysSinceMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
        if (daysSinceMonday < 0) daysSinceMonday += 7;
        return today.AddDays(-daysSinceMonday);
    }

    // Method to check if the week has changed and update the start date automatically
    public void CheckAndUpdateWeekStartDate()
    {
        DateTime storedWeekStartDate = GetCurrentWeekStartDate();
        DateTime currentWeekStartDate = GetStartOfCurrentWeek();

        // If the stored week start date is not the same as the current week start date, update it
        if (storedWeekStartDate != currentWeekStartDate)
        {
            SetCurrentWeekStartDate(currentWeekStartDate);
        }
    }

    // Helper method to get the start date of the current week
    public DateTime GetStartOfCurrentWeek()
    {
        DateTime today = DateTime.Now;
        int daysSinceMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
        if (daysSinceMonday < 0) daysSinceMonday += 7;
        return today.AddDays(-daysSinceMonday);
    }
    public int GetUserStreak()
    {
        return PreferenceManager.Instance.GetInt("UserStreak", 0);
    }

    // Sets the user's streak
    public void SetUserStreak(int streak)
    {
        PreferenceManager.Instance.SetInt("UserStreak", streak);
    }
}
