using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddNewExercise : MonoBehaviour, PageController
{
    public TMP_InputField exerciseName;
    public TMP_InputField categoryName;
    public TMP_InputField rank;
    public Toggle isWeightExercise;

    public ExerciseDataItem exerciseDataItem;
    private Action<ExerciseDataItem> callback;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
    }
    private void Start()
    {
        exerciseDataItem = new ExerciseDataItem();

        // Subscribe to the onValueChanged events
        exerciseName.onValueChanged.AddListener(OnExerciseNameChanged);
        categoryName.onValueChanged.AddListener(OnCategoryNameChanged);
        rank.onValueChanged.AddListener(OnRankChanged);
        isWeightExercise.onValueChanged.AddListener(OnIsWeightExerciseChanged);
    }

    private void OnExerciseNameChanged(string value)
    {
        exerciseDataItem.exerciseName = value;
    }

    private void OnCategoryNameChanged(string value)
    {
        exerciseDataItem.category = value;
    }

    private void OnRankChanged(string value)
    {
        // Try to parse the rank string to an int
        if (int.TryParse(value, out int result))
        {
            exerciseDataItem.rank = result;
        }
        else
        {
            Debug.LogWarning("Invalid rank value. Please enter a valid integer.");
        }
    }

    private void OnIsWeightExerciseChanged(bool value)
    {
        exerciseDataItem.isWeightExercise = value;
    }

    public void Save()
    {
        DataManager.Instance.SaveData(exerciseDataItem);
        callback.Invoke(exerciseDataItem);
        OnClose();
    }
    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }
}
