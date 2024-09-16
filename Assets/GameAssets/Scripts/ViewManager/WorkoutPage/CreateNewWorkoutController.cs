using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewWorkoutController : MonoBehaviour,PageController
{
    public TMP_InputField workoutName;
    public Transform content;
    public Button addExercise;

    private int exerciseCounter = 0;
    public List<ExerciseDataItem> exerciseDataItems = new List<ExerciseDataItem>();
    public DefaultTempleteModel templeteModel = new DefaultTempleteModel();

    Action<object> callback;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        workoutName.text = (string)data["workoutName"];
        this.callback = callback;
        addExercise.onClick.AddListener(() => AddExerciseButton());
    }
    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }
    public void AddExerciseButton()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "isWorkoutLog", false }
        };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true, OnExerciseAdd);
    }
    public void OnExerciseAdd(object data)
    {
        if (data is List<ExerciseDataItem> dataList)
        {
            foreach (object item in dataList)
            {
                ExerciseTypeModel typeModel;

                if (item is ExerciseDataItem dataItem)
                {
                    typeModel = new ExerciseTypeModel
                    {
                        name = dataItem.exerciseName,
                        exerciseModel = new List<ExerciseModel>(),
                        index = exerciseCounter++,
                        exerciseType=dataItem.exerciseType
                    };

                    templeteModel.exerciseTemplete.Add(typeModel);

                    exerciseDataItems.Add(dataItem);
                }
                else
                {
                    typeModel = (ExerciseTypeModel)item;
                    templeteModel.exerciseTemplete.Add(typeModel);
                }

                Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { "data", typeModel },
                    { "isWorkoutLog", false }
                };

                GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/workoutLog/workoutLogScreenDataModel");
                //GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/createWorkout/createNewWorkoutDataModel");
                GameObject exerciseObject = Instantiate(exercisePrefab, content);
                exerciseObject.transform.SetSiblingIndex(content.childCount-2);
                exerciseObject.GetComponent<workoutLogScreenDataModel>().onInit(mData, OnRemoveIndex);
            }
        }
    }
    private void OnRemoveIndex(object data)
    {
        int index = (int)data;

        for (int i = 0; i < templeteModel.exerciseTemplete.Count; i++)
        {
            if (templeteModel.exerciseTemplete[i].index == index)
            {
                templeteModel.exerciseTemplete.RemoveAt(i);
                break;
            }
        }
    }
    public void SaveNewWorkout()
    {
        templeteModel.templeteName = workoutName.text;
        userSessionManager.Instance.excerciseData.exerciseTemplete.Add(templeteModel);
        userSessionManager.Instance.SaveExcerciseData();
        //callback?.Invoke(null);
        //StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", null);
    }
}
