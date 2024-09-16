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
    List<HistoryExerciseModel> exerciseHistory;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        this.exerciseTypeModel = (ExerciseTypeModel)data["data"];
        isWorkoutLog = (bool)data["isWorkoutLog"];
        exerciseNameText.text = exerciseTypeModel.name.ToUpper();
        exerciseHistory=GetExerciseData(userSessionManager.Instance.historyData, exerciseTypeModel.name, exerciseTypeModel.exerciseType);
        switch (exerciseTypeModel.exerciseType)
        {
            case ExerciseType.RepsOnly:
                reps.gameObject.SetActive(true);
                print("Need to implement REPS only");
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
                print("Need to implement Time and Miles");
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
            OnAddSet();
        }
    }
    private void OnEnable()
    {
        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Dark:
                exerciseNameText.font = userSessionManager.Instance.darkHeadingFont;
                exerciseNameText.color = Color.white;
                exerciseNotes.gameObject.GetComponent<Image>().color = userSessionManager.Instance.darkBgColor;
                TextMeshProUGUI placeholde= exerciseNotes.placeholder as TextMeshProUGUI;
                TextMeshProUGUI text = exerciseNotes.textComponent as TextMeshProUGUI;
                placeholde.color= new Color32(255,255,255,150);
                placeholde.font = userSessionManager.Instance.darkTextFont;
                text.font = userSessionManager.Instance.darkTextFont;
                text.color = Color.white;
                foreach(TextMeshProUGUI _text in labelText)
                {
                    _text.font=userSessionManager.Instance.darkHeadingFont;
                    _text.color= Color.white;
                }
                line.color = Color.white;
                addSet.color = Color.white;
                addSet.transform.GetComponentInChildren<TextMeshProUGUI>().font=userSessionManager.Instance.darkHeadingFont;
                addSet.transform.GetComponentInChildren<TextMeshProUGUI>().color=userSessionManager.Instance.darkBgColor;
                break;
            case Theme.Light:
                exerciseNameText.font = userSessionManager.Instance.lightHeadingFont;
                exerciseNameText.color = userSessionManager.Instance.lightHeadingColor;
                exerciseNotes.gameObject.GetComponent<Image>().color = new Color32(246, 236, 220, 255);
                TextMeshProUGUI _placeholde_ = exerciseNotes.placeholder as TextMeshProUGUI;
                TextMeshProUGUI _text_ = exerciseNotes.textComponent as TextMeshProUGUI;
                _placeholde_.color = new Color32(92, 59, 28, 150);
                _placeholde_.font = userSessionManager.Instance.lightTextFont;
                _text_.font = userSessionManager.Instance.lightTextFont;
                _text_.color = userSessionManager.Instance.lightTextColor;
                foreach (TextMeshProUGUI _text in labelText)
                {
                    _text.font = userSessionManager.Instance.lightHeadingFont;
                    _text.color = userSessionManager.Instance.darkBgColor;
                }
                line.color = new Color32(218,52,52,150);
                addSet.color = userSessionManager.Instance.lightButtonColor;
                addSet.transform.GetComponentInChildren<TextMeshProUGUI>().font = userSessionManager.Instance.lightHeadingFont;
                addSet.transform.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                break;
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
