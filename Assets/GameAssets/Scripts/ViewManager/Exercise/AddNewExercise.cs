using System;
using System.Collections;
using System.Collections.Generic;
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
    public Toggle isWeightExercise;
    public Image backImage, saveImage;
    public TextMeshProUGUI saveText;

    public ExerciseDataItem exerciseDataItem;
    private Action<ExerciseDataItem> callback;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        TMP_FontAsset headingFont = null;
        TMP_FontAsset textFont = null;
        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Light:
                headingFont = userSessionManager.Instance.lightHeadingFont;
                textFont = userSessionManager.Instance.lightTextFont;
                this.gameObject.GetComponent<Image>().color = userSessionManager.Instance.lightBgColor;
                foreach (TextMeshProUGUI text in labelHeading)
                {
                    text.color = userSessionManager.Instance.lightHeadingColor;
                    text.font = headingFont;
                }
                foreach (TextMeshProUGUI text in placeholderAndText)
                {
                    text.color = userSessionManager.Instance.lightTextColor;
                    text.font = textFont;
                }
                saveText.font = headingFont;
                saveText.color = Color.white;
                exerciseName.gameObject.GetComponent<Image>().color = Color.white;
                categoryName.gameObject.GetComponent<Image>().color = Color.white;
                backImage.color = userSessionManager.Instance.lightButtonColor;
                saveImage.color = userSessionManager.Instance.lightButtonColor;
                break;
            case Theme.Dark:
                headingFont = userSessionManager.Instance.darkHeadingFont;
                textFont = userSessionManager.Instance.darkTextFont;
                this.gameObject.GetComponent<Image>().color = userSessionManager.Instance.darkBgColor;
                foreach (TextMeshProUGUI text in labelHeading)
                {
                    text.color = Color.white;
                    text.font = headingFont;
                }
                foreach (TextMeshProUGUI text in placeholderAndText)
                {
                    text.color = userSessionManager.Instance.darkSearchIconColor;
                    text.font = textFont;
                }
                saveText.font = headingFont;
                saveText.color = new Color32(51, 23, 23, 255);
                exerciseName.gameObject.GetComponent<Image>().color = userSessionManager.Instance.darkSearchBarColor;
                categoryName.gameObject.GetComponent<Image>().color = userSessionManager.Instance.darkSearchBarColor;
                backImage.color = Color.white;
                saveImage.color = Color.white;

                break;
        }
    }
    private void Start()
    {
        exerciseDataItem = new ExerciseDataItem();

        // Subscribe to the onValueChanged events
        exerciseName.onValueChanged.AddListener(OnExerciseNameChanged);
        categoryName.onValueChanged.AddListener(OnCategoryNameChanged);
        rank.onValueChanged.AddListener(OnRankChanged);
        isWeightExercise.onValueChanged.AddListener(OnIsWeightExerciseChanged);
    }

    private void OnExerciseNameChanged(string value)
    {
        exerciseDataItem.exerciseName = value;
    }

    private void OnCategoryNameChanged(string value)
    {
        exerciseDataItem.category = value;
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
        if (!string.IsNullOrEmpty(exerciseName.text) && !string.IsNullOrEmpty(categoryName.text))
        {
            DataManager.Instance.SaveData(exerciseDataItem);
            callback.Invoke(exerciseDataItem);
            OnClose();
        }
    }
    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }
}
