using DG.Tweening;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryController : MonoBehaviour, PageController
{
    public Transform content;
    public RectTransform selectionLine;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {

    }
    private void Start()
    {
        //List<HistoryTempleteModel> list = new List<HistoryTempleteModel>();
        //foreach (var workouts in userSessionManager.Instance.historyData.exerciseTempleteModel)
        //{
        //    list.Add(workouts);
        //}
        Completed();
        //OnExerciseAdd(list);
    }

    public void Completed()
    {
        List<HistoryTempleteModel> list = new List<HistoryTempleteModel>();
        foreach (var workouts in userSessionManager.Instance.historyData.exerciseTempleteModel)
        {
            list.Add(workouts);
        }
        VerticalLayoutGroup vlg = content.gameObject.GetComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = true;
        vlg.spacing = 30;
        vlg.childAlignment = TextAnchor.UpperCenter;
        GlobalAnimator.Instance.AnimateRectTransformX(selectionLine, -85f, 0.25f);
        OnExerciseAdd(list);
    }
    public void Exercise()
    {
        VerticalLayoutGroup vlg = content.gameObject.GetComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = false;
        vlg.spacing = 5;
        vlg.childAlignment = TextAnchor.UpperLeft;
        GlobalAnimator.Instance.AnimateRectTransformX(selectionLine, 85f, 0.25f);
        AllExercises();
    }
    public void OnExerciseAdd(object data)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
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
                        dateTime= dataItem.dateTime,
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

    void AllExercises()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        ExerciseData exerciseData = DataManager.Instance.getExerciseData();

        foreach (ExerciseDataItem exercise in exerciseData.exercises)
        {

            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/exercise/exerciseScreenDataModel");
            GameObject newExerciseObject = Instantiate(exercisePrefab, content);

            ExerciseItem newExerciseItem = newExerciseObject.GetComponent<ExerciseItem>();
            newExerciseObject.GetComponent<Button>().onClick.AddListener(()=>ShowExerciseHistory(exercise));
            Dictionary<string, object> initData = new Dictionary<string, object>
            {
                { "data", exercise },
            };

            newExerciseItem.onInit(initData);

        }
    }
    void ShowExerciseHistory(ExerciseDataItem exercise)
    {
        Dictionary<string, object> initData = new Dictionary<string, object>
        {
                { "data", exercise },
        };
        StateManager.Instance.OpenStaticScreen("history", gameObject, "exerciseHistoryScreen", initData,keepState: true);
        StateManager.Instance.CloseFooter();
    }
    void PerformedExercises()
    {
        foreach(Transform child in content)
        {
            Destroy(child.gameObject);
        }
        ExerciseData exerciseData = DataManager.Instance.getExerciseData();
        HistoryModel historyData = userSessionManager.Instance.historyData;
        List<string> filterExercises = GetUniqueExercises(historyData);
        //string lowerFilter = filter.ToLower(); // Convert filter to lowercase for case-insensitive comparison

        foreach (ExerciseDataItem exercise in exerciseData.exercises)
        {
            // Check if the exercise name is in the list of filterExercises
            if (filterExercises != null && !filterExercises.Contains(exercise.exerciseName))
            {
                continue;
            }

            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/exercise/exerciseScreenDataModel");
            GameObject newExerciseObject = Instantiate(exercisePrefab, content);

            ExerciseItem newExerciseItem = newExerciseObject.GetComponent<ExerciseItem>();

            Dictionary<string, object> initData = new Dictionary<string, object>
            {
                { "data", exercise },
            };

            newExerciseItem.onInit(initData);

        }
    }
    public static List<string> GetUniqueExercises(HistoryModel historyData)
    {
        // Create a HashSet to store unique exercise names
        HashSet<string> uniqueExercises = new HashSet<string>();

        // Iterate over each HistoryTempleteModel in the historyData
        foreach (var template in historyData.exerciseTempleteModel)
        {
            // Iterate over each HistoryExerciseTypeModel in the current HistoryTempleteModel
            foreach (var exerciseType in template.exerciseTypeModel)
            {
                // Add each exercise name to the HashSet
                uniqueExercises.Add(exerciseType.exerciseName);
            }
        }

        // Convert HashSet to List and return it
        return uniqueExercises.ToList();
    }
    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }
   
}
