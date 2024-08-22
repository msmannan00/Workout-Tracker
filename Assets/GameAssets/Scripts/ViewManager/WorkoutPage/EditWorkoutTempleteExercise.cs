using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditWorkoutTempleteExercise : MonoBehaviour
{
    public Text exersisNameText;

    public void RemoveExercise()
    {
        FindObjectOfType<EditWorkoutTemplete>().RemoveExerciseButton(exersisNameText.text);
        Destroy(this.gameObject);
    }
}
