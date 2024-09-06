using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoryController : MonoBehaviour, PageController
{
    public Transform content;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {

    }
    private void Start()
    {
        List<HistoryTempleteModel> list = new List<HistoryTempleteModel>();
        foreach (var workouts in userSessionManager.Instance.historyData.exerciseTempleteModel)
        {
            list.Add(workouts);
        }
    }

    public void OnExerciseAdd(object data)
    {
        //List<object> dataList = data as List<object>;
        if (data == null)
        {
            print("data null");
        }

        if (data is List<HistoryTempleteModel> dataList)
        {
            foreach (object item in dataList)
            {
                HistoryTempleteModel typeModel =null;

                if (item is HistoryTempleteModel dataItem)
                {
                    typeModel = new HistoryTempleteModel
                    {
                        templeteName = dataItem.templeteName,
                        completedTime = dataItem.completedTime,
                        totalWeight= dataItem.totalWeight,
                        exerciseTypeModel=dataItem.exerciseTypeModel
                        //index = exerciseCounter++
                    };
                }

                Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { "data", typeModel } //, { "isWorkoutLog", true }
                };

                GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/history/historyScreenDataModel");
                GameObject exerciseObject = Instantiate(exercisePrefab, content);
                int childCount = content.childCount;
                exerciseObject.transform.SetSiblingIndex(childCount - 1);
                exerciseObject.GetComponent<historyScreenDataModel>().onInit(mData, null);
            }
        }
    }
}
