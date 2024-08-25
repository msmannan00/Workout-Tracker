using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkoutLogSubItem : MonoBehaviour, ItemController
{
    public TMP_Text sets;
    public TMP_Text previous;
    public TMP_InputField weight;
    public TMP_InputField lbs;
    public TMP_Dropdown reps;

    private ExerciseModel exerciseModel;

    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        exerciseModel = (ExerciseModel)data["data"];

        sets.text = exerciseModel.setID.ToString();
        previous.text = exerciseModel.previous;

        if (exerciseModel.weight != 0)
        {
            weight.text = exerciseModel.weight.ToString();
        }
        else
        {
            weight.text = "";
        }

        if (exerciseModel.lbs != 0)
        {
            lbs.text = exerciseModel.lbs.ToString();
        }
        else
        {
            lbs.text = "";
        }

        InitializeRepsDropdown();

        if (exerciseModel.reps > 0)
        {
            reps.value = exerciseModel.reps - 1;
        }
        else
        {
            reps.value = 0;
        }

        weight.onEndEdit.AddListener(OnWeightChanged);
        lbs.onEndEdit.AddListener(OnLbsChanged);
        reps.onValueChanged.AddListener(OnRepsChanged);
    }

    private void InitializeRepsDropdown()
    {
        reps.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 1; i <= 10; i++)
        {
            options.Add(i.ToString());
        }
        reps.AddOptions(options);
    }

    private void OnWeightChanged(string newWeight)
    {
        if (int.TryParse(newWeight, out int weightValue))
        {
            exerciseModel.weight = weightValue;
        }
        else
        {
            exerciseModel.weight = 0;
        }
    }

    private void OnLbsChanged(string newLbs)
    {
        if (int.TryParse(newLbs, out int lbsValue))
        {
            exerciseModel.lbs = lbsValue;
        }
        else
        {
            exerciseModel.lbs = 0;
        }
    }

    private void OnRepsChanged(int newRepsIndex)
    {
        exerciseModel.reps = newRepsIndex + 1;
    }
}
