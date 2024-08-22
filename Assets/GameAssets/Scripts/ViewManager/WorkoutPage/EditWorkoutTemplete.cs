using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EditWorkoutTemplete : MonoBehaviour,PageController
{
    public GameObject exerciseDetailPrefab;
    public Transform exerciseParent;
    public InputField workoutName;
    public List<ExerciseInformation> exercises = new List<ExerciseInformation>();

    int addedExercises;

    [System.Serializable]
    public class ExerciseInformation
    {
        public string Name;
        public int Sets;
    }
    public void onInit(Dictionary<string, object> data)
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        workoutName.text = PreferenceManager.Instance.GetString("TempleteName");
        workoutName.onValueChanged.AddListener(OnInputFieldValueChanged);
        addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        for(int i = 0; i < addedExercises; i++)
        {
            GameObject obj=Instantiate(exerciseDetailPrefab,exerciseParent);
            string exerciseName= PreferenceManager.Instance.GetString("Exercise" + i);
            obj.GetComponent<EditWorkoutTempleteExercise>().exersisNameText.text = exerciseName;
            int sets = PreferenceManager.Instance.GetInt("Exercise" + i + "set");
            print(sets);
            for (int j = 1; j < sets; j++)
            {
                obj.GetComponentInChildren<OnClickAddElement>().AddSetOnStart();
            }

            ExerciseInformation info = new ExerciseInformation
            {
                Name = exerciseName,
                Sets = sets
            };
            exercises.Add(info);
        }
    }
    public void AddExerciseButton()
    {
        StateManager.Instance.onRemoveBackHistory();
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true);
    }
    public void RemoveExerciseButton(string exerciseName)
    {
        int index = 0;
        for(int i = 0;i< exercises.Count; i++)
        {
            if (exercises[i].Name == exerciseName)
            {
                index = i; break;
            }
        }
        int _addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        for (int i = 0; i < _addedExercises; i++)
        {
            PreferenceManager.Instance.DeleteKey("Exercise" + i);
            PreferenceManager.Instance.DeleteKey("Exercise" + i + "set");
        }
        exercises.RemoveAt(index);
        PreferenceManager.Instance.SetInt("Exercises", exercises.Count);
        _addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        for (int i = 0; i < _addedExercises; ++i)
        {
            PreferenceManager.Instance.SetString("Exercise" + i, exercises[i].Name);
            PreferenceManager.Instance.SetInt("Exercise" + i + "set", exercises[i].Sets);
        }
    }
    public void AddExerciseSet(string exerciseName)
    {
        int index = 0;
        for (int i = 0; i < exercises.Count; i++)
        {
            if (exercises[i].Name == exerciseName)
            {
                index = i; break;
            }
        }
        PreferenceManager.Instance.SetInt("Exercise" + index + "set", PreferenceManager.Instance.GetInt("Exercise" + index + "set") + 1);
    }
    private void OnInputFieldValueChanged(string newValue)
    {
        // Save the new value to preferences
        PreferenceManager.Instance.SetString("TempleteName", newValue);
    }
    public void ClosePanel()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
        //Destroy(this.gameObject);
    }
}
