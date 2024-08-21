using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseItem : MonoBehaviour
{
    [SerializeField]
    private ExerciseData data;
    [SerializeField]
    private Text exerciseNameText;
    [SerializeField]
    private Text categoryNameText;
    [SerializeField]
    private Image exerciseImage;
    void Start()
    {
        exerciseNameText.text = data.exerciseName;
        categoryNameText.text = data.category;
        exerciseImage.sprite = data.icon;
    }
}
