using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PersonalBestController : MonoBehaviour, PageController
{
    public Transform content;
    public TextMeshProUGUI messageText;
    public PersonalBestData _data;
    public List<string> haveExercises = new List<string>();
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        _data = DataManager.Instance.getPersonalBestData();
        LoadData();
    }
    public void LoadData()
    {
        foreach (PersonalBestDataItem item in _data.exercises)
        {
            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/profile/personalBestSubItem");
            GameObject exerciseObject = Instantiate(exercisePrefab, content);
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { "data", item }
            };
            exerciseObject.GetComponent<ItemController>().onInit(mData, null);
            haveExercises.Add(item.exerciseName.ToLower());
        }
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
                    if (!haveExercises.Contains(dataItem.exerciseName.ToLower()))
                    {
                        PersonalBestDataItem _data = new PersonalBestDataItem();
                        _data.exerciseName = dataItem.exerciseName;
                        _data.weight = 0;
                        GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/profile/personalBestSubItem");
                        GameObject exerciseObject = Instantiate(exercisePrefab, content);
                        Dictionary<string, object> mData = new Dictionary<string, object>
                        {
                            { "data", _data }
                        };
                        exerciseObject.GetComponent<ItemController>().onInit(mData, null);
                        haveExercises.Add(dataItem.exerciseName.ToLower());
                    }
                    else
                    {
                        ShowTextForOneSecond("Exercise already added.");
                    }
                }

            }
            DataManager.Instance.SavePersonalBestData();
        }
    }
    public void Back()
    {
        DataManager.Instance.SavePersonalBestData();
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null, null, false);
    }
    public void ShowTextForOneSecond(string message)
    {
        // Set the text and ensure it's fully visible (alpha = 1)
        messageText.text = message;
        messageText.alpha = 1;

        // Hide the text after 1 second using a fade-out animation
        messageText.DOFade(0, 1f).SetDelay(1f);  // Wait for 1 second, then fade out over 1 second
    }
}
