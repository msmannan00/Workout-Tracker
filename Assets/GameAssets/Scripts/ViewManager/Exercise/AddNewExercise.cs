using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddNewExercise : MonoBehaviour, PageController
{
    public List<TextMeshProUGUI> labelHeading;
    public List<TextMeshProUGUI> placeholderAndText = new List<TextMeshProUGUI>();
    public TMP_InputField exerciseName;
    public TMP_InputField categoryName;
    public TMP_InputField rank;
    public TMP_Dropdown categoryDropdown;
    public TMP_Dropdown exerciseTypeDropdown;
    public Toggle isWeightExercise;
    public Image backImage, saveImage;
    public TextMeshProUGUI saveText;
    public GameObject messageObj;
    public Button saveButton, backButton;

    public ExerciseDataItem exerciseDataItem = new ExerciseDataItem();
    private Action<ExerciseDataItem> callback;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        
        List<string> categorys = GetUniqueCategorys(ApiDataHandler.Instance.getExerciseData());
        InitializeCategoryDropdown(categorys);
        InitializeExerciseTypeDropdown(new List<string>(Enum.GetNames(typeof(ExerciseType))));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClose();
        }
    }
    private void Start()
    {
        // Subscribe to the onValueChanged events
        exerciseName.onValueChanged.AddListener(OnExerciseNameChanged);
        //categoryName.onValueChanged.AddListener(OnCategoryNameChanged);
        categoryDropdown.onValueChanged.AddListener(OnCategoryValueChanged);
        rank.onValueChanged.AddListener(OnRankChanged);
        exerciseTypeDropdown.onValueChanged.AddListener(OnExerciseTypeValueChanged);
        isWeightExercise.onValueChanged.AddListener(OnIsWeightExerciseChanged);
        saveButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }

    private void OnExerciseNameChanged(string value)
    {
        exerciseDataItem.exerciseName = value;
    }

    //private void OnCategoryNameChanged(string value)
    //{
    //    exerciseDataItem.category = value;
    //}
    private void OnCategoryValueChanged(int category)
    {
        AudioController.Instance.OnButtonClick();
        exerciseDataItem.category = categoryDropdown.options[category].text;
    }
    private void OnExerciseTypeValueChanged(int category)
    {
        AudioController.Instance.OnButtonClick();
        exerciseDataItem.exerciseType = (ExerciseType)(category + 1);
    }
    private void InitializeCategoryDropdown(List<string> categorys)
    {
        categoryDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (string category in categorys)
        {
            options.Add(category);
        }
        categoryDropdown.AddOptions(options);
        OnCategoryValueChanged(0);
    }
    private void InitializeExerciseTypeDropdown(List<string> exerciseTypes)
    {
        exerciseTypeDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (string type in exerciseTypes)
        {
            options.Add(type);
        }
        exerciseTypeDropdown.AddOptions(options);
        OnExerciseTypeValueChanged(0);
    }
    private void OnRankChanged(string value)
    {
        // Try to parse the rank string to an int
        if (int.TryParse(value, out int result))
        {
            exerciseDataItem.rank = result;
        }
        else
        {
            Debug.LogWarning("Invalid rank value. Please enter a valid integer.");
        }
    }

    private void OnIsWeightExerciseChanged(bool value)
    {
        //exerciseDataItem.isWeightExercise = value;
    }

    public void Save()
    {
        if (!string.IsNullOrEmpty(exerciseDataItem.exerciseName))
        {
            if (!IsExerciseNamePresent(exerciseDataItem.exerciseName))
            {
                ApiDataHandler.Instance.SaveExerciseData(exerciseDataItem);
                callback.Invoke(exerciseDataItem);
                OnClose();
            }
            else
            {
                messageObj.GetComponent<TextMeshProUGUI>().text = "Exercise already exists.";
                messageObj.SetActive(true);
                CancelInvoke("OffMessageObject");
                Invoke("OffMessageObject", 1.5f);
            }

        }
        else
        {
            messageObj.GetComponent<TextMeshProUGUI>().text = "Please! enter exercise name.";
            messageObj.SetActive(true);
            CancelInvoke("OffMessageObject");
            Invoke("OffMessageObject", 1.5f);
        }
    }
    void OffMessageObject()
    {
        messageObj.SetActive(false);
    }
    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }
    public bool IsExerciseNamePresent(string nameToCheck)
    {
        // Loop through the exercises and check if any exerciseName matches the input string
        foreach (var exercise in ApiDataHandler.Instance.getExerciseData().exercises)
        {
            if (exercise.exerciseName.ToLower() == nameToCheck.ToLower())
            {
                return true; // Return true if a match is found
            }
        }

        return false; // Return false if no match is found
    }
    public List<string> GetUniqueCategorys(ExerciseData excerciseData)
    {
        // Create a HashSet to store unique exercise names
        HashSet<string> uniqueExercises = new HashSet<string>();

        // Iterate over each HistoryTempleteModel in the historyData
        foreach (var template in excerciseData.exercises)
        {
            uniqueExercises.Add(template.category);
        }

        // Convert HashSet to List and return it
        return uniqueExercises.ToList();
    }
}
