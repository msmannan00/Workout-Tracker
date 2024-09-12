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
    public TMP_InputField reps;
    public TMP_Dropdown rir;
    public Toggle isComplete;
    public bool isWeight;
    public Image previousImage,dropDownArrow;
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
            timerText.transform.parent.gameObject.SetActive(false);
            weight.transform.parent.gameObject.SetActive(true);
            rir.transform.parent.gameObject.SetActive(true);
            reps.transform.parent.gameObject.SetActive(true);
            if(exerciseHistory!=null)
                previous.text = exerciseHistory.weight.ToString() + "kg " + exerciseHistory.reps.ToString();
        }
        else
        {
            timerText.transform.parent.gameObject.SetActive(true);
            weight.transform.parent.gameObject.SetActive(false);
            rir.transform.parent.gameObject.SetActive(false);
            reps.transform.parent.gameObject.SetActive(false);
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
        reps.onEndEdit.AddListener(OnREPSChanged);
        rir.onValueChanged.AddListener(OnRIRChanged);
        if(isComplete!=null)
            isComplete.onValueChanged.AddListener(OnToggleValueChange);
        timerText.onValueChanged.AddListener(OnTimerInput);
        UpdateToggleInteractableState();
        OnRIRChanged(0);
    }

    private void OnEnable()
    {
        TMP_FontAsset textFont = null;
        Color color = Color.white;
        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Dark:
                textFont = userSessionManager.Instance.darkTextFont;
                color = userSessionManager.Instance.darkBgColor;
                sets.font = textFont;
                sets.color = Color.white;
                previous.color = Color.white;
                previous.font = textFont;
                ChangeInputFieldFount_Color(timerText, textFont, Color.white);
                ChangeInputFieldFount_Color(weight, textFont, Color.white);
                ChangeInputFieldFount_Color(reps, textFont, Color.white);
                ChangeImageColorAndOutlineColor(timerText.gameObject, color, Color.white);
                ChangeImageColorAndOutlineColor(previousImage.gameObject, color, Color.white);
                ChangeImageColorAndOutlineColor(weight.gameObject, color, Color.white);
                ChangeImageColorAndOutlineColor(reps.gameObject, color, Color.white);
                ChangeImageColorAndOutlineColor(rir.gameObject, color, Color.white);
                ChangeImageColorAndOutlineColor(isComplete.targetGraphic.gameObject, color, Color.white);
                rir.captionText.color= Color.white;
                dropDownArrow.color= Color.white;
                //rir.itemText.color= Color.white;
                isComplete.graphic.color = Color.white;
                break;
            case Theme.Light:
                textFont = userSessionManager.Instance.lightTextFont;
                color = userSessionManager.Instance.darkBgColor;
                sets.font = textFont;
                sets.color = color;
                previous.color = color;
                previous.font = textFont;
                ChangeInputFieldFount_Color(timerText, textFont, color);
                ChangeInputFieldFount_Color(weight, textFont, color);
                ChangeInputFieldFount_Color(reps, textFont, color);
                Color outLine = userSessionManager.Instance.lightButtonColor;
                ChangeImageColorAndOutlineColor(timerText.gameObject, new Color32(246, 236, 220, 255), outLine);
                ChangeImageColorAndOutlineColor(previousImage.gameObject, new Color32(246, 236, 220, 255), outLine);
                ChangeImageColorAndOutlineColor(weight.gameObject, new Color32(246, 236, 220, 255), outLine);
                ChangeImageColorAndOutlineColor(reps.gameObject, new Color32(246, 236, 220, 255), outLine);
                ChangeImageColorAndOutlineColor(rir.gameObject, new Color32(246, 236, 220, 255), outLine);
                ChangeImageColorAndOutlineColor(isComplete.targetGraphic.gameObject, new Color32(246, 236, 220, 255), outLine);
                rir.captionText.color = color;
                rir.itemText.color = color;
                dropDownArrow.color= color;
                isComplete.graphic.color = outLine;
                break;
        }
    }
    void ChangeInputFieldFount_Color(TMP_InputField inputField,TMP_FontAsset fontAsset,Color color)
    {
        TextMeshProUGUI placeholder=inputField.placeholder as TextMeshProUGUI;
        TextMeshProUGUI text = inputField.textComponent as TextMeshProUGUI;
        placeholder.font = fontAsset;
        placeholder.color = color;
        text.font = fontAsset;
        text.color = color;
    }
    void ChangeImageColorAndOutlineColor(GameObject obj,Color imgColor,Color outlineColor)
    {
        obj.GetComponent<Image>().color = imgColor;
        obj.GetComponent<Outline>().effectColor= outlineColor;
    }
    private void InitializeRepsDropdown()
    {
        rir.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 1; i <= 10; i++)
        {
            options.Add(i.ToString());
        }
        rir.AddOptions(options);
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

    private void OnREPSChanged(string newLbs)
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

    private void OnRIRChanged(int newRepsIndex)
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
        exerciseModel.time = enteredTimeInSeconds;
        if (enteredTimeInSeconds > userEnteredTimeInSeconds)
        {
            // If user enters a greater time, restart the timer
            currentTimeInSeconds = enteredTimeInSeconds;
            userEnteredTimeInSeconds = enteredTimeInSeconds;
            if (isComplete != null)
            {
                isComplete.isOn = false;
                exerciseModel.toggle = false;
            }

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
            {
                isComplete.isOn = false;
                exerciseModel.toggle = false;
            }
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
        if (isComplete != null)
        {
            isComplete.isOn = true;
            exerciseModel.toggle = true;
        }
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
