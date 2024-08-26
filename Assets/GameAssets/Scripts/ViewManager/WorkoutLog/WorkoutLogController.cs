using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutLogController : MonoBehaviour, PageController
{
    public InputField workoutNameText;
    public Text timerText;
    public Transform content;

    private int exerciseCounter = 0;
    public List<ExerciseInformationModel> exercises = new List<ExerciseInformationModel>();
    private List<ExerciseDataItem> exerciseDataItems = new List<ExerciseDataItem>();
    private DefaultTempleteModel templeteModel = new DefaultTempleteModel();

    private bool isTemplateCreator;
    private bool isTimerRunning = false;
    private float elapsedTime = 0f;
    private Coroutine timerCoroutine;
    private Color enabledColor = Color.black;
    private Color disabledColor = Color.gray;
    Action<object> callback;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        isTemplateCreator = (bool)data["isTemplateCreator"];

        if (data.ContainsKey("dataTemplate"))
        {
            DefaultTempleteModel dataTemplate = (DefaultTempleteModel)data["dataTemplate"];
            foreach (var exerciseType in dataTemplate.exerciseTemplete)
            {
                OnExerciseAdd(exerciseType);
            }
        }
    }

    private void Start()
    {
        timerText.color = disabledColor;
        if (workoutNameText != null)
        {
            workoutNameText.onEndEdit.AddListener(OnNameChanged);
        }
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
            timerText.text = "Timer: " + string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }
    }

    public void OnNameChanged(string name)
    {
        templeteModel.templeteName = name;
    }

    public void AddExerciseButton()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>();
        NavigationManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true, OnExerciseAdd);
    }

    public void OnExerciseAdd(object data)
    {
        ExerciseTypeModel typeModel;

        if (data is ExerciseDataItem dataItem)
        {
            typeModel = new ExerciseTypeModel
            {
                name = dataItem.exerciseName,
                exerciseModel = new List<ExerciseModel>(),
                index = exerciseCounter++
            };

            templeteModel.exerciseTemplete.Add(typeModel);

            exerciseDataItems.Add(dataItem);
        }
        else
        {
            typeModel = (ExerciseTypeModel)data;
            templeteModel.exerciseTemplete.Add(typeModel);
        }

        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { "data", typeModel },
        };

        GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/workoutLog/workoutLogScreenDataModel");
        GameObject exerciseObject = Instantiate(exercisePrefab, content);
        exerciseObject.GetComponent<workoutLogScreenDataModel>().onInit(mData, OnRemoveIndex);
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
        foreach (var exerciseType in templeteModel.exerciseTemplete)
        {
            foreach (var exercise in exerciseType.exerciseModel)
            {
                exercise.setID = 1;
                exercise.previous = "-";
                exercise.weight = 0;
                exercise.lbs = 0;
                exercise.reps = 0;
            }
        }

        if (isTemplateCreator && templeteModel.exerciseTemplete.Count > 0)
        {
            userSessionManager.Instance.excerciseData.exerciseTemplete.Add(templeteModel);
            NavigationManager.Instance.HandleBackAction(gameObject);
            this.callback.Invoke(null);
            userSessionManager.Instance.SaveExcerciseData();
        }
        OnBack();
    }

    public void OnBack()
    {
        NavigationManager.Instance.HandleBackAction(gameObject);
    }
}
