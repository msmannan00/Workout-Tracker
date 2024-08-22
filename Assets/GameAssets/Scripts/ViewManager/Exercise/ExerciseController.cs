using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EditWorkoutTemplete;

public class ExerciseController : MonoBehaviour,PageController
{
    public List<ExerciseInformation> exercises = new List<ExerciseInformation>();
    public void onInit(Dictionary<string, object> data)
    {
        
    }
    void Start()
    {
        int addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        for (int i = 0; i < addedExercises; i++)
        {
            //GameObject obj = Instantiate(exerciseDetailPrefab, exerciseParent);
            string exerciseName = PreferenceManager.Instance.GetString("Exercise" + i);
            //obj.GetComponent<EditWorkoutTempleteExercise>().exersisNameText.text = exerciseName;
            int sets = PreferenceManager.Instance.GetInt("Exercise" + i + "set");
            print(sets);
            //for (int j = 1; j < sets; j++)
            //{
            //    obj.GetComponentInChildren<OnClickAddElement>().AddNewSet();
            //}

            ExerciseInformation info = new ExerciseInformation
            {
                Name = exerciseName,
                Sets = sets
            };
            exercises.Add(info);
        }
    }
    public void Close()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("editWorkoutTemplete", gameObject, "editWorkoutTempleteScreen", mData, true);
        Destroy(this.gameObject);
    }
    public void AddExerciseToTemplete(string exerciseName)
    {
        ExerciseInformation info = new ExerciseInformation
        {
            Name = exerciseName,
            Sets = 1
        };
        exercises.Add(info);
        SetPreference();
    }
    void SetPreference()
    {
        
        int addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        for (int i = 0; i < addedExercises; i++)
        {
            PreferenceManager.Instance.DeleteKey("Exercise" + i);
            PreferenceManager.Instance.DeleteKey("Exercise" + i + "set");
        }
        PreferenceManager.Instance.SetInt("Exercises", exercises.Count);
        addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        for (int i = 0; i < addedExercises; ++i)
        {
            PreferenceManager.Instance.SetString("Exercise" + i, exercises[i].Name);
            PreferenceManager.Instance.SetInt("Exercise" + i + "set", exercises[i].Sets);

        }
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        if (StateManager.Instance.checkPageByTag("editWorkoutTempleteScreen"))
        {
            StateManager.Instance.OpenStaticScreen("editWorkoutTemplete", gameObject, "editWorkoutTempleteScreen", mData, true);
        }
        else if (StateManager.Instance.checkPageByTag("workoutLogScreen"))
        {
            StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true);
        }
        StateManager.Instance.onRemoveBackHistory();
    }
}
