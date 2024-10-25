using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DashboardController : MonoBehaviour, PageController
{
    //public List<TextMeshProUGUI> headingTexts;
    //public TextMeshProUGUI headingColorText;
    //public List<Image> footerButtonImages;
    //public List<Image> headerButtonImages;
    //public GameObject bottomMiddelObject;
    public TMP_InputField searchInputField;
    //public Image searchIcon1, searchIcon2, topButtonBar;
    public Transform content;
    public RectTransform switchButton;
    public TextMeshProUGUI switchWorkout, switchSplit;
    List<GameObject> items = new List<GameObject>();
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        onReloadData(null);
        searchInputField.onValueChanged.AddListener(OnSearchChanged);
        Workout();
    }
    private void OnEnable()
    {
        foreach(GameObject go in items)
        {
            go.SetActive(false);
            go.SetActive(true);
        }
        Workout();
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
        Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { "workoutName", "Workout " + content.childCount }
            };
        StateManager.Instance.OpenStaticScreen("createWorkout", gameObject, "createNewWorkoutScreen", mData, true, onReloadData,true);
        StateManager.Instance.CloseFooter();
    }
    public void StartEmptyWorkout()
    {
        DefaultTempleteModel exercise=new DefaultTempleteModel();
        exercise.templeteName="Workout " + content.childCount.ToString();
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            {"dataTemplate", exercise }, { "isTemplateCreator", true }
        };
        Action<object> callback = onReloadData;

        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData, true, callback, true);
        StateManager.Instance.CloseFooter();
    }

    public void onReloadData(object data)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var exercise in ApiDataHandler.Instance.getTemplateData().exerciseTemplete)
        {
            DefaultTempleteModel templeteData = exercise;
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { "data", templeteData }
            };

            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/dashboard/dashboardDataModel");
            GameObject exerciseObject = Instantiate(exercisePrefab, content);

            DashboardItemController itemController = exerciseObject.GetComponent<DashboardItemController>();
            itemController.onInit(mData,StartEmptyWorkoutWithTemplate);
        }
    }

    public void StartEmptyWorkoutWithTemplate(object exercise)
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "isTemplateCreator", false },
            { "dataTemplate", exercise }
        };

        //Action<object> callback = onReloadData;

        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData,true);
    }

    void OnSearchChanged(string searchQuery)
    {
        LoadExerciseTemplates(searchQuery);
    }
    void LoadExerciseTemplates(string filter = "")
    {
        foreach(Transform child in content)
        {
            Destroy(child.gameObject) ;
        }
        TemplateData exerciseData = ApiDataHandler.Instance.getTemplateData();

        bool showAll = string.IsNullOrEmpty(filter);

        foreach (DefaultTempleteModel template in exerciseData.exerciseTemplete)
        {
            string lowerFilter = filter.ToLower();

            // Check if the templateName or any exerciseType name matches the filter
            if (!showAll &&
                !(template.templeteName.ToLower().Contains(lowerFilter) ||
                  template.exerciseTemplete.Any(e => e.name.ToLower().Contains(lowerFilter))))
            {
                continue;
            }

            // If a match is found, spawn the dashboard prefab
            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/dashboard/dashboardDataModel");
            GameObject newExerciseObject = Instantiate(exercisePrefab, content);

            // Initialize data for the dashboard item
            Dictionary<string, object> initData = new Dictionary<string, object>
            {
                { "data", template }
            };

            // Initialize the DashboardItemController
            DashboardItemController itemController = newExerciseObject.GetComponent<DashboardItemController>();
            itemController.onInit(initData, StartEmptyWorkoutWithTemplate);


        }
    }
 
    public void OnHistory()
    {
        StateManager.Instance.OpenStaticScreen("history", gameObject, "historyScreen", null, true, null);
    }

    public void Workout()
    {
        GlobalAnimator.Instance.AnimateRectTransformX(switchButton, -3, 0.25f);
        switch(ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                switchWorkout.color = new Color32(255, 255, 255, 255);
                switchSplit.color = new Color32(92, 59, 28, 155);
                break;
            case Theme.Dark:
                switchWorkout.color = new Color32(51, 23, 23, 255);
                switchSplit.color = new Color32(171, 162, 162, 255);
                break;
        }
    }
    public void Splits()
    {
        GlobalAnimator.Instance.AnimateRectTransformX(switchButton, 141, 0.25f);
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                switchSplit.color = new Color32(255, 255, 255, 255);
                switchWorkout.color = new Color32(92, 59, 28, 155);
                break;
            case Theme.Dark:
                switchSplit.color = new Color32(51, 23, 23, 255);
                switchWorkout.color = new Color32(171, 162, 162, 255);
                break;
        }
    }

}
