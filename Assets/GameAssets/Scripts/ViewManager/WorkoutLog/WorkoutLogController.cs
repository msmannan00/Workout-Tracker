using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EditWorkoutTemplete;

public class WorkoutLogController : MonoBehaviour,PageController
{
    public Text workoutNameText;
    public Text timerText;
    public GameObject exercisePrefab;
    public Transform exerciseParent;

    public List<ExerciseInformation> exercises = new List<ExerciseInformation>();
    
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
        workoutNameText.text = PreferenceManager.Instance.GetString("TempleteName");
        int addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        for (int i = 0; i < addedExercises; i++)
        {
            GameObject obj = Instantiate(exercisePrefab, exerciseParent);
            obj.transform.SetSiblingIndex(i);
            string exerciseName = PreferenceManager.Instance.GetString("Exercise" + i);
            obj.GetComponent<WorkoutLogExercise>().exerciseNameText.text = exerciseName;
            int sets = PreferenceManager.Instance.GetInt("Exercise" + i + "set");
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
    public void AddExerciseButton()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true);
    }
    public void Finish()
    {
        Timer.Instance.StopTimer();
        Timer.Instance.ResetTimer();
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
    }
    public void Cancel()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
    }
}
