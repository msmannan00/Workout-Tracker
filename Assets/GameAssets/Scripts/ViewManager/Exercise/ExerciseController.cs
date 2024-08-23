using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EditWorkoutTemplete;

public class ExerciseController : MonoBehaviour,PageController
{
    public List<ExerciseInformation> exercises = new List<ExerciseInformation>();
    public string templeteName;
    public int totalExercises;
    bool setNew;
    public void onInit(Dictionary<string, object> data)
    {
        
    }
    void Start()
    {
        if (PreferenceManager.Instance.GetBool("EmptyWorkout"))
        {
            
        }
        else 
        {
            templeteName = PreferenceManager.Instance.GetString("Templete" + PreferenceManager.Instance.GetInt("SelectedTemplete"));
            totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
            for (int i = 0; i < totalExercises; i++)
            {
                string exerciseName = PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "name");
                int sets = PreferenceManager.Instance.GetInt(templeteName + "Exercise" + i + "set");

                string note = PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "note");
                ExerciseInformation info = new ExerciseInformation
                {
                    Name = exerciseName,
                    Sets = sets,
                    Note = note
                };
                exercises.Add(info);
            }
        }

        //int addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        //for (int i = 0; i < addedExercises; i++)
        //{
        //    //GameObject obj = Instantiate(exerciseDetailPrefab, exerciseParent);
        //    string exerciseName = PreferenceManager.Instance.GetString("Exercise" + i);
        //    //obj.GetComponent<EditWorkoutTempleteExercise>().exersisNameText.text = exerciseName;
        //    int sets = PreferenceManager.Instance.GetInt("Exercise" + i + "set");
        //    print(sets);
        //    //for (int j = 1; j < sets; j++)
        //    //{
        //    //    obj.GetComponentInChildren<OnClickAddElement>().AddNewSet();
        //    //}

        //    ExerciseInformation info = new ExerciseInformation
        //    {
        //        Name = exerciseName,
        //        Sets = sets
        //    };
        //    exercises.Add(info);
        //}
    }
    public void Close()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("editWorkoutTemplete", gameObject, "editWorkoutTempleteScreen", mData, true);
        Destroy(this.gameObject);
    }
    public void AddExerciseToDefaultTemplete(string exerciseName)
    {

        ExerciseInformation info = new ExerciseInformation
        {
            Name = exerciseName,
            Sets = 1,
            Note=""
        };
        exercises.Add(info);
        SetEditPreference();
    }
    void SetEditPreference()
    {
        int _totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
        for (int i = 0; i < _totalExercises; i++)
        {
            PreferenceManager.Instance.DeleteKey(templeteName + "Exercise" + i + "name");
            PreferenceManager.Instance.DeleteKey(templeteName + "Exercise" + i + "set");
            PreferenceManager.Instance.DeleteKey(templeteName + "Exercise" + i + "note");
        }
        PreferenceManager.Instance.SetInt(templeteName + "TotalExercises", exercises.Count);
        _totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
        for (int i = 0; i < _totalExercises; ++i)
        {
            PreferenceManager.Instance.SetString(templeteName + "Exercise" + i + "name" , exercises[i].Name);
            PreferenceManager.Instance.SetInt(templeteName + "Exercise" + i + "set", exercises[i].Sets);
            PreferenceManager.Instance.SetString(templeteName + "Exercise" + i + "note", exercises[i].Note);
        }


        Dictionary<string, object> mData = new Dictionary<string, object> { };
        
        StateManager.Instance.OpenStaticScreen("editWorkoutTemplete", gameObject, "editWorkoutTempleteScreen", mData, true);

        StateManager.Instance.onRemoveBackHistory();
    }
    public void AddExerciseToNewTemplete(string exerciseName)
    {

        ExerciseInformation info = new ExerciseInformation
        {
            Name = exerciseName,
            Sets = 1,
            Note = ""
        };
        exercises.Add(info);
        if (!PreferenceManager.Instance.HasKey("newTempleteExercise"))
        {
            PreferenceManager.Instance.SetInt("newTempleteExercise", 1);
            int totalexercises= PreferenceManager.Instance.GetInt("newTempleteExercise")-1;
            PreferenceManager.Instance.SetString("newTempleteExercise" + totalexercises + "name", info.Name);
            PreferenceManager.Instance.SetInt("newTempleteExercise" + totalexercises + "set", info.Sets);
        }
        else
        {
            PreferenceManager.Instance.SetInt("newTempleteExercise", PreferenceManager.Instance.GetInt("newTempleteExercise")+1);
            int totalexercises = PreferenceManager.Instance.GetInt("newTempleteExercise") - 1;
            PreferenceManager.Instance.SetString("newTempleteExercise" + totalexercises + "name", info.Name);
            PreferenceManager.Instance.SetInt("newTempleteExercise" + totalexercises + "set", info.Sets);
        }
        Dictionary<string, object> mData = new Dictionary<string, object> { };

        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true);
        StateManager.Instance.onRemoveBackHistory();

    }

}
