using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class historyScreenDataModel : MonoBehaviour, ItemController
{
    public TextMeshProUGUI workoutNameText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;

    public HistoryTempleteModel historyWorkout;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.historyWorkout = (HistoryTempleteModel)data["data"];
        workoutNameText.text = historyWorkout.templeteName.ToUpper();
        int completeTime = historyWorkout.completedTime;
        if(completeTime > 60) 
        { 
            timeText.text=((int)completeTime/60).ToString()+"m";
        }
        else
        {
            timeText.text=completeTime.ToString()+"s";
        }
        DateTime parsedDateTime = DateTime.Parse(historyWorkout.dateTime);
        string formattedDate = parsedDateTime.ToString("MMM dd, yyyy");
        dateText.text = formattedDate;
        //if (historyWorkout.totalWeight > 0) { dateText.text = historyWorkout.totalWeight.ToString() + " kg"; }
        //else { dateText.text = "-"; }
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
        //newSubItem.transform.SetSiblingIndex(childCount - 2);
        HistorySubItem newSubItemScript = newSubItem.GetComponent<HistorySubItem>();

        //HistoryExerciseModel history = null;
        //if (exerciseHistory.Count > 0)
        //{
        //    history = exerciseHistory[0];
        //    exerciseHistory.RemoveAt(0);
        //}
        Dictionary<string, object> initData = new Dictionary<string, object>
        {
            {  "data", exerciseModel   }
        };
        newSubItemScript.onInit(initData,null);
    }
    public void OpenDetails()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
            {
            { "workout",historyWorkout }
            };
        StateManager.Instance.OpenStaticScreen("history", gameObject, "completeWorkoutScreen", mData);
        StateManager.Instance.CloseFooter();
    }
}
