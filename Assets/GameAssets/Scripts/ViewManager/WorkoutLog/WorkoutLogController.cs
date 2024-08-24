using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutLogController : MonoBehaviour,PageController
{
    public InputField workoutNameText;
    public Text timerText;
    public Transform content;

    public List<ExerciseInformationModel> exercises = new List<ExerciseInformationModel>();

    public void onInit(Dictionary<string, object> data)
    {


    }

    private void Awake()
    {
    }

    private void Start()
    {
        
    }

    public void AddExerciseSet(string exerciseName)
    {
    }

    public void AddExerciseButton()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true);
    }

    public void Finish()
    {
    }

    public void Cancel()
    {
    }
}
