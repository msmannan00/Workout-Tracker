using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class workoutLogScreenDataModel : MonoBehaviour, ItemController
{
    public TextMeshProUGUI exerciseNameText;
    public GameObject timer, weight, lbs, reps;
    ExerciseTypeModel exerciseTypeModel;
    Action<object> callback;
    bool isWorkoutLog;
    List<HistoryExerciseModel> exerciseHistory;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        this.exerciseTypeModel = (ExerciseTypeModel)data["data"];
        isWorkoutLog = (bool)data["isWorkoutLog"];
        exerciseNameText.text = exerciseTypeModel.name;
        exerciseHistory=GetExerciseData(userSessionManager.Instance.historyData, exerciseTypeModel.name, exerciseTypeModel.isWeigtExercise);
        if (exerciseTypeModel.isWeigtExercise)
        {
            timer.gameObject.SetActive(false);
            weight.gameObject.SetActive(true);
            lbs.gameObject.SetActive(true);
            reps.gameObject.SetActive(true);
        }
        else
        {
            timer.gameObject.SetActive(true);
            weight.gameObject.SetActive(false);
            lbs.gameObject.SetActive(false);
            reps.gameObject.SetActive(false);
        }
        if (exerciseTypeModel.exerciseModel.Count > 0)
        {
            foreach (var exerciseModel in exerciseTypeModel.exerciseModel)
            {
                AddSetFromModel(exerciseModel);
            }
        }
        else
        {
            OnAddSet();
        }
    }

    private void AddSetFromModel(ExerciseModel exerciseModel)
    {
        GameObject prefab;
        if(isWorkoutLog)
            prefab = Resources.Load<GameObject>("Prefabs/workoutLog/workoutLogSubItems");
        else
            prefab = Resources.Load<GameObject>("Prefabs/createWorkout/createNewSubItems");
        GameObject newSubItem = Instantiate(prefab, transform);
        int childCount = transform.childCount;
        newSubItem.transform.SetSiblingIndex(childCount - 2);
        WorkoutLogSubItem newSubItemScript = newSubItem.GetComponent<WorkoutLogSubItem>();

        HistoryExerciseModel history = null;
        if(exerciseHistory.Count > 0)
        {
            history = exerciseHistory[0];
            exerciseHistory.RemoveAt(0);
        }
        Dictionary<string, object> initData = new Dictionary<string, object>
        {
            {  "data", exerciseModel   },
            {"isWeight", exerciseTypeModel.isWeigtExercise  },
            {"exerciseHistory",history}
        };
        newSubItemScript.onInit(initData);
    }

    public void OnAddSet()
    {
        ExerciseModel exerciseModel = new ExerciseModel();
        exerciseTypeModel.exerciseModel.Add(exerciseModel);
        AddSetFromModel(exerciseModel);
    }

    public void onRemoveSet()
    {
        this.callback.Invoke(this.exerciseTypeModel.index);
        GameObject.Destroy(gameObject);
    }

    public void SaveExercisePreferences(string exerciseName, HistoryExerciseModel exercisedata)
    {
        string json = JsonUtility.ToJson(exercisedata);
        PlayerPrefs.SetString((exerciseName), json);
        PlayerPrefs.Save();
    }
    public HistoryExerciseModel GetResentPerformed(string exerciseName)
    {
        if (PreferenceManager.Instance.HasKey(exerciseName))
        {
            string json = PreferenceManager.Instance.GetString(exerciseName);
            return JsonUtility.FromJson<HistoryExerciseModel>(json);
        }
        return null;
    }
    public List<HistoryExerciseModel> GetExerciseData(HistoryModel historyData, string exerciseName, bool isWeight)
    {
        List<HistoryExerciseModel> exerciseDataList = new List<HistoryExerciseModel>();

        foreach (var template in historyData.exerciseTempleteModel)
        {
            foreach (var exerciseType in template.exerciseTypeModel)
            {
                if (exerciseType.exerciseName.Equals(exerciseName, StringComparison.OrdinalIgnoreCase))
                {
                    exerciseDataList.AddRange(exerciseType.exerciseModel);
                }
            }
        }
        if (isWeight)
        {
            exerciseDataList = exerciseDataList
                .OrderByDescending(e => e.weight * e.reps)
                .ToList();
        }
        else
        {
            exerciseDataList = exerciseDataList
                .OrderBy(e => e.time)
                .ToList();
        }
        return exerciseDataList;
    }
}
