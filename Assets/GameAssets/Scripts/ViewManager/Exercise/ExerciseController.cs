using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseController : MonoBehaviour,PageController
{
    public void onInit(Dictionary<string, object> data)
    {
        //throw new System.NotImplementedException();
    }
    public void Close()
    {
        StateManager.Instance.onRemoveBackHistory();
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true);
        //Destroy(this.gameObject);
    }
}
