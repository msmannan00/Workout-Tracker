
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExerciseHistoryDataModel : MonoBehaviour, ItemController
{
    public TextMeshProUGUI dateText;
    public GameObject weight;
    public GameObject reps;
    public GameObject rir;
    public GameObject rpe;
    public GameObject mile;
    public GameObject time;
    public GameObject rm;
    public RectTransform heaviestSet;
    public RectTransform bestSet;
    public Transform content;
    ExerciseWithDate exerciseInHistory;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        exerciseInHistory = (ExerciseWithDate)data["history"];
        string dateTime = exerciseInHistory.dateTime;
        DateTime parsedDateTime = DateTime.Parse(dateTime);
        string formattedDate = parsedDateTime.ToString("MMM dd, yyyy");
        dateText.text = formattedDate;

        switch (exerciseInHistory.exerciseType)
        {
            case ExerciseType.RepsOnly:
                reps.gameObject.SetActive(true);
                rir.SetActive(true);
                break;
            case ExerciseType.TimeBased:
                time.SetActive(true);
                rpe.SetActive(true);
                break;
            case ExerciseType.TimeAndMiles:
                mile.SetActive(true);
                time.SetActive(true);
                rpe.SetActive(true);
                break;
            case ExerciseType.WeightAndReps:
                weight.gameObject.SetActive(true);
                reps.gameObject.SetActive(true);
                rir.SetActive(true);
                rm.SetActive(true);
                break;
        }

        AddSubItems(exerciseInHistory);
        CheckPerformance(exerciseInHistory);
    }
    public void AddSubItems(ExerciseWithDate history)
    {
        for(int i = 0;i<history.exercise.Count;i++)
        {
            Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { "model", history.exercise[i] }, { "exerciseType", history.exerciseType },{ "setNumber", (i + 1).ToString() },
                };
            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/history/exerciseHistorySubItem");
            GameObject exerciseObject = Instantiate(exercisePrefab, content);
            exerciseObject.GetComponent<ItemController>().onInit(mData, null);
            if (i == history.exercise.Count - 1)
            {
                RectTransform rectTransform = exerciseObject.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 75);
            }
        }
    }
    public void CheckPerformance(ExerciseWithDate history)
    {
        bool heaviest = false;
        if ((history.performance & HistoryPerformance.HeaviestLifted) != 0)
        {
            heaviest = true;
            heaviestSet.gameObject.SetActive(true);
        }
        if ((history.performance & HistoryPerformance.BestSet) != 0)
        {
            if(heaviest)
                bestSet.gameObject.SetActive(true);
            else
            {
                Vector2 pos = heaviestSet.position;
                bestSet.position = pos;
                bestSet.gameObject.SetActive(true);
            }

        }
    }
}
