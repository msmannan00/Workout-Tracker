using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ExerciseController : MonoBehaviour, PageController
{
    public Transform content;
    public TMP_InputField searchInputField;

    private List<GameObject> alphabetLabels = new List<GameObject>();
    private List<GameObject> exerciseItems = new List<GameObject>();
    private Action<ExerciseDataItem> callback;

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        this.callback = callback;
    }


    void Start()
    {
        for (char letter = 'A'; letter <= 'Z'; letter++)
        {
            GameObject textLabelObject = new GameObject($"Label_{letter}");
            textLabelObject.transform.SetParent(content, false);

            TextMeshProUGUI textMeshPro = textLabelObject.AddComponent<TextMeshProUGUI>();
            textMeshPro.text = letter.ToString();
            textMeshPro.fontSize = 17;
            textMeshPro.color = Color.black;
            textMeshPro.fontStyle = FontStyles.Bold;
            textMeshPro.alignment = TextAlignmentOptions.Left;
            textMeshPro.margin = new Vector4(20, 0, 0, 0);

            alphabetLabels.Add(textLabelObject);
        }

        LoadExercises();
        searchInputField.onValueChanged.AddListener(OnSearchChanged);
    }

    void LoadExercises(string filter = "")
    {
        foreach (GameObject item in exerciseItems)
        {
            Destroy(item);
        }
        exerciseItems.Clear();

        ExerciseData exerciseData = DataManager.Instance.getExerciseData();

        foreach (ExerciseDataItem exercise in exerciseData.exercises)
        {
            if (!string.IsNullOrEmpty(filter) && !exercise.exerciseName.ToLower().Contains(filter.ToLower()))
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
                        callback?.Invoke(exercise);
                        OnClose();
                    });
                }

                exerciseItems.Add(newExerciseObject);
            }
        }
    }


    void OnSearchChanged(string searchQuery)
    {
        LoadExercises(searchQuery);
    }

    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }
}
