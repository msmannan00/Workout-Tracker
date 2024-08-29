using UnityEngine;
using System.Collections.Generic;

public class userSessionManager : GenericSingletonClass<userSessionManager>
{
    public string mProfileUsername;
    public string mProfileID;
    public bool mSidebar = false;
    public ExcerciseData excerciseData = new ExcerciseData();
    public HistoryModel historyData = new HistoryModel();

    public void OnInitialize(string pProfileUsername, string pProfileID)
    {
        this.mProfileUsername = pProfileUsername;
        this.mProfileID = pProfileID;
        PreferenceManager.Instance.SetString("login_username", pProfileUsername);
        mSidebar = false;
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
        ExerciseTypeModel defaultExerciseType = new ExerciseTypeModel
        {
            index = 0,
            name = "Default Exercise",
            exerciseModel = new List<ExerciseModel>()
        };

        ExerciseModel defaultExerciseModel = new ExerciseModel
        {
            setID = 1,
            previous = "-",
            weight = 20,
            lbs = 45,
            reps = 6
        };

        defaultExerciseType.exerciseModel.Add(defaultExerciseModel);

        DefaultTempleteModel defaultTemplate = new DefaultTempleteModel
        {
            templeteName = "Default Workout",
            exerciseTemplete = new List<ExerciseTypeModel> { defaultExerciseType }
        };

        excerciseData.exerciseTemplete.Clear();
        excerciseData.exerciseTemplete.Add(defaultTemplate);

        SaveExcerciseData();
    }
}
