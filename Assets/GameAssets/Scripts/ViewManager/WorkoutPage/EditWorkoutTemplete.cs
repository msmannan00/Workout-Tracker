using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    int totalExercises;
    string templeteName;
    [System.Serializable]
    public class ExerciseInformation
    {
        public string Name;
        public int Sets;
        public string Note;
    }
    public void onInit(Dictionary<string, object> data)
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        templeteName = PreferenceManager.Instance.GetString("Templete" + PreferenceManager.Instance.GetInt("SelectedTemplete"));


        workoutName.text = templeteName;
        workoutName.onValueChanged.AddListener(OnInputFieldValueChanged);

        totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
        for (int i = 0; i < totalExercises; i++)
        {
            GameObject obj=Instantiate(exerciseDetailPrefab,exerciseParent);
            string exerciseName= PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "name");
            obj.GetComponent<EditWorkoutTempleteExercise>().exersisNameText.text = exerciseName;
            int sets = PreferenceManager.Instance.GetInt(templeteName + "Exercise" + i + "set");
            print(sets);
            for (int j = 1; j < sets; j++)
            {
                obj.GetComponentInChildren<OnClickAddElement>().AddSetOnStart();
            }
            string note= PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "note");
            ExerciseInformation info = new ExerciseInformation
            {
                Name = exerciseName,
                Sets = sets,
                Note=note
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
        for(int i = 0;i < exercises.Count; i++)
        {
            if (exercises[i].Name == exerciseName)
            {
                index = i; break;
            }
        }
         //templeteName = PreferenceManager.Instance.GetString("Templete" + PreferenceManager.Instance.GetInt("SelectedTemplete"));
        int _totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
        for (int i = 0; i < _totalExercises; i++)
        {
            PreferenceManager.Instance.DeleteKey(templeteName + "Exercise" + i + "name");
            PreferenceManager.Instance.DeleteKey(templeteName + "Exercise" + i + "set");
            PreferenceManager.Instance.DeleteKey(templeteName + "Exercise" + i + "note");
        }
        exercises.RemoveAt(index);
        PreferenceManager.Instance.SetInt(templeteName + "TotalExercises", exercises.Count);
        _totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
        for (int i = 0; i < _totalExercises; ++i)
        {
            PreferenceManager.Instance.SetString(templeteName + "Exercise" + i + "name" , exercises[i].Name);
            PreferenceManager.Instance.SetInt(templeteName + "Exercise" + i + "set", exercises[i].Sets);
            PreferenceManager.Instance.SetString(templeteName + "Exercise" + i + "note", exercises[i].Note);
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
        PreferenceManager.Instance.SetInt(templeteName + "Exercise" + index + "set", PreferenceManager.Instance.GetInt(templeteName + "Exercise" + index + "set") + 1);
    }
    private void OnInputFieldValueChanged(string newValue)
    {
        // Save the new value to preferences
        PreferenceManager.Instance.SetString("Templete" + PreferenceManager.Instance.GetInt("SelectedTemplete"), newValue);
    }
    public void ClosePanel()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
        //Destroy(this.gameObject);
    }
}
