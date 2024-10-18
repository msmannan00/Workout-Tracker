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
    public TMP_InputField exerciseNotes;
    public List<TextMeshProUGUI> labelText = new List<TextMeshProUGUI>();
    public Image addSet, line;

    public GameObject timer, mile, weight, reps, rir;
    public ExerciseTypeModel exerciseTypeModel;
    Action<object> callback;
    bool isWorkoutLog;
    public bool isTemplateCreator;
    List<HistoryExerciseModel> exerciseHistory;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        this.exerciseTypeModel = (ExerciseTypeModel)data["data"];
        isWorkoutLog = (bool)data["isWorkoutLog"];
        isTemplateCreator = (bool)data["isTemplateCreator"];
        exerciseNameText.text = exerciseTypeModel.name.ToUpper();
        exerciseHistory = GetExerciseData(ApiDataHandler.Instance.getHistoryData(), exerciseTypeModel.name, exerciseTypeModel.exerciseType);
        switch (exerciseTypeModel.exerciseType)
        {
            case ExerciseType.RepsOnly:
                reps.gameObject.SetActive(true);
                break;
            case ExerciseType.TimeBased:
                timer.gameObject.SetActive(true);
                weight.gameObject.SetActive(false);
                rir.gameObject.SetActive(false);
                reps.gameObject.SetActive(false);
                break;
            case ExerciseType.TimeAndMiles:
                timer.gameObject.SetActive(true);
                mile.gameObject.SetActive(true);
                break;
            case ExerciseType.WeightAndReps:
                timer.gameObject.SetActive(false);
                weight.gameObject.SetActive(true);
                rir.gameObject.SetActive(true);
                reps.gameObject.SetActive(true);
                break;
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
            OnAddSet(false);
        }
    }
    private void AddSetFromModel(ExerciseModel exerciseModel)
    {
        GameObject prefab;
        if(isWorkoutLog)
            prefab = Resources.Load<GameObject>("Prefabs/workoutLog/workoutLogSubItems");
        else
        {
            prefab = Resources.Load<GameObject>("Prefabs/workoutLog/workoutLogSubItems");
            //prefab = Resources.Load<GameObject>("Prefabs/createWorkout/createNewSubItems");
        }
        GameObject newSubItem = Instantiate(prefab, transform);
        int childCount = transform.childCount;
        newSubItem.transform.SetSiblingIndex(childCount - 3);
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
            {"exerciseType", exerciseTypeModel.exerciseType  },
            {"exerciseHistory",history}
        };
        newSubItemScript.onInit(initData,callback);
    }

    public void OnAddSet(bool addMore)
    {
        if (isWorkoutLog && !isTemplateCreator)
        {
            FindAnyObjectByType<WorkoutLogController>().addSets = addMore;
        }
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
    public List<HistoryExerciseModel> GetExerciseData(HistoryModel historyData, string exerciseName, ExerciseType type)
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
        switch (type)
        {
            case ExerciseType.RepsOnly:
                exerciseDataList = exerciseDataList
                .OrderBy(e => e.reps)
                .ToList();
                break;
            case ExerciseType.TimeBased:
                exerciseDataList = exerciseDataList
                .OrderBy(e => e.time)
                .ToList();
                break;
            case ExerciseType.TimeAndMiles:
                break;
            case ExerciseType.WeightAndReps:
                exerciseDataList = exerciseDataList
                .OrderByDescending(e => e.weight * e.reps)
                .ToList();
                break;
        }
        return exerciseDataList;
    }
}
