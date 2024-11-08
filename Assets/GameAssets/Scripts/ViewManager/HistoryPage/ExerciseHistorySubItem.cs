using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExerciseHistorySubItem : MonoBehaviour, ItemController
{
    public TextMeshProUGUI setText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI repsText;
    public TextMeshProUGUI rirText;
    public TextMeshProUGUI mileText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI rmText;
    ExerciseType exerciseType;
    HistoryExerciseModel exerciseModel;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        exerciseType = (ExerciseType)data["exerciseType"];
        exerciseModel = (HistoryExerciseModel)data["model"];
        setText.text = (string)data["setNumber"];
        switch (exerciseType)
        {
            case ExerciseType.RepsOnly:
                RepsOnly();
                break;
            case ExerciseType.TimeBased:
                TimeBased();
                break;
            case ExerciseType.TimeAndMiles:
                TimeAndMiles();
                break;
            case ExerciseType.WeightAndReps:
                WeightAndReps();
                break;
        }
    }
    void RepsOnly()
    {
        weightText.transform.parent.gameObject.SetActive(false);
        repsText.transform.parent.gameObject.SetActive(true);
        rirText.transform.parent.gameObject.SetActive(false);
        mileText.transform.parent.gameObject.SetActive(false);
        timeText.transform.parent.gameObject.SetActive(false);
        rmText.transform.parent.gameObject.SetActive(true);
        repsText.text=exerciseModel.reps.ToString();
    }
    void TimeBased()
    {
        weightText.transform.parent.gameObject.SetActive(false);
        repsText.transform.parent.gameObject.SetActive(false);
        rirText.transform.parent.gameObject.SetActive(false);
        mileText.transform.parent.gameObject.SetActive(false);
        timeText.transform.parent.gameObject.SetActive(true);
        rmText.transform.parent.gameObject.SetActive(true);
        ShowTime();
    }
    void TimeAndMiles()
    {
        weightText.transform.parent.gameObject.SetActive(false);
        repsText.transform.parent.gameObject.SetActive(false);
        rirText.transform.parent.gameObject.SetActive(false);
        mileText.transform.parent.gameObject.SetActive(true);
        timeText.transform.parent.gameObject.SetActive(true);
        rmText.transform.parent.gameObject.SetActive(true);
        mileText.text=exerciseModel.mile.ToString();
        ShowTime();
    }

    void WeightAndReps()
    {
        weightText.transform.parent.gameObject.SetActive(true);
        repsText.transform.parent.gameObject.SetActive(true);
        rirText.transform.parent.gameObject.SetActive(true);
        mileText.transform.parent.gameObject.SetActive(false);
        timeText.transform.parent.gameObject.SetActive(false);
        rmText.transform.parent.gameObject.SetActive(true);
        rirText.text=exerciseModel.rir.ToString();
        repsText.text=exerciseModel.reps.ToString();
        rmText.text = (exerciseModel.weight * (1 + 0.0333f * exerciseModel.reps)).ToString("F1");
        switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
        {
            case WeightUnit.kg:
                weightText.text=exerciseModel.weight.ToString();
                break;
            case WeightUnit.lbs:
                weightText.text = userSessionManager.Instance.ConvertKgToLbs(exerciseModel.weight).ToString("F2");
                break;
        }
    }
    void ShowTime()
    {
        if (exerciseModel.time > 60)
        {
            int minutes = exerciseModel.time / 60;
            int seconds = exerciseModel.time % 60;
            timeText.text = $"{minutes:D2} : {seconds:D2}";
        }
        else
        {
            timeText.text = $"00 : {exerciseModel.time:D2}"; // Format as 00:secs
        }
    }
}
