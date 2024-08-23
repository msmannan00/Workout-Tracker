using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EditWorkoutTemplete;

public class ExerciseItem : MonoBehaviour, IPointerClickHandler
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
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PreferenceManager.Instance.GetBool("EmptyWorkout"))
        {
            FindObjectOfType<ExerciseController>().AddExerciseToNewTemplete(data.exerciseName);
        }
        else 
        {
            FindObjectOfType<ExerciseController>().AddExerciseToDefaultTemplete(data.exerciseName);
            
        }

        
        
    }
}
