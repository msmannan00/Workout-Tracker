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
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "isTemplateCreator", true }
        };
        Action<object> callback = onReloadData;

        NavigationManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true, callback);
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
            DashboardItemController itemController = exercisePrefab.GetComponent<DashboardItemController>();
            itemController.onInit(mData);

            GameObject exerciseObject = Instantiate(exercisePrefab, content);

            Button button = exerciseObject.GetComponentInChildren<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => StartEmptyWorkoutWithTemplate(exercise));
            }
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

        NavigationManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true, callback);
    }

    public void SelectWorkout(GameObject obj)
    {
    }
}
