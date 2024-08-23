using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkoutTempleteModel : MonoBehaviour, IPointerClickHandler
{
    public string _templeteName;
    public List<Exercise> exercises=new List<Exercise>();


    public TMP_Text templeteNameText;
    public int totalExercises;
    public string[] exerciseName;
    public string notes;
    public Set[] sets;
    public GameObject exercisePrefab;

    private void Awake()
    {
        //templeteNameText.text= templeteName;
        //for(int i = 0; i < exercises.Count; i++)
        //{
        //    if (!PreferenceManager.Instance.HasKey(templeteName+"TotalExercises"))
        //    {
        //        PreferenceManager.Instance.SetInt(templeteName + "TotalExercises", exercises.Count);
        //    }

        //    totalExercises = PreferenceManager.Instance.GetInt(templeteName + "Exercises");
        //    for (int j = 0; i < totalExercises; ++j)
        //    {
        //        if (!PreferenceManager.Instance.HasKey(templeteName + "Exercise" + j + "name"))
        //        {
        //            PreferenceManager.Instance.SetString(templeteName + "Exercise" + j + "name", exercises[i].name);
        //        }
        //        if (!PreferenceManager.Instance.HasKey(templeteName + "Exercise" + i + "set"))
        //        {
        //            PreferenceManager.Instance.SetInt(templeteName + "Exercise" + i + "set", exercises[i].sets);
        //        }
        //        if (!PreferenceManager.Instance.HasKey(templeteName + "Exercise" + i + "note"))
        //        {
        //            PreferenceManager.Instance.SetString(templeteName + "Exercise" + i + "note", exercises[i].note);
        //        }
        //    }
           
        //}
        //totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
        //for (int i = 0; i < totalExercises; ++i)
        //{
        //    GameObject obj = Instantiate(exercisePrefab, this.transform);
        //    obj.GetComponent<Text>().text = PreferenceManager.Instance.GetInt(templeteName + "Exercise" + i + "set").ToString() + " x " + PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "name");
        //}

        //////////////////////////////////////////////////////////////////////////////////////
        ///

        //if (!PreferenceManager.Instance.HasKey("TempleteName"))
        //{
        //    PreferenceManager.Instance.SetString("TempleteName", templeteNameText.text);
        //}
        //templeteNameText.text = PreferenceManager.Instance.GetString("TempleteName");
        //if (!PreferenceManager.Instance.HasKey("Exercises"))
        //{
        //    PreferenceManager.Instance.SetInt("Exercises", totalExercises);
        //}
        //for(int i=0;i<totalExercises;++i)
        //{
        //    if (!PreferenceManager.Instance.HasKey("Exercise"+i))
        //    {
        //        PreferenceManager.Instance.SetString("Exercise" + i, exerciseName[i]);
        //    }
        //    if (!PreferenceManager.Instance.HasKey("Exercise" + i+"set"))
        //    {
        //        PreferenceManager.Instance.SetInt("Exercise" + i + "set", sets[i].sets);
        //    }
        //    if (!PreferenceManager.Instance.HasKey("Exercise" + i + "note"))
        //    {
        //        PreferenceManager.Instance.SetString("Exercise" + i + "note", "");
        //    }
        //}
        //totalExercises= PreferenceManager.Instance.GetInt("Exercises");
        //for (int i = 0; i < totalExercises; ++i)
        //{
        //    GameObject obj = Instantiate(exercisePrefab, this.transform);
        //    obj.GetComponent<Text>().text= PreferenceManager.Instance.GetInt("Exercise" + i + "set").ToString()+" x " + PreferenceManager.Instance.GetString("Exercise" + i);
        //}

    }
    public void SetTempleteValuesFirstTime(DefaultTemplete dt, int index)
    {
        templeteNameText.text = dt.templeteName;
        if (!PreferenceManager.Instance.HasKey("Templete" + index))
        {
            PreferenceManager.Instance.SetString("Templete" + index, dt.templeteName);
        }
        dt.templeteName = PreferenceManager.Instance.GetString("Templete" + index);
        //for (int i = 0; i < dt.exerciseTemplete.Count; i++)
        //{
            if (!PreferenceManager.Instance.HasKey(dt.templeteName + "TotalExercises"))
            {
                PreferenceManager.Instance.SetInt(dt.templeteName + "TotalExercises", dt.exerciseTemplete.Count);
            }

            totalExercises = PreferenceManager.Instance.GetInt(dt.templeteName + "TotalExercises");
            for (int j = 0; j < totalExercises; ++j)
            {
                if (!PreferenceManager.Instance.HasKey(dt.templeteName + "Exercise" + j + "name"))
                {
                    PreferenceManager.Instance.SetString(dt.templeteName + "Exercise" + j + "name", dt.exerciseTemplete[j].name);
                }
                if (!PreferenceManager.Instance.HasKey(dt.templeteName + "Exercise" + j + "set"))
                {
                    PreferenceManager.Instance.SetInt(dt.templeteName + "Exercise" + j + "set", dt.exerciseTemplete[j].sets);
                }
                if (!PreferenceManager.Instance.HasKey(dt.templeteName + "Exercise" + j + "note"))
                {
                    PreferenceManager.Instance.SetString(dt.templeteName + "Exercise" + j + "note", dt.exerciseTemplete[j].note);
                }
            }

        //}
        totalExercises = PreferenceManager.Instance.GetInt(dt.templeteName + "TotalExercises");
        for (int i = 0; i < totalExercises; ++i)
        {
            GameObject obj = Instantiate(exercisePrefab, this.transform);
            obj.transform.SetAsLastSibling();
            obj.GetComponent<Text>().text = PreferenceManager.Instance.GetInt(dt.templeteName + "Exercise" + i + "set").ToString() + " x " + PreferenceManager.Instance.GetString(dt.templeteName + "Exercise" + i + "name");
        }
    }
    public void SetTempleteValues(int index)
    {
        string templeteName = PreferenceManager.Instance.GetString("Templete" + index);
        templeteNameText.text = templeteName;
        
        totalExercises = PreferenceManager.Instance.GetInt(templeteName + "TotalExercises");
        for (int i = 0; i < totalExercises; ++i)
        {
            GameObject obj = Instantiate(exercisePrefab, this.transform);
            obj.transform.SetAsLastSibling();
            obj.GetComponent<TMP_Text>().text = PreferenceManager.Instance.GetInt(templeteName + "Exercise" + i + "set").ToString() + " x " + PreferenceManager.Instance.GetString(templeteName + "Exercise" + i + "name");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<WorkoutPageController>().SelectWorkout(this.gameObject);

    }
}
