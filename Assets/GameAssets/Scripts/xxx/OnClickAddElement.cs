using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickAddElement : MonoBehaviour
{
    public Button AddElementBtn;
    public GameObject ElementPrefabsRef;
    List<GameObject> AllElements = new List<GameObject>();
    private void Start()
    {
        AddElementBtn.onClick.AddListener(AddNewSetOnButton);
    }

    public void AddNewSetOnButton()
    {
        AllElements.Add(Instantiate(ElementPrefabsRef, this.transform));
        if(this.transform.parent.GetComponent<EditWorkoutTempleteExercise>() != null)
        {
            FindObjectOfType<EditWorkoutTemplete>().AddExerciseSet(this.transform.parent.GetComponent<EditWorkoutTempleteExercise>().exersisNameText.text);
        }
        if (this.transform.parent.GetComponent<WorkoutLogExercise>() != null)
        {
            FindObjectOfType<WorkoutLogController>().AddExerciseSet(this.transform.parent.GetComponent<WorkoutLogExercise>().exerciseNameText.text);
        }
    }
    public void AddSetOnStart()
    {
        AllElements.Add(Instantiate(ElementPrefabsRef, this.transform));
    }
}
