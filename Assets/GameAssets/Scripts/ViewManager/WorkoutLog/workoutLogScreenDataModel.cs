using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class workoutLogScreenDataModel : MonoBehaviour, ItemController
{
    public Text exerciseNameText;
    ExerciseTypeModel exerciseTypeModel;
    Action<object> callback;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        this.exerciseTypeModel = (ExerciseTypeModel)data["data"];
        exerciseNameText.text = exerciseTypeModel.name;

        if (exerciseTypeModel.exerciseModel.Count > 0)
        {
            foreach (var exerciseModel in exerciseTypeModel.exerciseModel)
            {
                AddSetFromModel(exerciseModel);
            }
        }
        else
        {
            OnAddSet();
        }
    }

    private void AddSetFromModel(ExerciseModel exerciseModel)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/workoutLog/workoutLogSubItems");
        GameObject newSubItem = Instantiate(prefab, transform);
        int childCount = transform.childCount;
        newSubItem.transform.SetSiblingIndex(childCount - 2);
        WorkoutLogSubItem newSubItemScript = newSubItem.GetComponent<WorkoutLogSubItem>();

        Dictionary<string, object> initData = new Dictionary<string, object>
        {
            { "data", exerciseModel }
        };
        newSubItemScript.onInit(initData);
    }

    public void OnAddSet()
    {
        ExerciseModel exerciseModel = new ExerciseModel();
        exerciseTypeModel.exerciseModel.Add(exerciseModel);
        AddSetFromModel(exerciseModel);
    }

    public void onRemoveSet()
    {
        this.callback.Invoke(this.exerciseTypeModel.index);
        GameObject.Destroy(gameObject);
    }
}
