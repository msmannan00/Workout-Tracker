using System.Collections.Generic;
using UnityEngine;

public class ExerciseController : MonoBehaviour,PageController
{
    public Transform content;

    public void onInit(Dictionary<string, object> data)
    {
        
    }

    void Start()
    {
        ExerciseData exerciseData = DataManager.Instance.getExerciseData();

        foreach (ExerciseDataItem exercise in exerciseData.exercises)
        {
            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/exercise/exerciseScreenDataModel");
            GameObject newExerciseObject = Instantiate(exercisePrefab, content);
            ExerciseItem newExerciseItem = newExerciseObject.GetComponent<ExerciseItem>();

            Dictionary<string, object> initData = new Dictionary<string, object>
            {
                { "data", exercise },
            };

            newExerciseItem.onInit(initData);

        }
    }

    public void Close()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("editWorkoutTemplete", gameObject, "editWorkoutTempleteScreen", mData, true);
        Destroy(this.gameObject);
    }

}
