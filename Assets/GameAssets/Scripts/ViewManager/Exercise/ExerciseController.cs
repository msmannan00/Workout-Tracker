using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using PlayFab.ClientModels;
using UnityEditor;

public class ExerciseController : MonoBehaviour, PageController
{
    public Transform content;
    public TextMeshProUGUI labelText;
    public TMP_InputField searchInputField;
    public Button addExerciseButton;
    public Button alphabetic, byRank, performed;
    public Color buttonUnselectColor;
    public GameObject bodyPartPrefab;
    public Transform bodyPartsContent;

    public List<Image> themeColorItems;
    public Image searchIcon1, searchIcon2;
    private SearchButtonType currentButton;
    public List<string> selectedBodyParts=new List<string>();
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

        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Light:
               // searchInputField.gameObject.GetComponent<Image>().color = Color.white;
                searchInputField.placeholder.color = userSessionManager.Instance.lightTextColor;
                searchInputField.textComponent.color= userSessionManager.Instance.lightTextColor;
                labelText.color = userSessionManager.Instance.lightHeadingColor;
                labelText.font = userSessionManager.Instance.lightHeadingFont;
                foreach (Image image in themeColorItems)
                {
                    image.color = userSessionManager.Instance.lightButtonColor;
                }
                searchIcon1.color= userSessionManager.Instance.lightTextColor;
                searchIcon2.color= userSessionManager.Instance.lightTextColor;
                break;
            case Theme.Dark:
              //  this.gameObject.GetComponent<Image>().color = userSessionManager.Instance.darkBgColor;
                searchInputField.gameObject.GetComponent<Image>().color = userSessionManager.Instance.darkSearchBarColor;
                searchInputField.placeholder.color = userSessionManager.Instance.darkSearchIconColor;
                searchInputField.textComponent.color = userSessionManager.Instance.darkSearchIconColor;
                labelText.color = Color.white;
                labelText.font = userSessionManager.Instance.darkHeadingFont;
                foreach (Image image in themeColorItems)
                {
                    image.color = Color.white;
                }
                searchIcon1.color = userSessionManager.Instance.darkSearchIconColor;
                searchIcon2.color = userSessionManager.Instance.darkSearchIconColor;
                break;
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
        alphabetic.onClick.AddListener(() =>ClearSearchBar());
        byRank.onClick.AddListener(() => ClearSearchBar());
        performed.onClick.AddListener(() => ClearSearchBar());
    }

    public void SortBodyParts()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { "controller", this.GetComponent<ExerciseController>() }
            };
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "bodyPartsScreen", mData, true, null);
    }
    void AddAlphabeticLabels()
    {
        for (char letter = 'A'; letter <= 'Z'; letter++)
        {
            GameObject labelPrefab = Resources.Load<GameObject>("Prefabs/exercise/exerciseLabel");
            GameObject textLabelObject = Instantiate(labelPrefab, content);
            TextMeshProUGUI textMeshPro = textLabelObject.GetComponentInChildren<TextMeshProUGUI>();
            textLabelObject.name = $"Label_{letter}";
            textMeshPro.text = letter.ToString();
            switch (userSessionManager.Instance.gameTheme)
            {
                case Theme.Light:
                    textMeshPro.font = userSessionManager.Instance.lightHeadingFont;
                    textMeshPro.color = userSessionManager.Instance.lightHeadingColor;
                    textLabelObject.GetComponentInChildren<Image>().color = userSessionManager.Instance.lightButtonColor;
                    break;
                case Theme.Dark:
                    textMeshPro.font = userSessionManager.Instance.darkHeadingFont;
                    textMeshPro.color = Color.white;
                    textLabelObject.GetComponentInChildren<Image>().color = Color.white;
                    break;
            }
            alphabetLabels.Add(textLabelObject.gameObject);
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
        foreach(string name in filterExercises)
        {
            print(name);
        }
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
            Color col = obj.GetComponent<Image>().color;
            col.a = 0;
            obj.GetComponent<Image>().color = col;
            obj.GetComponent<ExerciseItem>().selected.SetActive(false);
            if (selectedExercises.Count <= 0)
            {
                addExerciseButton.gameObject.SetActive(false);
            }
        }
        else
        {
            selectedExercises.Add(exercise);
            Color col = obj.GetComponent<Image>().color;
            col.a = 1;
            obj.GetComponent<Image>().color = col;
            obj.GetComponent<ExerciseItem>().selected.SetActive(true);
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

        List<GameObject> relevantLabels = new List<GameObject>();

        bool showAll = string.IsNullOrEmpty(filter);

        foreach (ExerciseDataItem exercise in exerciseData.exercises)
        {
            string lowerFilter = filter.ToLower();

            if (!showAll &&
                !(exercise.exerciseName.ToLower().Contains(lowerFilter) ||
                  exercise.category.ToLower().Contains(lowerFilter)))
            {
                continue;
            }

            char firstLetter = char.ToUpper(exercise.exerciseName[0]);
            GameObject targetLabel = alphabetLabels.Find(label => label.name == $"Label_{firstLetter}");

            if (targetLabel != null)
            {
                if (!relevantLabels.Contains(targetLabel))
                {
                    relevantLabels.Add(targetLabel);
                }

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
                        SelectAndDeselectExercise(newExerciseObject, exercise);
                    });
                }

                exerciseItems.Add(newExerciseObject);
            }
        }
        foreach (GameObject label in alphabetLabels)
        {
            if (showAll)
            {
                label.SetActive(true);
            }
            else if (relevantLabels.Contains(label))
            {
                label.SetActive(true);
            }
            else
            {
                label.SetActive(false);
            }
        }
    }

    public void LoadExercisesByBodyParts()
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

        List<GameObject> relevantLabels = new List<GameObject>();

        foreach (ExerciseDataItem exercise in exerciseData.exercises)
        {
            // Check if the exercise category is in the provided list
            if (!selectedBodyParts.Contains(exercise.category))
            {
                continue;
            }

            char firstLetter = char.ToUpper(exercise.exerciseName[0]);
            GameObject targetLabel = alphabetLabels.Find(label => label.name == $"Label_{firstLetter}");

            if (targetLabel != null)
            {
                if (!relevantLabels.Contains(targetLabel))
                {
                    relevantLabels.Add(targetLabel);
                }

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
                        SelectAndDeselectExercise(newExerciseObject, exercise);
                    });
                }

                exerciseItems.Add(newExerciseObject);
            }
        }
        foreach (GameObject label in alphabetLabels)
        {
            if (relevantLabels.Contains(label))
            {
                label.SetActive(true);
            }
            else
            {
                label.SetActive(false);
            }
        }
    }

    public void CreateBodyPartChecks()
    {
        foreach (string text in selectedBodyParts)
        {
            // Instantiate text prefab
            GameObject newTextObj = Instantiate(bodyPartPrefab, bodyPartsContent);
            newTextObj.transform.GetChild(1).gameObject.SetActive(true);
            TextMeshProUGUI textComponent = newTextObj.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = text;
            textComponent.color = new Color32(51, 23, 23, 255);
            newTextObj.GetComponent<Button>().onClick.AddListener(() => UnSelectBodypart(newTextObj,text));
            //newTextObj.GetComponent<Image>().color = itemColor;
            LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform);
            float textWidth = textComponent.preferredWidth + 15;
            RectTransform textRect = newTextObj.GetComponent<RectTransform>();
            RectTransform objRect = newTextObj.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(textWidth, textRect.sizeDelta.y);
            objRect.sizeDelta = new Vector2(textWidth, objRect.sizeDelta.y);
        }
    }
    public void UnSelectBodypart(GameObject obj,string text)
    {
        Destroy(obj);
        selectedBodyParts.Remove(text);
        if(selectedBodyParts.Count > 0)
        {
            LoadExercisesByBodyParts();
        }
        else
        {
            LoadExercises();
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

    void ClearSearchBar()
    {
        searchInputField.text = "";
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
