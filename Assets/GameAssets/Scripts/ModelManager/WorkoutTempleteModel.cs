using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Set
{
    [SerializeField]
    public int sets;
}
public class WorkoutTempleteModel : MonoBehaviour
{
    public Text templeteNameText;
    public  int totalExercises;
    public string[] exerciseName;
    public string notes;
    public Set[] sets;
    public GameObject exercisePrefab;

    private void Awake()
    {
        if (!PreferenceManager.Instance.HasKey("TempleteName"))
        {
            PreferenceManager.Instance.SetString("TempleteName", templeteNameText.text);
        }
        templeteNameText.text = PreferenceManager.Instance.GetString("TempleteName");
        if (!PreferenceManager.Instance.HasKey("Exercises"))
        {
            PreferenceManager.Instance.SetInt("Exercises", totalExercises);
        }
        for(int i=0;i<totalExercises;++i)
        {
            if (!PreferenceManager.Instance.HasKey("Exercise"+i))
            {
                PreferenceManager.Instance.SetString("Exercise" + i, exerciseName[i]);
            }
            if (!PreferenceManager.Instance.HasKey("Exercise" + i+"set"))
            {
                PreferenceManager.Instance.SetInt("Exercise" + i + "set", sets[i].sets);
            }
            if (!PreferenceManager.Instance.HasKey("Exercise" + i + "note"))
            {
                PreferenceManager.Instance.SetString("Exercise" + i + "note", "");
            }
        }
        totalExercises= PreferenceManager.Instance.GetInt("Exercises");
        for (int i = 0; i < totalExercises; ++i)
        {
            GameObject obj = Instantiate(exercisePrefab, this.transform);
            obj.GetComponent<Text>().text= PreferenceManager.Instance.GetInt("Exercise" + i + "set").ToString()+" x " + PreferenceManager.Instance.GetString("Exercise" + i);
        }

    }
}
