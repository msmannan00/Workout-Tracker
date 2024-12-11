using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PersonalBestSubItem : MonoBehaviour,ItemController
{
    public TextMeshProUGUI exerciseName;
    public TMP_InputField weight;
    public TMP_InputField rep;
    public PersonalBestDataItem _data;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        _data = (PersonalBestDataItem)data["data"];
        exerciseName.text = userSessionManager.Instance.FormatStringAbc(_data.exerciseName);
        rep.text = _data.rep.ToString();
        weight.text = _data.weight.ToString() + " kg";
        weight.onEndEdit.AddListener(WeightValueChange);
        rep.onEndEdit.AddListener(RepValueChange);
        if (new[] { "bench press (barbell)", "squat (barbell)", "deadlifts (barbell)" }.Contains(_data.exerciseName.ToLower()))
        {
            transform.SetAsFirstSibling();
        }
    }
    void RepValueChange(string value)
    {
        if (int.TryParse(value, out int parsedWeight))
        {
            rep.text = parsedWeight.ToString();
            _data.rep = parsedWeight; // Update weight only if parsing succeeds
        }
        ApiDataHandler.Instance.SavePersonalBestData();
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
        ApiDataHandler.Instance.SavePersonalBestData();
        userSessionManager.Instance.CheckAchievementStatus();
        //ApiDataHandler.Instance.SaveAchievementData();
    }
}
