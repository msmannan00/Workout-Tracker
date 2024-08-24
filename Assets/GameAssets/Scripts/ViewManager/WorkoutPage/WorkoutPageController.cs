using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorkoutPageController : MonoBehaviour, PageController
{
    public Transform content;
    public List<DefaultTempleteModel> defaultTempletes = new List<DefaultTempleteModel>();
    private List<GameObject> templeteObjects=new List<GameObject>();
    public void onInit(Dictionary<string, object> data)
    {
        
    }
    
    private void Awake()
    {
    }
    void Start()
    {
    }
    public void EditTemplete()
    {
    }
    public void Play()
    {
    }
    public void StartEmptyWorkout()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        PreferenceManager.Instance.SetBool("EmptyWorkout", true);
        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true);
    }
    public void SelectWorkout(GameObject obj)
    {
    }
}
