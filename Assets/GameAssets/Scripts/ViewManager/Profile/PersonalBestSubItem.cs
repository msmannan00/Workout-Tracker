using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PersonalBestSubItem : MonoBehaviour,ItemController
{
    public TextMeshProUGUI exerciseName;
    public TMP_InputField weight;
    PersonalBestDataItem _data;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        _data = (PersonalBestDataItem)data["data"];
        exerciseName.text = _data.exerciseName;
        weight.text = _data.weight.ToString() + " kg";
        weight.onEndEdit.AddListener(WeightValueChange);
    }
    void WeightValueChange(string value)
    {
        string numericValue = Regex.Replace(value, @"\D", "");

        if (int.TryParse(numericValue, out int parsedWeight))
        {
            weight.text = parsedWeight.ToString() + " kg";
            _data.weight = parsedWeight; // Update weight only if parsing succeeds
        }
        else
        {
            // Optional: Handle invalid input case, like clearing or showing a warning
            Debug.Log("Invalid input: Please enter a valid integer.");
        }
    }
}
