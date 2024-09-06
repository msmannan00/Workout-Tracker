using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class ExerciseController : MonoBehaviour, PageController
{
    public Transform content;
    public TMP_InputField searchInputField;
    public Button addExerciseButton;
    public Button alphabetic, byRank, performed;
    public Color buttonUnselectColor;
    public Color exerciseUnselectColor;

    private SearchButtonType currentButton;
    public List<ExerciseDataItem> selectedExercises = new List<ExerciseDataItem>();
    private List<GameObject> alphabetLabels = new List<GameObject>();
    private List<GameObject> exerciseItems = new List<GameObject>();
    private Action<List<ExerciseDataItem>> callback;
    private bool isWorkoutLog;

    public HistoryModel testHistory = new HistoryModel();

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
        isWorkoutLog = (bool)data["isWorkoutLog"];
        if(isWorkoutLog)
        {
            addExerciseButton.onClick.AddListener(() => AddExerciseToWorkoutLog());
        }
        else
        {
            addExerciseButton.onClick.AddListener(() => AddExerciseToCreateWorkout());
        }
    }


    void Start()
    {
        //SaveTestHistory();

        LoadExercises();
        searchInputField.onValueChanged.AddListener(OnSearchChanged);
        alphabetic.onClick.AddListener(() => LoadExercises());
        byRank.onClick.AddListener(() => ByRankExercises(""));
        performed.onClick.AddListener(() => PerformedExercises(""));
    }
    void AddAlphabeticLabels()
    {
        for (char letter = 'A'; letter <= 'Z'; letter++)
        {
            GameObject textLabelObject = new GameObject($"Label_{letter}");
            textLabelObject.transform.SetParent(content, false);

            TextMeshProUGUI textMeshPro = textLabelObject.AddComponent<TextMeshProUGUI>();
            textMeshPro.text = letter.ToString();
            textMeshPro.fontSize = 17;
            textMeshPro.color = Color.white;
            textMeshPro.fontStyle = FontStyles.Bold;
            textMeshPro.alignment = TextAlignmentOptions.Left;
            textMeshPro.margin = new Vector4(20, 0, 0, 0);

            alphabetLabels.Add(textLabelObject);
        }
    }
    public static List<string> GetUniqueExercises(HistoryModel historyData)
    {
        // Create a HashSet to store unique exercise names
        HashSet<string> uniqueExercises = new HashSet<string>();

        // Iterate over each HistoryTempleteModel in the historyData
        foreach (var template in historyData.exerciseTempleteModel)
        {
            // Iterate over each HistoryExerciseTypeModel in the current HistoryTempleteModel
            foreach (var exerciseType in template.exerciseTypeModel)
            {
                // Add each exercise name to the HashSet
                uniqueExercises.Add(exerciseType.exerciseName);
            }
        }

        // Convert HashSet to List and return it
        return uniqueExercises.ToList();
    }
    void PerformedExercises(string filter)
    {
        addExerciseButton.gameObject.SetActive(false);
        selectedExercises.Clear();
        currentButton = SearchButtonType.Performed;
        SetSelectedButton();

        if (alphabetLabels != null)
        {
            foreach (GameObject label in alphabetLabels)
            {
                if (label != null)
                {
                    Destroy(label);
                }
            }
            alphabetLabels.Clear();
        }

        foreach (GameObject item in exerciseItems)
        {
            Destroy(item);
        }
        exerciseItems.Clear();

        ExerciseData exerciseData = DataManager.Instance.getExerciseData();
        HistoryModel historyData = userSessionManager.Instance.historyData;
        List<string> filterExercises = GetUniqueExercises(historyData);

        string lowerFilter = filter.ToLower(); // Convert filter to lowercase for case-insensitive comparison

        foreach (ExerciseDataItem exercise in exerciseData.exercises)
        {
            // Check if the exercise name or category should be filtered out
            if (!string.IsNullOrEmpty(lowerFilter) &&
                !(exercise.exerciseName.ToLower().Contains(lowerFilter) ||
                  exercise.category.ToLower().Contains(lowerFilter)))
            {
                continue;
            }

            // Check if the exercise name is in the list of filterExercises
            if (filterExercises != null && !filterExercises.Contains(exercise.exerciseName))
            {
                continue;
            }

            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/exercise/exerciseScreenDataModel");
            GameObject newExerciseObject = Instantiate(exercisePrefab, content);


            ExerciseItem newExerciseItem = newExerciseObject.GetComponent<ExerciseItem>();

            Dictionary<string, object> initData = new Dictionary<string, object>
            {
                { "data", exercise },
            };

            newExerciseItem.onInit(initData);

            Button button = newExerciseObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    //callback?.Invoke(exercise);
                    SelectAndDeselectExercise(newExerciseObject, exercise);
                    //OnClose();
                });
            }

            exerciseItems.Add(newExerciseObject);
        }
    }
    void SelectAndDeselectExercise(GameObject obj, ExerciseDataItem exercise)
    {
        if (selectedExercises.Contains(exercise))
        {
            selectedExercises.Remove(exercise);
            obj.GetComponent<Image>().color= exerciseUnselectColor;
            if(selectedExercises.Count <= 0)
            {
                addExerciseButton.gameObject.SetActive(false);
            }
        }
        else
        {
            selectedExercises.Add(exercise);
            obj.GetComponent<Image>().color = Color.blue;
            if (selectedExercises.Count > 0)
            {
                addExerciseButton.gameObject.SetActive(true);
            }
        }
    }
    void SetSelectedButton()
    {
        if (currentButton == SearchButtonType.Alphabetic)
        {
            alphabetic.gameObject.GetComponent<Image>().color = Color.blue;
            byRank.gameObject.GetComponent<Image>().color = buttonUnselectColor;
            performed.gameObject.GetComponent<Image>().color = buttonUnselectColor;
        }
        else if(currentButton == SearchButtonType.ByRank)
        {
            alphabetic.gameObject.GetComponent<Image>().color = buttonUnselectColor;
            byRank.gameObject.GetComponent<Image>().color = Color.blue;
            performed.gameObject.GetComponent<Image>().color = buttonUnselectColor;
        }
        else if (currentButton == SearchButtonType.Performed)
        {
            alphabetic.gameObject.GetComponent<Image>().color = buttonUnselectColor;
            byRank.gameObject.GetComponent<Image>().color = buttonUnselectColor;
            performed.gameObject.GetComponent<Image>().color = Color.blue;
        }
    }
    void LoadExercises(string filter = "")
    {
        addExerciseButton.gameObject.SetActive(false);
        selectedExercises.Clear();
        currentButton = SearchButtonType.Alphabetic;
        SetSelectedButton();
        foreach (GameObject item in exerciseItems)
        {
            Destroy(item);
        }
        exerciseItems.Clear();

        if (alphabetLabels != null)
        {
            foreach (GameObject label in alphabetLabels)
            {
                if (label != null)
                {
                    Destroy(label);
                }
            }
            alphabetLabels.Clear();
        }
        AddAlphabeticLabels();

        ExerciseData exerciseData = DataManager.Instance.getExerciseData();

        foreach (ExerciseDataItem exercise in exerciseData.exercises)
        {
            string lowerFilter = filter.ToLower();

            if (!string.IsNullOrEmpty(lowerFilter) &&
                !(exercise.exerciseName.ToLower().Contains(lowerFilter) ||
                  exercise.category.ToLower().Contains(lowerFilter)))
            {
                continue;
            }

            char firstLetter = char.ToUpper(exercise.exerciseName[0]);
            GameObject targetLabel = alphabetLabels.Find(label => label.name == $"Label_{firstLetter}");

            if (targetLabel != null)
            {
                GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/exercise/exerciseScreenDataModel");
                GameObject newExerciseObject = Instantiate(exercisePrefab, content);

                int labelIndex = targetLabel.transform.GetSiblingIndex();
                newExerciseObject.transform.SetSiblingIndex(labelIndex + 1);

                ExerciseItem newExerciseItem = newExerciseObject.GetComponent<ExerciseItem>();

                Dictionary<string, object> initData = new Dictionary<string, object>
                {
                { "data", exercise },
                };

                newExerciseItem.onInit(initData);

                Button button = newExerciseObject.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        //callback?.Invoke(exercise);
                        SelectAndDeselectExercise(newExerciseObject, exercise);
                        //OnClose();
                    });
                }

                exerciseItems.Add(newExerciseObject);
            }
        }
    }

    void ByRankExercises(string filter)
    {
        addExerciseButton.gameObject.SetActive(false);
        selectedExercises.Clear();
        currentButton = SearchButtonType.ByRank;
        SetSelectedButton();

        if (alphabetLabels != null)
        {
            foreach (GameObject label in alphabetLabels)
            {
                if (label != null)
                {
                    Destroy(label);
                }
            }
            alphabetLabels.Clear();
        }

        foreach (GameObject item in exerciseItems)
        {
            Destroy(item);
        }
        exerciseItems.Clear();

        ExerciseData exerciseData = DataManager.Instance.getExerciseData();

        string lowerFilter = filter.ToLower();

        var filteredAndSortedExercises = exerciseData.exercises
            .Where(exercise =>
                string.IsNullOrEmpty(lowerFilter) ||
                exercise.exerciseName.ToLower().Contains(lowerFilter) ||
                exercise.category.ToLower().Contains(lowerFilter))
            .OrderByDescending(exercise => exercise.rank)
            .ToList();

        foreach (ExerciseDataItem exercise in filteredAndSortedExercises)
        {
            
            char firstLetter = char.ToUpper(exercise.exerciseName[0]);

           

            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/exercise/exerciseScreenDataModel");
            GameObject newExerciseObject = Instantiate(exercisePrefab, content);


            ExerciseItem newExerciseItem = newExerciseObject.GetComponent<ExerciseItem>();

            Dictionary<string, object> initData = new Dictionary<string, object>
            {
                { "data", exercise },
            };

            newExerciseItem.onInit(initData);

            Button button = newExerciseObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    //callback?.Invoke(exercise);
                    SelectAndDeselectExercise(newExerciseObject, exercise);
                    //OnClose();
                });
            }

            exerciseItems.Add(newExerciseObject);
        }
    }
    void OnSearchChanged(string searchQuery)
    {
        switch (currentButton)
        {
            case SearchButtonType.Alphabetic:
                LoadExercises(searchQuery);
                break;
            case SearchButtonType.ByRank:
                ByRankExercises(searchQuery);
                break;
            case SearchButtonType.Performed:
                PerformedExercises(searchQuery);
                break;
        }

    }

    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        print("close");
    }
    public void AddNewExercise()
    {
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "addNewExerciseScreen", null,true,OnAddExercise);
    }
    void OnAddExercise(object data)
    {
        OnSearchChanged("");
    }

    public void AddExerciseToWorkoutLog()
    {
        callback?.Invoke(selectedExercises);
        OnClose();
    }
    public void AddExerciseToCreateWorkout()
    {
        callback?.Invoke(selectedExercises);
        OnClose();
    }
    public void SaveTestHistory()
    {
        // test code for history
        if (!PreferenceManager.Instance.HasKey("historyData"))
        {
            string json = JsonUtility.ToJson(testHistory);
            PreferenceManager.Instance.SetString("historyData", json);
            PreferenceManager.Instance.Save();
            print("save");
            print(json);
        }
        userSessionManager.Instance.LoadHistory();
    }
}
