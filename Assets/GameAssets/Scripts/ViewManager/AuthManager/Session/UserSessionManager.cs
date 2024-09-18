using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class userSessionManager : GenericSingletonClass<userSessionManager>
{
    public string mProfileUsername;
    public string mProfileID;
    public bool mSidebar = false;
    [Header("Theme Settings")]
    public Theme gameTheme;
    public TMP_FontAsset darkHeadingFont, darkTextFont;
    public TMP_FontAsset lightHeadingFont, lightTextFont;
    public Color darkBgColor, darkSearchBarColor, darkSearchIconColor;
    public Color lightBgColor, lightHeadingColor, lightButtonColor, lightTextColor;

    public DefaultTempleteModel selectedTemplete;
    public ExcerciseData excerciseData = new ExcerciseData();
    public HistoryModel historyData = new HistoryModel();

    private void Start()
    {
        darkHeadingFont= Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Hoog0555/Hoog0555");
        darkTextFont = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/K2D/K2D");
        lightHeadingFont = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Alexandria/Alexandria");
        lightTextFont = Resources.Load<TMP_FontAsset>("UIAssets/Shared/Font/Afacad/Afacad");
        darkBgColor = new Color32(99, 24, 24, 255);
        darkSearchBarColor = new Color32(81, 14, 14, 255);
        darkSearchIconColor = new Color32(127, 77, 77, 255);
        lightBgColor = new Color32(250, 249, 240, 255);
        lightHeadingColor = new Color32(150, 0, 0, 255);
        lightButtonColor = new Color32(218, 52, 52, 255);
        lightTextColor = new Color32(92, 59, 28, 255);
    }

    public void OnInitialize(string pProfileUsername, string pProfileID)
    {
        this.mProfileUsername = pProfileUsername;
        this.mProfileID = pProfileID;
        PreferenceManager.Instance.SetString("login_username", pProfileUsername);
        mSidebar = false;

        LoadHistory();
        gameTheme = LoadTheme();

    }
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
    public void OnResetSession()
    {
        this.mProfileUsername = null;
        this.mProfileID = null;
    }

    public void SaveExcerciseData()
    {
        string json = JsonUtility.ToJson(excerciseData);
        print(json);
        PreferenceManager.Instance.SetString("excerciseData", json);
        PreferenceManager.Instance.Save();
    }

    public void LoadExcerciseData()
    {
        if (PreferenceManager.Instance.HasKey("excerciseData"))
        {
            string json = PreferenceManager.Instance.GetString("excerciseData");
            excerciseData = JsonUtility.FromJson<ExcerciseData>(json);
            print(json);
        }
        else
        {
            excerciseData = new ExcerciseData();
            CreateRandomDefaultEntry();
        }
    }

    public void CreateRandomDefaultEntry()
    {
        ExerciseTypeModel back1 = new ExerciseTypeModel
        {
            index = 0,
            name = "Deadlift (Barbell)",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel back2 = new ExerciseTypeModel
        {
            index = 0,
            name = "Seated Row (Cable)",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        }; 
        ExerciseTypeModel chest = new ExerciseTypeModel
        {
            index = 0,
            name = "Bench Press (Barbell)",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel running = new ExerciseTypeModel
        {
            index = 0,
            name = "Running",
            exerciseType = ExerciseType.TimeAndMiles,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel jumpRope = new ExerciseTypeModel
        {
            index = 0,
            name = "Jump Rope",
            exerciseType = ExerciseType.RepsOnly,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel spiderCurls = new ExerciseTypeModel
        {
            index = 0,
            name = "Spider Curls",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel bicepCulDumbbell = new ExerciseTypeModel
        {
            index = 0,
            name = "Bicep Curl (Dumbbell)",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseTypeModel bicepCurlMachine = new ExerciseTypeModel
        {
            index = 0,
            name = "Bicep Curl (Machine)",
            exerciseType = ExerciseType.WeightAndReps,
            exerciseModel = new List<ExerciseModel>()
        };
        ExerciseModel defaultExerciseModel1 = new ExerciseModel
        {
            setID = 1,
            previous = "-",
            weight = 0,
            lbs = 0,
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
        excerciseData.exerciseTemplete.Clear();
        excerciseData.exerciseTemplete.Add(chestAndBack);
        excerciseData.exerciseTemplete.Add(runingAndJumpRope);
        excerciseData.exerciseTemplete.Add(bicep);
        
        SaveExcerciseData();
    }

    public void SaveHistory()
    {
        string json = JsonUtility.ToJson(historyData);
        //print(json);
        PreferenceManager.Instance.SetString("historyData", json);
        PreferenceManager.Instance.Save();
    }
    public void LoadHistory()
    {
        if (PreferenceManager.Instance.HasKey("historyData"))
        {
            string json = PreferenceManager.Instance.GetString("historyData");
            historyData = JsonUtility.FromJson<HistoryModel>(json);
            
            print(json);
        }
        else
        {
            historyData = new HistoryModel();
        }
    }
}
