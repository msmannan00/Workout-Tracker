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


    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        if (data.TryGetValue("data", out object exerciseDataObj) && exerciseDataObj is ExerciseDataItem exerciseData)
        {
            exerciseNameText.text = userSessionManager.Instance.FormatStringAbc(exerciseData.exerciseName);
            categoryNameText.text = exerciseData.category;
            Sprite sp = null;
            if (exerciseData.exerciseName == "Pec deck")
                sp = Resources.Load<Sprite>("UIAssets/ExcerciseIcons/Chest fly (machine)-1");
            else
                sp = Resources.Load<Sprite>("UIAssets/ExcerciseIcons/" + exerciseData.exerciseName + "-1");
            if (sp != null)
                exerciseImage.sprite = sp;
        }
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                SetExerciseText(userSessionManager.Instance.lightPrimaryFontBold, userSessionManager.Instance.lightButtonTextColor);
                SetCategoryText(userSessionManager.Instance.lightPrimaryFontBold, userSessionManager.Instance.lightPlaceholder);
                break;
            case Theme.Dark:
                SetExerciseText(userSessionManager.Instance.darkPrimaryFont, Color.white);
                SetCategoryText(userSessionManager.Instance.lightPrimaryFontBold, new Color32(255, 255, 255, 150));
                break;
        }
    }
    public void SetExerciseText(TMP_FontAsset font, Color color)
    {
        if (exerciseNameText != null)
        {
            exerciseNameText.font = font;
            exerciseNameText.color = color;
        }
    }
    public void SetCategoryText(TMP_FontAsset font, Color color)
    {
        if (categoryNameText != null)
        {
            categoryNameText.font = font;
            categoryNameText.color = color;
        }
    }

}
