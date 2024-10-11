using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalBestController : MonoBehaviour,PageController
{
    public Transform content; 
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        
    }
    public void AddExerciseButton()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "isWorkoutLog", false }, {"ExerciseAddOnPage",ExerciseAddOnPage.PersonalBestPage}
        };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true, OnExerciseAdd, true);
        StateManager.Instance.CloseFooter();
    }
    public void OnExerciseAdd(object data)
    {
        if (data is List<ExerciseDataItem> dataList)
        {
            foreach (object item in dataList)
            {
                if (item is ExerciseDataItem dataItem)
                {
                }
                GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/profile/personalBestSubItem");
                GameObject exerciseObject = Instantiate(exercisePrefab, content);
                exerciseObject.transform.SetSiblingIndex(content.childCount - 2);
                //exerciseObject.GetComponent<workoutLogScreenDataModel>().onInit(mData, null);
            }
        }
    }
}
