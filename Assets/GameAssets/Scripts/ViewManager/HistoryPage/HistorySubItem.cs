using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistorySubItem : MonoBehaviour,ItemController
{
    public TextMeshProUGUI exerciseNameText;
    public Image line;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        HistoryExerciseTypeModel history = (HistoryExerciseTypeModel)data["data"];
        exerciseNameText.text = history.exerciseName + " x " + history.exerciseModel.Count.ToString();
        //bestSetText.text=GetBestSet(history.exerciseModel,history.isWeightExercise);
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                exerciseNameText.color = new Color32(92, 59, 28, 155);
                line.color = new Color32(92, 59, 28, 155);
                break;
            case Theme.Dark:
                exerciseNameText.color = new Color32(217, 217, 217, 127);
                line.color = new Color32(217, 217, 217, 127);
                break;
        }
    }
    public string GetBestSet(List<HistoryExerciseModel> exerciseModel, bool isWeight)
    {
        if (isWeight)
        {
            var maxExercise = exerciseModel.OrderByDescending(exercise => exercise.weight * exercise.reps).FirstOrDefault();
            if (maxExercise != null)
            {
                return $"{maxExercise.weight}kg x {maxExercise.reps}";
            }
        }
        else
        {
            // If the bool is false, find the max time value and return it as a formatted string
            int maxTime = exerciseModel.Max(exercise => exercise.time);
            int minutes = maxTime / 60;
            int seconds = maxTime % 60;
            return $"{minutes:00}:{seconds:00}";
        }
        return null;
    }
}
