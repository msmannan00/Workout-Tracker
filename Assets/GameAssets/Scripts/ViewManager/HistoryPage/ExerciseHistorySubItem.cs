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
    public TextMeshProUGUI rpeText;
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
        repsText.transform.parent.gameObject.SetActive(true);
        rirText.transform.parent.gameObject.SetActive(true);
        repsText.text = userSessionManager.Instance.ShowFormattedNumber(exerciseModel.reps);//.ToString("F1");
        rirText.text=exerciseModel.rir.ToString();
    }
    void TimeBased()
    {
        rpeText.transform.parent.gameObject.SetActive(true);
        timeText.transform.parent.gameObject.SetActive(true);
        rpeText.text= exerciseModel.rpe.ToString();
        ShowTime();
    }
    void TimeAndMiles()
    {
        mileText.transform.parent.gameObject.SetActive(true);
        timeText.transform.parent.gameObject.SetActive(true);
        rpeText.transform.parent.gameObject.SetActive(true);
        rpeText.text = exerciseModel.rpe.ToString();
        mileText.text=exerciseModel.mile.ToString();
        ShowTime();
    }

    void WeightAndReps()
    {
        weightText.transform.parent.gameObject.SetActive(true);
        repsText.transform.parent.gameObject.SetActive(true);
        rirText.transform.parent.gameObject.SetActive(true);
        rmText.transform.parent.gameObject.SetActive(true);
        rirText.text=exerciseModel.rir.ToString();
        repsText.text=userSessionManager.Instance.ShowFormattedNumber(exerciseModel.reps);
        // calculating 1Rm
        if ((int)exerciseModel.reps == 1)
        {
            switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
            {
                case WeightUnit.kg:
                    rmText.text = exerciseModel.weight % 1 == 0
                ? exerciseModel.weight.ToString("F0")
                : exerciseModel.weight.ToString("F1");
                    break;
                case WeightUnit.lbs:
                    rmText.text = exerciseModel.weight % 1 == 0
                ? Mathf.Round(userSessionManager.Instance.ConvertKgToLbs(exerciseModel.weight)).ToString("F0")
                : Mathf.Round(userSessionManager.Instance.ConvertKgToLbs(exerciseModel.weight)).ToString("F1");
                    break;
            }
            //rmText.text = exerciseModel.weight % 1 == 0
            //    ? exerciseModel.weight.ToString("F0")
            //    : exerciseModel.weight.ToString("F1");
        }
        else
        {
            float result = exerciseModel.weight * (1 + 0.0333f * (int)exerciseModel.reps);
            float roundedResult = Mathf.Round(result * 2) / 2; // Rounds to the nearest 0.5
            switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
            {
                case WeightUnit.kg:
                    rmText.text = roundedResult % 1 == 0
                ? roundedResult.ToString("F0")
                : roundedResult.ToString("F1");
                    break;
                case WeightUnit.lbs:
                    rmText.text = exerciseModel.weight % 1 == 0
                ? Mathf.Round(userSessionManager.Instance.ConvertKgToLbs(roundedResult)).ToString("F0")
                : Mathf.Round(userSessionManager.Instance.ConvertKgToLbs(roundedResult)).ToString("F1");
                    break;
            }
            //rmText.text = roundedResult % 1 == 0
            //    ? roundedResult.ToString("F0")
            //    : roundedResult.ToString("F1");
        }

        //rmText.text = Mathf.RoundToInt((exerciseModel.weight * (1 + 0.0333f * exerciseModel.reps))).ToString();
        switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
        {
            case WeightUnit.kg:
                weightText.text=exerciseModel.weight.ToString();
                break;
            case WeightUnit.lbs:
                weightText.text = Mathf.Round(userSessionManager.Instance.ConvertKgToLbs(exerciseModel.weight)).ToString("F1");
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
