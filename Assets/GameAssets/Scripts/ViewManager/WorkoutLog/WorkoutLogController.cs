using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EditWorkoutTemplete;

public class WorkoutLogController : MonoBehaviour,PageController
{
    public InputField workoutNameText;
    public Text timerText;
    public GameObject exercisePrefab;
    public Transform exerciseParent;

    public List<ExerciseInformation> exercises = new List<ExerciseInformation>();

    string templeteName;
    int totalExercises;
    public void onInit(Dictionary<string, object> data)
    {
    }
    private void Awake()
    {
        Timer.Instance.timerText = timerText;
        Timer.Instance.StartTimer();
    }
    private void Start()
    {
        
        if( PreferenceManager.Instance.HasKey("newTempleteExercise"))
        {
            int total = PreferenceManager.Instance.GetInt("newTempleteExercise");
            for (int i = 0; i < total; i++)
            {
                GameObject obj = Instantiate(exercisePrefab, exerciseParent);
                obj.transform.SetSiblingIndex(i);

                string exerciseName = PreferenceManager.Instance.GetString("newTempleteExercise" + i + "name");
                int sets = PreferenceManager.Instance.GetInt("newTempleteExercise" + i + "set");
                string note = PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "note");

                obj.GetComponent<WorkoutLogExercise>().exerciseNameText.text = exerciseName;

                for (int j = 1; j < sets; j++)
                {
                    obj.GetComponentInChildren<OnClickAddElement>().AddSetOnStart();
                }
                ExerciseInformation info = new ExerciseInformation
                {
                    Name = exerciseName,
                    Sets = sets,
                    Note = note
                };
                exercises.Add(info);
            }
        }
        else if (PreferenceManager.Instance.GetBool("EmptyWorkout"))
        {
            templeteName = "New Workout";
            workoutNameText.text = templeteName;
        }
        else
        {
            templeteName = PreferenceManager.Instance.GetString("Templete" + PreferenceManager.Instance.GetInt("SelectedTemplete"));
            totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
            workoutNameText.text = templeteName;
            for (int i = 0; i < totalExercises; i++)
            {
                GameObject obj = Instantiate(exercisePrefab, exerciseParent);
                obj.transform.SetSiblingIndex(i);

                string exerciseName = PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "name");
                int sets = PreferenceManager.Instance.GetInt(templeteName + "Exercise" + i + "set");
                string note = PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "note");

                obj.GetComponent<WorkoutLogExercise>().exerciseNameText.text = exerciseName;

                for (int j = 1; j < sets; j++)
                {
                    obj.GetComponentInChildren<OnClickAddElement>().AddSetOnStart();
                }
                ExerciseInformation info = new ExerciseInformation
                {
                    Name = exerciseName,
                    Sets = sets,
                    Note = note
                };
                exercises.Add(info);
            }
        }
    }
    public void AddExerciseSet(string exerciseName)
    {
        if (PreferenceManager.Instance.GetBool("EmptyWorkout"))
        {
            int index = 0;
            for (int i = 0; i < exercises.Count; i++)
            {
                if (exercises[i].Name == exerciseName)
                {
                    index = i; break;
                }
            }
            PreferenceManager.Instance.SetInt("newTempleteExercise" +  index + "set", PreferenceManager.Instance.GetInt("newTempleteExercise" + index + "set") + 1);
        }
        else
        {
            print("222222");
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
    }
    public void AddExerciseButton()
    {
        if (PreferenceManager.Instance.GetBool("EmptyWorkout"))
        {
            
            Dictionary<string, object> mData = new Dictionary<string, object> { };
            StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true);
        }
        else
        {
            Dictionary<string, object> mData = new Dictionary<string, object> { };
            StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true);
        }
    }
    //public void SetNewTemplete(ExerciseInformation exercise)
    //{
    //    GameObject obj = Instantiate(exercisePrefab, exerciseParent);
    //    obj.transform.SetAsFirstSibling();

    //    obj.GetComponent<WorkoutLogExercise>().exerciseNameText.text = exercise.Name;
    //    exercises.Add(exercise);
        
    //}
    public void Finish()
    {
        Timer.Instance.StopTimer();
        Timer.Instance.ResetTimer();
        if (PreferenceManager.Instance.GetBool("EmptyWorkout"))
        {
            PreferenceManager.Instance.SetInt("TotalTempletes", PreferenceManager.Instance.GetInt("TotalTempletes")+1);
            int totalTempletes= PreferenceManager.Instance.GetInt("TotalTempletes");
            int index = totalTempletes - 1;
            if (!PreferenceManager.Instance.HasKey("Templete" + index))
            {
                PreferenceManager.Instance.SetString("Templete" + index, workoutNameText.text);
            }
            templeteName = PreferenceManager.Instance.GetString("Templete" + index);
            //for (int i = 0; i < dt.exerciseTemplete.Count; i++)
            //{
            if (!PreferenceManager.Instance.HasKey(templeteName + "TotalExercises"))
            {
                PreferenceManager.Instance.SetInt(templeteName + "TotalExercises", PreferenceManager.Instance.GetInt("newTempleteExercise"));
            }

            totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
            for (int j = 0; j < totalExercises; ++j)
            {
                if (!PreferenceManager.Instance.HasKey(templeteName + "Exercise" + j + "name"))
                {
                    PreferenceManager.Instance.SetString(templeteName + "Exercise" + j + "name", PreferenceManager.Instance.GetString("newTempleteExercise" + j + "name"));
                }
                if (!PreferenceManager.Instance.HasKey(templeteName + "Exercise" + j + "set"))
                {
                    PreferenceManager.Instance.SetInt(templeteName + "Exercise" + j + "set", PreferenceManager.Instance.GetInt("newTempleteExercise" + j + "set"));
                }
                if (!PreferenceManager.Instance.HasKey(templeteName + "Exercise" + j + "note"))
                {
                    PreferenceManager.Instance.SetString(templeteName + "Exercise" + j + "note", PreferenceManager.Instance.GetString("newTempleteExercise" + j + "note"));
                }
            }
            Dictionary<string, object> mData = new Dictionary<string, object> { };
            StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
        }
        else
        {
            Dictionary<string, object> mData = new Dictionary<string, object> { };
            StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
        }
    }
    public void Cancel()
    {
        Timer.Instance.StopTimer();
        Timer.Instance.ResetTimer();
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
    }
}
