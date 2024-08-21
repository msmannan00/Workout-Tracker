using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditWorkoutTemplete : MonoBehaviour,PageController
{
    public GameObject exerciseDetailPrefab;
    public Transform exerciseParent;

    int addedExercises;

    public void onInit(Dictionary<string, object> data)
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        addedExercises = PreferenceManager.Instance.GetInt("Exercises");
        for(int i = 0; i < addedExercises; i++)
        {
            GameObject obj=Instantiate(exerciseDetailPrefab,exerciseParent);
            obj.GetComponent<EditWorkoutTempleteExercise>().exersisNameText.text = PreferenceManager.Instance.GetString("Exercise" + i);
            int sets = PreferenceManager.Instance.GetInt("Exercise" + i + "set");
            print(sets);
            for (int j = 1; j < sets; j++)
            {
                obj.GetComponentInChildren<OnClickAddElement>().AddNewSet();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
