using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorkoutPageController : MonoBehaviour, PageController
{
    public string templatesFolder = "WorkoutTemplates";
    public GameObject templetePrefab;
    public Transform templeteParent;
    public List<DefaultTemplete> defaultTempletes = new List<DefaultTemplete>();
    private int totalTempletes;
    private List<GameObject> templeteObjects=new List<GameObject>();
    public void onInit(Dictionary<string, object> data)
    {
        
    }
    
    private void Awake()
    {
        if (!PreferenceManager.Instance.HasKey("TotalTempletes"))
        {
            PreferenceManager.Instance.SetInt("TotalTempletes", defaultTempletes.Count);
            totalTempletes = PreferenceManager.Instance.GetInt("TotalTempletes");
            for (int i = 0; i < totalTempletes; i++)
            {
                GameObject temp = Instantiate(templetePrefab, templeteParent);
                temp.GetComponent<WorkoutTempleteModel>().SetTempleteValuesFirstTime(defaultTempletes[i],i);
                templeteObjects.Add(temp);
            }
        }
        else
        {
            totalTempletes = PreferenceManager.Instance.GetInt("TotalTempletes");
            for (int i = 0; i < totalTempletes; i++)
            {
                GameObject temp = Instantiate(templetePrefab, templeteParent);
                temp.GetComponent<WorkoutTempleteModel>().SetTempleteValues(i);
                templeteObjects.Add(temp);
            }
        }
        for(int i=0;i<templeteObjects.Count;i++)
        {
            if (i == 0)
            {
                templeteObjects[i].GetComponent<Outline>().enabled = true;
                PreferenceManager.Instance.SetInt("SelectedTemplete",i);
            }
            else
            {
                templeteObjects[i].GetComponent<Outline>().enabled = false;
            }
        }
    }
    void Start()
    {
        if(PreferenceManager.Instance.HasKey("newTempleteExercise"))
        {
            int totalexercises = PreferenceManager.Instance.GetInt("newTempleteExercise");
            for(int i = 0;i < totalexercises; i++)

            {
                PreferenceManager.Instance.DeleteKey("newTempleteExercise" + i + "name");
                PreferenceManager.Instance.DeleteKey("newTempleteExercise" + i + "set");
            }
           
        }
        if (PreferenceManager.Instance.HasKey("newTempleteExercise"))
        {
            PreferenceManager.Instance.DeleteKey("newTempleteExercise");
        }
        if (PreferenceManager.Instance.HasKey("EmptyWorkout"))
        {
            print("sadfads");
            PreferenceManager.Instance.DeleteKey("EmptyWorkout");
        }
    }
    public void EditTemplete()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        PreferenceManager.Instance.SetBool("EmptyWorkout", false);
        StateManager.Instance.OpenStaticScreen("editWorkoutTemplete", gameObject, "editWorkoutTempleteScreen", mData, true);
    }
    public void Play()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        PreferenceManager.Instance.SetBool("EmptyWorkout", false);
        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true);
    }
    public void StartEmptyWorkout()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        PreferenceManager.Instance.SetBool("EmptyWorkout", true);
        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true);
    }
    public void SelectWorkout(GameObject obj)
    {
        for(int i=0; i < templeteObjects.Count; i++)
        {
            if (templeteObjects[i] == obj)
            {
                templeteObjects[i].GetComponent<Outline>().enabled = true;
                PreferenceManager.Instance.SetInt("SelectedTemplete", i);
            }
            else
            {
                templeteObjects[i].GetComponent<Outline>().enabled = false;
            }
        }
    }
}
