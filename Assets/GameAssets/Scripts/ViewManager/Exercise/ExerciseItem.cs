using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExerciseItem : MonoBehaviour, IPointerClickHandler, ItemController
{
    [SerializeField]
    private TextMeshProUGUI exerciseNameText;
    [SerializeField]
    private TextMeshProUGUI categoryNameText;
    [SerializeField]
    private Image exerciseImage;
    [SerializeField]
    public GameObject selected;

    void Start()
    {
    }


    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        if (data.TryGetValue("data", out object exerciseDataObj) && exerciseDataObj is ExerciseDataItem exerciseData)
        {
            exerciseNameText.text = exerciseData.exerciseName;
            categoryNameText.text = exerciseData.category;
            switch (ApiDataHandler.Instance.gameTheme)
            {
                case Theme.Light:
                    exerciseNameText.font = userSessionManager.Instance.lightHeadingFont;
                    categoryNameText.font=userSessionManager.Instance.lightTextFont;
                    exerciseNameText.color = userSessionManager.Instance.lightHeadingColor;
                    categoryNameText.color=userSessionManager.Instance.lightTextColor;
                    break;
                case Theme.Dark:
                    exerciseNameText.font = userSessionManager.Instance.darkHeadingFont;
                    categoryNameText.font = userSessionManager.Instance.darkTextFont;
                    exerciseNameText.color = Color.white;
                    categoryNameText.color = new Color32(255, 255, 255, 153);
                    break;
            }
        }
    }

}
