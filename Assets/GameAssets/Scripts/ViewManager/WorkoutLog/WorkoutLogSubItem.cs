using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutLogSubItem : MonoBehaviour, ItemController
{
    public TMP_Text sets;
    public TMP_Text previous;
    public TMP_InputField timerText;
    public TMP_InputField weight;
    public TMP_InputField lbs;
    public TMP_Dropdown reps;
    public Toggle isComplete;
    public bool isWeight;

    public ExerciseModel exerciseModel;

    //public bool timerReached; 
    private int currentTimeInSeconds; 
    private int userEnteredTimeInSeconds; 
    private float timer;
    private bool timerRunning = false;
    private Coroutine timerCoroutine;

    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        exerciseModel = (ExerciseModel)data["data"];
        isWeight=(bool)data["isWeight"];
        HistoryExerciseModel exerciseHistory = (HistoryExerciseModel)data["exerciseHistory"];
        sets.text = exerciseModel.setID.ToString();
        previous.text = exerciseModel.previous;
        if(isWeight)
        {
            timerText.gameObject.SetActive(false);
            weight.gameObject.SetActive(true);
            lbs.gameObject.SetActive(true);
            reps.gameObject.SetActive(true);
            if(exerciseHistory!=null)
                previous.text = exerciseHistory.weight.ToString() + "kg " + exerciseHistory.reps.ToString();
        }
        else
        {
            timerText.gameObject.SetActive(true);
            weight.gameObject.SetActive(false);
            lbs.gameObject.SetActive(false);
            reps.gameObject.SetActive(false);
            if (exerciseHistory != null)
            {
                int minutes = exerciseHistory.time / 60;
                int seconds = exerciseHistory.time % 60;

                previous.text = $"{minutes:D2}:{seconds:D2}";
            }
        }
        //if (exerciseModel.weight != 0)
        //{
        //    weight.text = exerciseModel.weight.ToString();
        //}
        //else
        //{
        //    weight.text = "";
        //}

        //if (exerciseModel.lbs != 0)
        //{
        //    lbs.text = exerciseModel.lbs.ToString();
        //}
        //else
        //{
        //    lbs.text = "";
        //}


        //if (exerciseModel.reps > 0)
        //{
        //    reps.value = exerciseModel.reps - 1;
        //}
        //else
        //{
        //    reps.value = 0;
        //}

        InitializeRepsDropdown();

        weight.onEndEdit.AddListener(OnWeightChanged);
        lbs.onEndEdit.AddListener(OnLbsChanged);
        reps.onValueChanged.AddListener(OnRepsChanged);
        if(isComplete!=null)
            isComplete.onValueChanged.AddListener(OnToggleValueChange);
        timerText.onValueChanged.AddListener(OnTimerInput);
        UpdateToggleInteractableState();
        OnRepsChanged(0);
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
        UpdateToggleInteractableState();
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
        UpdateToggleInteractableState();
    }
    private void OnTimerInput(string input)
    {
        // Remove non-numeric characters
        input = input.Replace(":", "").Trim();

        if (input.Length > 4) input = input.Substring(0, 4);

        // Format the input to "00:00"
        string formattedInput = input.PadLeft(4, '0');
        formattedInput = formattedInput.Insert(2, ":");

        timerText.text = formattedInput;

        // Convert formatted time to seconds
        string[] timeParts = formattedInput.Split(':');
        int minutes = int.Parse(timeParts[0]);
        int seconds = int.Parse(timeParts[1]);
        int enteredTimeInSeconds = (minutes * 60) + seconds;

        if (enteredTimeInSeconds > userEnteredTimeInSeconds)
        {
            // If user enters a greater time, restart the timer
            currentTimeInSeconds = enteredTimeInSeconds;
            userEnteredTimeInSeconds = enteredTimeInSeconds;
            if (isComplete != null)
                isComplete.isOn = false;

            // Stop the previous coroutine if it's running
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }

            // Start the timer coroutine again
            timerCoroutine = StartCoroutine(StartTimer());
        }
        else if (enteredTimeInSeconds == userEnteredTimeInSeconds)
        {
            // If the same time is re-entered, do nothing
            return;
        }
        else if (enteredTimeInSeconds < userEnteredTimeInSeconds)
        {
            // If a lesser time is entered, the bool stays the same
            userEnteredTimeInSeconds = enteredTimeInSeconds;
        }
        else if (enteredTimeInSeconds == 0)
        {
            if (isComplete != null)
                isComplete.isOn = false;
        }
    }
    private IEnumerator StartTimer()
    {
        float timer = 0;

        while (timer < userEnteredTimeInSeconds)
        {
            yield return new WaitForSeconds(1); // Wait for 1 second each loop
            timer++;
        }

        // When the timer reaches the user-entered time
        if(isComplete!=null)
            isComplete.isOn = true;
    }

    public void OnToggleValueChange(bool value)
    {
        if (exerciseModel.weight > 0 && exerciseModel.reps > 0)
        {
            exerciseModel.toggle=value;
        }
    }
    private void UpdateToggleInteractableState()
    {
        if (isComplete != null)
        {
            if (isWeight)
                isComplete.interactable = (exerciseModel.weight > 0 && exerciseModel.reps > 0);
            else
                isComplete.interactable = false;
        }
    }
}
