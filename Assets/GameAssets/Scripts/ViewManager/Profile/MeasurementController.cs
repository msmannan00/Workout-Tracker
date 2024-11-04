using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeasurementController : MonoBehaviour,PageController
{
    public Button backButton;

    public TMP_InputField weight;
    public TMP_InputField bodyFat;
    public TMP_InputField chest;
    public TMP_InputField shoulder;
    public TMP_InputField hips;
    public TMP_InputField waist;
    public TMP_InputField leftThigh;
    public TMP_InputField rightThigh;
    public TMP_InputField leftBicep;
    public TMP_InputField rightBicep;
    public TMP_InputField leftForearm;
    public TMP_InputField rightForearm;
    public TMP_InputField leftCalf;
    public TMP_InputField rightCalf;

    //private MeasurementModel measurementModel;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        InitializeInputFields();
        AddListeners();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    private void Start()
    {
        backButton.onClick.AddListener(Back);
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    void AddListeners()
    {
        weight.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().weight, "kg"));
        bodyFat.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().bodyFat, "%"));
        chest.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().chest, "cm"));
        shoulder.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().shoulder, "cm"));
        hips.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().hips, "cm"));
        waist.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().waist, "cm"));
        leftThigh.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().leftThigh, "cm"));
        rightThigh.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().rightThigh, "cm"));
        leftBicep.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().leftBicep, "cm"));
        rightBicep.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().rightBicep, "cm"));
        leftForearm.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().leftForearm, "cm"));
        rightForearm.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().rightForearm, "cm"));
        leftCalf.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().leftCalf, "cm"));
        rightCalf.onEndEdit.AddListener(value => OnInputEditEnd(value, targetField: ref ApiDataHandler.Instance.getMeasurementData().rightCalf, "cm"));
    }

    // Initialize input fields with values from the MeasurementModel and add units
    void InitializeInputFields()
    {
        weight.text = ApiDataHandler.Instance.getMeasurementData().weight + " kg";
        bodyFat.text = ApiDataHandler.Instance.getMeasurementData().bodyFat + " %";
        chest.text = ApiDataHandler.Instance.getMeasurementData().chest + " cm";
        shoulder.text = ApiDataHandler.Instance.getMeasurementData().shoulder + " cm";
        hips.text = ApiDataHandler.Instance.getMeasurementData().hips + " cm";
        waist.text = ApiDataHandler.Instance.getMeasurementData().waist + " cm";
        leftThigh.text = ApiDataHandler.Instance.getMeasurementData().leftThigh + " cm";
        rightThigh.text = ApiDataHandler.Instance.getMeasurementData().rightThigh + " cm";
        leftBicep.text = ApiDataHandler.Instance.getMeasurementData().leftBicep + " cm";
        rightBicep.text = ApiDataHandler.Instance.getMeasurementData().rightBicep + " cm";
        leftForearm.text = ApiDataHandler.Instance.getMeasurementData().leftForearm + " cm";
        rightForearm.text = ApiDataHandler.Instance.getMeasurementData().rightForearm + " cm";
        leftCalf.text = ApiDataHandler.Instance.getMeasurementData().leftCalf + " cm";
        rightCalf.text = ApiDataHandler.Instance.getMeasurementData().rightCalf + " cm";
    }

    // Generic function to handle input editing and update the MeasurementModel
    public void OnInputEditEnd(string value, ref int targetField, string unit)
    {
        // Remove the unit from the input string before parsing
        string cleanedValue = value.Replace(unit, "").Trim();

        if (int.TryParse(cleanedValue, out int result))
        {
            targetField = result;  // Update the corresponding field in the MeasurementModel
        }
        else
        {
            Debug.LogWarning($"Invalid input: {value}");
        }

        // Update the input field to reflect the value with the unit
        UpdateInputFieldWithUnit(targetField, unit, value);
    }

    // Helper method to update the input field text with the value and unit
    private void UpdateInputFieldWithUnit(int value, string unit, string originalValue)
    {
        // Check which input field needs to be updated by comparing the original text value
        if (originalValue.Contains("kg")) weight.text = value + " kg";
        else if (originalValue.Contains("%")) bodyFat.text = value + " %";
        else if (originalValue.Contains("cm"))
        {
            if (chest.text.Contains(originalValue)) chest.text = value + " cm";
            else if (shoulder.text.Contains(originalValue)) shoulder.text = value + " cm";
            else if (hips.text.Contains(originalValue)) hips.text = value + " cm";
            else if (waist.text.Contains(originalValue)) waist.text = value + " cm";
            else if (leftThigh.text.Contains(originalValue)) leftThigh.text = value + " cm";
            else if (rightThigh.text.Contains(originalValue)) rightThigh.text = value + " cm";
            else if (leftBicep.text.Contains(originalValue)) leftBicep.text = value + " cm";
            else if (rightBicep.text.Contains(originalValue)) rightBicep.text = value + " cm";
            else if (leftForearm.text.Contains(originalValue)) leftForearm.text = value + " cm";
            else if (rightForearm.text.Contains(originalValue)) rightForearm.text = value + " cm";
            else if (leftCalf.text.Contains(originalValue)) leftCalf.text = value + " cm";
            else if (rightCalf.text.Contains(originalValue)) rightCalf.text = value + " cm";
        }
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null, null, false);
        ApiDataHandler.Instance.LoadMeasurementData();
    }

}
