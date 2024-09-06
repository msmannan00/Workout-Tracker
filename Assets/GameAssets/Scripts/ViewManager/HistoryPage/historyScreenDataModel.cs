using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class historyScreenDataModel : MonoBehaviour, ItemController
{
    public TextMeshProUGUI workoutNameText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI WeightText;

    public HistoryTempleteModel historyWorkout;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.historyWorkout = (HistoryTempleteModel)data["data"];
        workoutNameText.text = historyWorkout.templeteName;
        int completeTime = historyWorkout.completedTime;
        if(completeTime > 60) 
        { 
            timeText.text=((int)completeTime/60).ToString()+"m";
        }
        else
        {
            timeText.text=completeTime.ToString()+"s";
        }
        if (historyWorkout.exerciseTypeModel.Count > 0)
        {
            foreach (var exerciseModel in historyWorkout.exerciseTypeModel)
            {
                AddSetFromModel(exerciseModel);
            }
        }
    }
    private void AddSetFromModel(HistoryExerciseTypeModel exerciseModel)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/history/historySubItem");

        GameObject newSubItem = Instantiate(prefab, transform);
        int childCount = transform.childCount;
        newSubItem.transform.SetSiblingIndex(childCount - 2);
        HistorySubItem newSubItemScript = newSubItem.GetComponent<HistorySubItem>();

        HistoryExerciseModel history = null;
        //if (exerciseHistory.Count > 0)
        //{
        //    history = exerciseHistory[0];
        //    exerciseHistory.RemoveAt(0);
        //}
        //Dictionary<string, object> initData = new Dictionary<string, object>
        //{
        //    {  "data", exerciseModel   },
        //    {"isWeight", exerciseTypeModel.isWeigtExercise  },
        //    {"exerciseHistory",history}
        //};
        //newSubItemScript.onInit(initData);
    }
}
