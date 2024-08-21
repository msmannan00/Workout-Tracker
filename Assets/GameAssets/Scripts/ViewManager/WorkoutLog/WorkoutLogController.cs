using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutLogController : MonoBehaviour,PageController
{
    public void onInit(Dictionary<string, object> data)
    {

    }

    public void AddExercise()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true);
    }
    public void Cancel()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("workoutPage", gameObject, "workoutPageScreen", mData);
    }
}
