using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WorkoutLogController : MonoBehaviour, PageController
{
    public TextMeshProUGUI workoutNameText;
    public TMP_InputField workoutNotes;
    public TMP_InputField editWorkoutName;
    public TextMeshProUGUI timerText;
    public Button editWorkoutButton;
    public Image back, watch, watchpins, addExercise1, addExercise2, line, save, cancle;
    public Transform content;

    private int exerciseCounter = 0;
    public List<ExerciseInformationModel> exercises = new List<ExerciseInformationModel>();
    private List<ExerciseDataItem> exerciseDataItems = new List<ExerciseDataItem>();
    public DefaultTempleteModel templeteModel = new DefaultTempleteModel();

    private bool isTemplateCreator;
    private bool isTimerRunning = false;
    private float elapsedTime = 0f;
    private Coroutine timerCoroutine;
    private Color enabledColor = Color.white;
    private Color disabledColor = Color.gray;
    Action<object> callback;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        isTemplateCreator = (bool)data["isTemplateCreator"];

        if (data.ContainsKey("dataTemplate"))
        {
            DefaultTempleteModel dataTemplate = (DefaultTempleteModel)data["dataTemplate"];
            templeteModel.templeteName= dataTemplate.templeteName;
            workoutNotes.text = dataTemplate.templeteNotes;
            List<ExerciseTypeModel> list = new List<ExerciseTypeModel>();
            foreach (var exerciseType in dataTemplate.exerciseTemplete)
            {
                list.Add(exerciseType);
                print(exerciseType.name);
            }
            OnExerciseAdd(list);
            if (workoutNameText != null)
            {
                workoutNameText.text = dataTemplate.templeteName;
                editWorkoutName.textComponent.text = dataTemplate.templeteNotes; 
                float textWidth = workoutNameText.preferredWidth;
                workoutNameText.transform.GetComponent<RectTransform>().sizeDelta=new Vector2(textWidth, workoutNameText.transform.GetComponent<RectTransform>().sizeDelta.y);
            }
            workoutNotes.onValueChanged.AddListener(OnNotesChange);
        }
        else
        {
            workoutNameText.text = (string)data["templeteName"];
            editWorkoutName.text = (string)data["templeteName"];
            float textWidth = workoutNameText.preferredWidth;
            workoutNameText.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, workoutNameText.transform.GetComponent<RectTransform>().sizeDelta.y);

        }
        editWorkoutName.onEndEdit.AddListener(OnNameChanged);
        editWorkoutButton.onClick.AddListener(EditWorkoutName);
        OnToggleWorkout();
    }

    private void OnEnable()
    {
        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Dark:
               // this.gameObject.GetComponent<Image>().color = userSessionManager.Instance.darkBgColor;
                workoutNameText.font=userSessionManager.Instance.darkHeadingFont;
                workoutNameText.color = Color.white;
                timerText.color = Color.white;
                back.color = Color.white;
                watch.color = Color.white;
                watchpins.color = userSessionManager.Instance.darkBgColor;
                addExercise1.color = Color.white;
                addExercise2.color = Color.white;
                line.color = Color.white;
                save.color = Color.white;
                cancle.color = Color.white;
                save.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().font = userSessionManager.Instance.darkHeadingFont;
                save.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().color = userSessionManager.Instance.darkBgColor;
                cancle.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().font = userSessionManager.Instance.darkHeadingFont;
                cancle.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().color = userSessionManager.Instance.darkBgColor;
                break;
            case Theme.Light:
               // this.gameObject.GetComponent<Image>().color = userSessionManager.Instance.lightBgColor;
                workoutNameText.font = userSessionManager.Instance.lightHeadingFont;
                workoutNameText.color = userSessionManager.Instance.lightHeadingColor;
                timerText.color = userSessionManager.Instance.lightHeadingColor;
                back.color = userSessionManager.Instance.lightButtonColor;
                watch.color = userSessionManager.Instance.lightButtonColor;
                watchpins.color = Color.white;
                addExercise1.color = userSessionManager.Instance.lightButtonColor;
                addExercise2.color = userSessionManager.Instance.lightButtonColor;
                line.color = new Color32(218,52,52,150);
                save.color = userSessionManager.Instance.lightButtonColor;
                cancle.color = userSessionManager.Instance.lightButtonColor;
                save.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().font = userSessionManager.Instance.lightHeadingFont;
                save.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                cancle.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().font = userSessionManager.Instance.lightHeadingFont;
                cancle.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                break;
        }
    }
    //private void Start()
    //{
    //    timerText.color = disabledColor;
    //    if (workoutNameText != null)
    //    {
    //        workoutNameText.onEndEdit.AddListener(OnNameChanged);
    //    }
    //}

    void EditWorkoutName()
    {
        workoutNameText.gameObject.SetActive(false);
        editWorkoutName.gameObject.SetActive(true);
        editWorkoutName.text=workoutNameText.text;
    }
    public void OnToggleWorkout()
    {
        if (templeteModel.exerciseTemplete.Count == 0)
        {
            return;
        }

        isTimerRunning = !isTimerRunning;

        if (isTimerRunning)
        {
            timerText.color = enabledColor;

            if (timerCoroutine == null)
            {
                timerCoroutine = StartCoroutine(TimerCoroutine());
            }
        }
        else
        {
            timerText.color = disabledColor;

            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = /*"Timer: " +*/ string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }
    }

    public void OnNameChanged(string name)
    {
        templeteModel.templeteName = name.ToUpper();
        workoutNameText.text= name.ToUpper();
        float textWidth = workoutNameText.preferredWidth;
        workoutNameText.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, workoutNameText.transform.GetComponent<RectTransform>().sizeDelta.y);
        workoutNameText.gameObject.SetActive(true);
        editWorkoutName.gameObject.SetActive(false);

    }

    public void AddExerciseButton()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "isWorkoutLog", true }
        };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true, OnExerciseAdd);
    }

    private void OnNotesChange(string input)
    {
        templeteModel.templeteNotes = input;
    }

    public void OnExerciseAdd(object data)
    {
        //List<object> dataList = data as List<object>;
        if (data == null)
        {
            print("data null");
        }

        if (data is List<ExerciseDataItem> dataList)
        {
            foreach (object item in dataList)
            {
                ExerciseTypeModel typeModel;

                if (item is ExerciseDataItem dataItem)
                {
                    typeModel = new ExerciseTypeModel
                    {
                        name = dataItem.exerciseName,
                        exerciseModel = new List<ExerciseModel>(),
                        index = exerciseCounter++,
                        exerciseType=dataItem.exerciseType
                    };

                    templeteModel.exerciseTemplete.Add(typeModel);

                    exerciseDataItems.Add(dataItem);
                }
                else
                {
                    typeModel = (ExerciseTypeModel)item;
                    templeteModel.exerciseTemplete.Add(typeModel);
                }

                Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { "data", typeModel }, { "isWorkoutLog", true }
                };

                GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/workoutLog/workoutLogScreenDataModel");
                GameObject exerciseObject = Instantiate(exercisePrefab, content);
                int childCount = content.childCount;
                exerciseObject.transform.SetSiblingIndex(childCount - 2);
                exerciseObject.GetComponent<workoutLogScreenDataModel>().onInit(mData, OnRemoveIndex);
            }
        }
        else if (data is List<ExerciseTypeModel> dataList2)
        {
            foreach (object item in dataList2)
            {
                ExerciseTypeModel typeModel;

                typeModel = (ExerciseTypeModel)item;
                templeteModel.exerciseTemplete.Add(typeModel);

                Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { "data", typeModel },
                    { "isWorkoutLog", true }
                };

                GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/workoutLog/workoutLogScreenDataModel");
                GameObject exerciseObject = Instantiate(exercisePrefab, content);
                exerciseObject.GetComponent<workoutLogScreenDataModel>().onInit(mData, OnRemoveIndex);
            }
        }
        else { print("null"); }
    }
    private void OnRemoveIndex(object data)
    {
        if (isTemplateCreator)
        {
            int index = (int)data;

            for (int i = 0; i < templeteModel.exerciseTemplete.Count; i++)
            {
                if (templeteModel.exerciseTemplete[i].index == index)
                {
                    templeteModel.exerciseTemplete.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void Finish()
    {
        isTimerRunning = false;
        DateTime currentDateTime = DateTime.Now;
        var historyTemplate = new HistoryTempleteModel
        {
            templeteName = templeteModel.templeteName,
            dateTime = currentDateTime.ToString("MMM dd, yyyy"),
            completedTime = (int)elapsedTime,
            totalWeight = CalculateTotalWeight(templeteModel),
            prs = 0 // Assuming PRs are not tracked here. Adjust as needed.
        };
        // Populate HistoryExerciseTypeModel list
        foreach (var exerciseType in templeteModel.exerciseTemplete)
        {
            var historyExerciseType = new HistoryExerciseTypeModel
            {
                exerciseName = exerciseType.name,
                index = exerciseType.index,
                exerciseType = exerciseType.exerciseType,
                exerciseModel = new List<HistoryExerciseModel>()
            };
            // Populate HistoryExerciseModel list but only add exercises where toggle is true
            foreach (var exercise in exerciseType.exerciseModel)
            {
                print("bool "+exercise.toggle);
                if (exercise.toggle) // Only add exercise if toggle is true
                {
                    var historyExercise = new HistoryExerciseModel
                    {
                        weight = exercise.weight,
                        reps = exercise.reps,
                        time = exercise.time
                    };

                    historyExerciseType.exerciseModel.Add(historyExercise);
                }
            }

            // Only add the exerciseType if it has any exercises with toggle true
            if (historyExerciseType.exerciseModel.Count > 0)
            {
                historyTemplate.exerciseTypeModel.Add(historyExerciseType);
            }
        }
        if (historyTemplate.exerciseTypeModel.Count > 0)
        {
            userSessionManager.Instance.historyData.exerciseTempleteModel.Add(historyTemplate);
            userSessionManager.Instance.SaveHistory();
        }
        //return historyTemplate;


        foreach (var exerciseType in templeteModel.exerciseTemplete)
        {
            foreach (var exercise in exerciseType.exerciseModel)
            {
                exercise.setID = 1;
                exercise.previous = "-";
                exercise.weight = 0;
                exercise.rir = 0;
                exercise.reps = 0;
                exercise.time = 0;
                exercise.toggle = false;
            }
        }

        //if (isTemplateCreator && templeteModel.exerciseTemplete.Count > 0)
        //{
        //userSessionManager.Instance.excerciseData.exerciseTemplete.Add(templeteModel);


        //int index = GetIndexByTempleteName(templeteModel.templeteName);
        //userSessionManager.Instance.excerciseData.exerciseTemplete.RemoveAt(index);
        //userSessionManager.Instance.excerciseData.exerciseTemplete.Insert(index, templeteModel);
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null, null, false);
        this.callback.Invoke(null);
        userSessionManager.Instance.SaveExcerciseData();
        //}
        //OnBack();
    }

    public void OnBack()
    {
        //StateManager.Instance.HandleBackAction(gameObject);
        List<object> initialData = new List<object> { this.gameObject };
        PopupController.Instance.OpenPopup("workoutLog", "CancelWorkoutPopup", null, initialData);
    }
    private int CalculateTotalWeight(DefaultTempleteModel defaultTemplate)
    {
        int totalWeight = 0;

        foreach (var exerciseType in defaultTemplate.exerciseTemplete)
        {
            if (exerciseType.exerciseType == ExerciseType.WeightAndReps)
            {
                foreach (var exercise in exerciseType.exerciseModel)
                {
                    totalWeight += exercise.weight * exercise.reps;
                }
            }
        }

        return totalWeight;
    }
    public int GetIndexByTempleteName(string name)
    {
        return userSessionManager.Instance.excerciseData.exerciseTemplete.FindIndex(t => t.templeteName == name);
    }
}
