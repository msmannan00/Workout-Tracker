using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardController : MonoBehaviour, PageController
{
    public List<TextMeshProUGUI> headingTexts;
    public TextMeshProUGUI headingColorText;
    public List<Image> footerButtonImages;
    public List<Image> headerButtonImages;
    public GameObject bottomMiddelObject;
    public TMP_InputField searchInputField;
    public Image searchIcon1, searchIcon2, topButtonBar;
    public Transform content;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        onReloadData(null);
        searchInputField.onValueChanged.AddListener(OnSearchChanged);
        
    }
    private void OnEnable()
    {
        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Dark:
               // this.GetComponent<Image>().color = userSessionManager.Instance.darkBgColor;
                headingColorText.color = Color.white;
                topButtonBar.color = new Color32(51, 23, 23, 255);
                searchIcon1.color = userSessionManager.Instance.darkSearchIconColor;
                searchIcon2.color = userSessionManager.Instance.darkSearchIconColor;
                searchInputField.textComponent.color = userSessionManager.Instance.darkSearchIconColor;
                searchInputField.placeholder.color = userSessionManager.Instance.darkSearchIconColor;
                searchInputField.GetComponent<Image>().color = userSessionManager.Instance.darkSearchBarColor;
                foreach (TextMeshProUGUI text in headingTexts)
                {
                    text.font = userSessionManager.Instance.darkHeadingFont;
                }
                foreach (Image image in footerButtonImages)
                {
                    image.color = Color.red;
                    foreach(Transform child in image.gameObject.transform)
                    {
                        if(child.GetComponent<Image>() != null)
                            child.GetComponent<Image>().color = Color.white;
                    }
                }
                BottomButtonSelectionSeter(bottomMiddelObject);
                foreach (Image image in headerButtonImages)
                {
                    image.color = Color.white;
                }
                break;
            case Theme.Light:
                topButtonBar.color = Color.white;
              //  this.GetComponent<Image>().color = userSessionManager.Instance.lightBgColor;
                headingColorText.color = userSessionManager.Instance.lightHeadingColor;
                searchIcon1.color = userSessionManager.Instance.lightTextColor;
                searchIcon2.color = userSessionManager.Instance.lightTextColor;
                searchInputField.GetComponent<Image>().color = Color.white;
                searchInputField.textComponent.color = userSessionManager.Instance.lightTextColor;
                searchInputField.placeholder.color = userSessionManager.Instance.lightTextColor;
                foreach (TextMeshProUGUI text in headingTexts)
                {
                    text.font = userSessionManager.Instance.lightHeadingFont;
                }
                foreach (Image image in footerButtonImages)
                {
                    image.color = Color.white;
                    foreach (Transform child in image.gameObject.transform)
                    {
                        if (child.GetComponent<Image>() != null)
                            child.GetComponent<Image>().color = Color.red;
                    }
                }
                BottomButtonSelectionSeter(bottomMiddelObject);
                foreach (Image image in headerButtonImages)
                {
                    image.color = userSessionManager.Instance.lightButtonColor;
                }
                break;
        }
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
        foreach (var exercise in userSessionManager.Instance.excerciseData.exerciseTemplete)
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

        StateManager.Instance.OpenStaticScreen("workoutLog", gameObject, "workoutLogScreen", mData);
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

        ExcerciseData exerciseData = userSessionManager.Instance.excerciseData;

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
    public void BottomButtonSelectionSeter(GameObject clickedObject)
    {
        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Dark:
                foreach(Image img in footerButtonImages)
                {
                    if (img.gameObject == clickedObject)
                        img.enabled = true;
                    else
                        img.enabled = false;
                }
                break;
            case Theme.Light:
                foreach (Image img in footerButtonImages)
                {
                    if (img.gameObject == clickedObject)
                    {
                        foreach(Transform child in img.gameObject.transform)
                        {
                            child.GetComponent<Image>().color = Color.red;
                        }
                    }
                        
                    else
                    {
                        foreach (Transform child in img.gameObject.transform)
                        {
                            child.GetComponent<Image>().color = Color.white;
                        }
                    }
                }       
                break;
        }
    }
}
