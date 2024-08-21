using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutPageController : MonoBehaviour, PageController
{
    public void onInit(Dictionary<string, object> data)
    {
        
    }

    public void EditTemplete()
    {

    }
    public void Play()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true);
    }
    public void StartEmptyWorkout()
    {

    }
}
