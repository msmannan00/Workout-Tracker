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
    public TMP_InputField mile;
    public TMP_InputField timerText;
    public TMP_InputField weight;
    public TMP_InputField reps;
    public TMP_Dropdown rir;
    public Toggle isComplete;
    public ExerciseType exerciseType;
    public Image previousImage,dropDownArrow;
    public ExerciseModel exerciseModel;

    //public bool timerReached; 
    private int currentTimeInSeconds; 
    private int userEnteredTimeInSeconds; 
    private float timer;
    private bool timerRunning = false;
    private Coroutine timerCoroutine;
    private Action<object> callBack;
    bool shake = true;
    bool isWorkoutLog;

    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        this.callBack = callback;
        exerciseModel = (ExerciseModel)data["data"];
        exerciseType = (ExerciseType)data["exerciseType"];
        HistoryExerciseModel exerciseHistory = (HistoryExerciseModel)data["exerciseHistory"];
        isWorkoutLog = (bool)data["isWorkoutLog"];
        //InputFieldManager inputFieldManager = (InputFieldManager)data["inputManager"];
        //inputFieldManager.inputFields.Add(mile);
        //inputFieldManager.inputFields.Add(timerText);
        //inputFieldManager.inputFields.Add(weight);
        //inputFieldManager.inputFields.Add(reps);
        ResetModel(exerciseModel);
        sets.text = exerciseModel.setID.ToString();
        previous.text = exerciseModel.previous;
        switch (exerciseType)
        {
            case ExerciseType.RepsOnly:
                reps.transform.parent.gameObject.SetActive(true);
                if (!isWorkoutLog)
                    OffAllInteractables();
                break;
            case ExerciseType.TimeBased:
                timerText.transform.parent.gameObject.SetActive(true);
                if (exerciseHistory != null && isWorkoutLog)
                {
                    int minutes = exerciseHistory.time / 60;
                    int seconds = exerciseHistory.time % 60;

                    previous.text = $"{minutes:D2}:{seconds:D2}";
                }
                if (!isWorkoutLog)
                    OffAllInteractables();
                break;
            case ExerciseType.TimeAndMiles:
                timerText.transform.parent.gameObject.SetActive(true);
                mile.transform.parent.gameObject.SetActive(true);
                if (!isWorkoutLog)
                    OffAllInteractables();
                //print("Need to implement Time and Miles");
                break;
            case ExerciseType.WeightAndReps:
                timerText.transform.parent.gameObject.SetActive(false);
                weight.transform.parent.gameObject.SetActive(true);
                rir.transform.parent.gameObject.SetActive(true);
                reps.transform.parent.gameObject.SetActive(true);
                if (exerciseHistory != null && isWorkoutLog)
                {
                    switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
                    {
                        case WeightUnit.kg:
                            previous.text = exerciseHistory.weight.ToString() + "kg " + "x " + exerciseHistory.reps.ToString();
                            break;
                        case WeightUnit.lbs:
                            previous.text = (userSessionManager.Instance.ConvertKgToLbs(exerciseHistory.weight)).ToString() + "lbs " + "x " + exerciseHistory.reps.ToString("F2");
                            break;
                    }
                    
                }
                if (!isWorkoutLog)
                    OffAllInteractables();
                break;
        }

        InitializeRirDropdown();

        weight.onEndEdit.AddListener(OnWeightChanged);
        rir.onValueChanged.AddListener(OnRIRChanged);
        reps.onEndEdit.AddListener(OnREPSChanged);
        timerText.onValueChanged.AddListener(OnTimerInput);
        mile.onValueChanged.AddListener(OnMileChanges);
        if(isComplete!=null)
            isComplete.onValueChanged.AddListener(OnToggleValueChange);
        UpdateToggleInteractableState();
        OnRIRChanged(0);
    }
    void OffAllInteractables()
    {
        weight.interactable = false;
        rir.interactable= false;
        reps.interactable= false;
        timerText.interactable= false;
        mile.interactable= false;
        isComplete.interactable= false;
    }
    void ResetModel(ExerciseModel model)
    {
        model.reps = 0;
        model.weight = 0;
        model.time = 0;
        model.mile= 0;
        model.toggle = false;
    }
    private void OnEnable()
    {
        //TMP_FontAsset textFont = null;
        //Color color = Color.white;
        //switch (userSessionManager.Instance.gameTheme)
        //{
        //    case Theme.Dark:
        //        textFont = userSessionManager.Instance.darkTextFont;
        //        color = userSessionManager.Instance.darkBgColor;
        //        sets.font = textFont;
        //        sets.color = Color.white;
        //        previous.color = Color.white;
        //        previous.font = textFont;
        //        ChangeInputFieldFount_Color(timerText, textFont, Color.white);
        //        ChangeInputFieldFount_Color(weight, textFont, Color.white);
        //        ChangeInputFieldFount_Color(reps, textFont, Color.white);
        //        ChangeImageColorAndOutlineColor(timerText.gameObject, color, Color.white);
        //        ChangeImageColorAndOutlineColor(previousImage.gameObject, color, Color.white);
        //        ChangeImageColorAndOutlineColor(weight.gameObject, color, Color.white);
        //        ChangeImageColorAndOutlineColor(reps.gameObject, color, Color.white);
        //        ChangeImageColorAndOutlineColor(rir.gameObject, color, Color.white);
        //        ChangeImageColorAndOutlineColor(isComplete.targetGraphic.gameObject, color, Color.white);
        //        rir.captionText.color= Color.white;
        //        dropDownArrow.color= Color.white;
        //        //rir.itemText.color= Color.white;
        //        isComplete.graphic.color = Color.white;
        //        break;
        //    case Theme.Light:
        //        textFont = userSessionManager.Instance.lightTextFont;
        //        color = userSessionManager.Instance.darkBgColor;
        //        sets.font = textFont;
        //        sets.color = color;
        //        previous.color = color;
        //        previous.font = textFont;
        //        ChangeInputFieldFount_Color(timerText, textFont, color);
        //        ChangeInputFieldFount_Color(weight, textFont, color);
        //        ChangeInputFieldFount_Color(reps, textFont, color);
        //        Color outLine = userSessionManager.Instance.lightButtonColor;
        //        ChangeImageColorAndOutlineColor(timerText.gameObject, new Color32(246, 236, 220, 255), outLine);
        //        ChangeImageColorAndOutlineColor(previousImage.gameObject, new Color32(246, 236, 220, 255), outLine);
        //        ChangeImageColorAndOutlineColor(weight.gameObject, new Color32(246, 236, 220, 255), outLine);
        //        ChangeImageColorAndOutlineColor(reps.gameObject, new Color32(246, 236, 220, 255), outLine);
        //        ChangeImageColorAndOutlineColor(rir.gameObject, new Color32(246, 236, 220, 255), outLine);
        //        ChangeImageColorAndOutlineColor(isComplete.targetGraphic.gameObject, new Color32(246, 236, 220, 255), outLine);
        //        rir.captionText.color = color;
        //        rir.itemText.color = color;
        //        dropDownArrow.color= color;
        //        isComplete.graphic.color = outLine;
        //        break;
        //}
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
    private void InitializeRirDropdown()
    {
        rir.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i <= 5; i++)
        {
            options.Add(i.ToString());
        }
        rir.AddOptions(options);
    }

    private void OnMileChanges(string newMile)
    {
        if (float.TryParse(newMile, out float mileValue))
        {
            exerciseModel.mile = mileValue;
        }
        else
        {
            exerciseModel.weight = 0;
        }
        UpdateToggleInteractableState();
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
            exerciseModel.reps = lbsValue;
        }
        else
        {
            exerciseModel.reps = 0;
        }
        UpdateToggleInteractableState();
    }

    private void OnRIRChanged(int newRepsIndex)
    {
        exerciseModel.rir = newRepsIndex;
        UpdateToggleInteractableState();
    }
    private void OnTimerInput(string input)
    {
        print(input);
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
            //if (isComplete != null)
            //{
            //    isComplete.isOn = false;
            //    exerciseModel.toggle = false;
            //    isComplete.targetGraphic.color = new Color32(81, 14, 14, 255);
            //}

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
                isComplete.targetGraphic.color = new Color32(81, 14, 14, 255);
            }
        }
        UpdateToggleInteractableState();
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
        //if (isComplete != null)
        //{
        //    isComplete.isOn = true;
        //    exerciseModel.toggle = true;
        //    isComplete.targetGraphic.color = new Color32(255, 182, 193, 255);

        //}
    }

    public void OnToggleValueChange(bool value)
    {
        //if (exerciseModel.weight > 0 && exerciseModel.reps > 0)
        //{
            exerciseModel.toggle=value;
        this.callBack?.Invoke(value);
            if (value)
            {
            AudioController.Instance.OnSetComplete();
                isComplete.targetGraphic.color = new Color32(255, 182, 193, 255);
            }
            else
            {
                isComplete.targetGraphic.color = new Color32(246, 236, 220, 255);
            }
        //}
    }
    private void UpdateToggleInteractableState()
    {
        if (isComplete != null)
        {
            switch (exerciseType)
            {
                case ExerciseType.RepsOnly:
                    isComplete.interactable = (exerciseModel.reps > 0);
                    break;
                case ExerciseType.TimeBased:
                    isComplete.interactable = (exerciseModel.time > 0);
                    break;
                case ExerciseType.TimeAndMiles:
                    isComplete.interactable = (exerciseModel.time > 0 && exerciseModel.mile > 0);
                    break;
                case ExerciseType.WeightAndReps:
                    isComplete.interactable = (exerciseModel.weight > 0 && exerciseModel.reps > 0);
                    break;
            }
            //if (exerciseType == ExerciseType.WeightAndReps)
            //    isComplete.interactable = (exerciseModel.weight > 0 && exerciseModel.reps > 0);
            //else
            //    isComplete.interactable = false;
        }
    }
    public void ToggleShake()
    {
        if (!isComplete.interactable && shake)
        {
            //shake = false;
            AudioController.Instance.OnError();
            //GlobalAnimator.Instance.ApplyShakeEffect(isComplete.gameObject.GetComponent<RectTransform>(), () => shake = true);
        }
    }

}
