using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeasurementHistorySubItem : MonoBehaviour,ItemController
{
    public TextMeshProUGUI dateTimeText;
    public TextMeshProUGUI measurementText;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        string name = (string)data["name"];
        float value = (float)data["value"];
        string dateTime = (string)data["dateTime"];
        DateTime parsedDateTime = DateTime.ParseExact(dateTime, "MMM dd, yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

        // Determine if the year should be displayed
        if (parsedDateTime.Year == DateTime.Now.Year)
        {
            dateTimeText.text = parsedDateTime.ToString("MMM dd  hh:mm tt");
        }
        else
        {
            dateTimeText.text = parsedDateTime.ToString("MMM dd, yyyy  hh:mm tt");
        }
        switch(name.ToLower()) 
        {
            case "weight":
                switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
                {
                    case WeightUnit.kg:
                        measurementText.text = value.ToString() + " kg";
                        break;
                    case WeightUnit.lbs:
                        measurementText.text= userSessionManager.Instance.ConvertKgToLbs(value).ToString() + " lbs";
                        break;
                }
                break;
            case "body fat":
                measurementText.text = value.ToString() + " %";
                break;
            default:
                measurementText.text = value.ToString() + " cm";
                break;
        }
    }

    
}
