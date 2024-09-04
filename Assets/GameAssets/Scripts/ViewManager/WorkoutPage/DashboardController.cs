using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardController : MonoBehaviour, PageController
{
    public Transform content;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        onReloadData(null);
        GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/dashboard/dashboardCreateModel");
        GameObject exerciseObject = Instantiate(exercisePrefab, content);
        exerciseObject.GetComponent<DashboardItemController>().createButton.onClick.AddListener(CreateNewWorkout);

    }

    private void Awake()
    {
    }

    void Start()
    {
        
        print("start");
    }

    public void EditTemplete()
    {
    }

    public void Play()
    {
        if (userSessionManager.Instance.selectedTemplete != null)
        {
            StartEmptyWorkoutWithTemplate(userSessionManager.Instance.selectedTemplete);
        }
    }
    public void CreateNewWorkout()
    {
        StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "createNewWorkoutScreen", null, true, null);
    }
    public void StartEmptyWorkout()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "isTemplateCreator", true }
        };
        Action<object> callback = onReloadData;

        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true, callback);
    }

    public void onReloadData(object data)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var exercise in userSessionManager.Instance.excerciseData.exerciseTemplete)
        {
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { "data", exercise }
            };

            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/dashboard/dashboardDataModel");
            GameObject exerciseObject = Instantiate(exercisePrefab, content);
            print("Oninit");

            DashboardItemController itemController = exerciseObject.GetComponent<DashboardItemController>();
            itemController.onInit(mData);


            //Button button = exerciseObject.GetComponentInChildren<Button>();
            //if (button != null)
            //{
            //    button.onClick.AddListener(() => StartEmptyWorkoutWithTemplate(exercise));
            //}
        }
    }

    private void StartEmptyWorkoutWithTemplate(object exercise)
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "isTemplateCreator", false },
            { "dataTemplate", exercise }
        };

        Action<object> callback = onReloadData;

        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true, callback);
    }

    public void SelectWorkout(GameObject obj)
    {
    }
}
