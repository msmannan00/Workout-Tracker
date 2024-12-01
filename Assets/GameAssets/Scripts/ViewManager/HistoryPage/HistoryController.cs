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
    public List<GameObject> templeteHistory = new List<GameObject>();
    public List<GameObject> exerciseHistory = new List<GameObject>();
    private bool isCompleted;
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
        isCompleted = true;
        //OnExerciseAdd(list);
    }

    public void Completed()
    {
        if (isCompleted) return;
        isCompleted = true;
        AudioController.Instance.OnButtonClick();
        List<HistoryTempleteModel> list = new List<HistoryTempleteModel>();
        foreach (var workouts in ApiDataHandler.Instance.getHistoryData().exerciseTempleteModel)
        {
            list.Add(workouts);
        }
        VerticalLayoutGroup vlg = content.gameObject.GetComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = true;
        vlg.spacing = 30;
        vlg.childAlignment = TextAnchor.UpperCenter;
        GlobalAnimator.Instance.AnimateRectTransformX(selectionLine, -85f, 0.25f);
        list.Reverse();
        Workout(list);
    }
    public void Exercise()
    {
        OpenExerciseScreen();
        //if (!isCompleted) return;
        //isCompleted = false;
        //AudioController.Instance.OnButtonClick();
        //VerticalLayoutGroup vlg = content.gameObject.GetComponent<VerticalLayoutGroup>();
        //vlg.childControlHeight = false;
        //vlg.spacing = 5;
        //vlg.childAlignment = TextAnchor.UpperLeft;
        //GlobalAnimator.Instance.AnimateRectTransformX(selectionLine, 85f, 0.25f);
        //AllExercises();
    }
    public void Workout(object data)
    {
        //foreach (Transform child in content)
        //{
        //    Destroy(child.gameObject);
        //}
        foreach (GameObject obj in exerciseHistory)
        {
           obj.SetActive(false);
        }

        if (templeteHistory.Count == 0)
        {
            if (data is List<HistoryTempleteModel> dataList)
            {
                foreach (object item in dataList)
                {
                    HistoryTempleteModel typeModel = null;

                    if (item is HistoryTempleteModel dataItem)
                    {
                        typeModel = new HistoryTempleteModel
                        {
                            templeteName = dataItem.templeteName,
                            completedTime = dataItem.completedTime,
                            dateTime = dataItem.dateTime,
                            totalWeight = dataItem.totalWeight,
                            exerciseTypeModel = dataItem.exerciseTypeModel
                            //index = exerciseCounter++
                        };
                    }

                    Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { "data", typeModel } , { "mainParent", gameObject }
                };

                    GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/history/historyScreenDataModel");
                    GameObject exerciseObject = Instantiate(exercisePrefab, content);
                    templeteHistory.Add(exerciseObject);
                    int childCount = content.childCount;
                    exerciseObject.transform.SetSiblingIndex(childCount - 1);
                    exerciseObject.GetComponent<historyScreenDataModel>().onInit(mData, null);
                }
            }
        }
        else
        {
            foreach (GameObject obj in templeteHistory)
            {
                obj.SetActive(true);
            }
        }
    }
    public void OpenExerciseScreen()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "isWorkoutLog", true }, {"ExerciseAddOnPage",ExerciseAddOnPage.HistoryPage}
        };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true, null);
        StateManager.Instance.CloseFooter();
    }

    void AllExercises()
    {
        //foreach (Transform child in content)
        //{
        //    Destroy(child.gameObject);
        //}
        foreach (GameObject obj in templeteHistory)
        {
            obj.SetActive(false);
        }
        if (exerciseHistory.Count == 0)
        {
            ExerciseData exerciseData = ApiDataHandler.Instance.getExerciseData();

            foreach (ExerciseDataItem exercise in exerciseData.exercises)
            {

                GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/exercise/exerciseScreenDataModel");
                GameObject newExerciseObject = Instantiate(exercisePrefab, content);
                exerciseHistory.Add(newExerciseObject);
                ExerciseItem newExerciseItem = newExerciseObject.GetComponent<ExerciseItem>();
                newExerciseObject.GetComponent<Button>().onClick.AddListener(() => ShowExerciseHistory(exercise));
                Dictionary<string, object> initData = new Dictionary<string, object>
                {
                    { "data", exercise },
                };

                newExerciseItem.onInit(initData);

            }
        }
        else
        {
            foreach (GameObject obj in exerciseHistory)
            {
                obj.SetActive(true);
            }
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
        ExerciseData exerciseData = ApiDataHandler.Instance.getExerciseData();
        HistoryModel historyData = ApiDataHandler.Instance.getHistoryData();
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
