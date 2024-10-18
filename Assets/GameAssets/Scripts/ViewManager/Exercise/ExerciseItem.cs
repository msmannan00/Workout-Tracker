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
            
        }
    }

}
