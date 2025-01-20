using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectBodyParts : MonoBehaviour,PageController
{
    public TextMeshProUGUI label,bodyPartLabel;
    public GameObject prefab;
    public RectTransform content;
    public Button back;
    public Image backButton;
    public float maxRowWidth = 350f; // Maximum width for one row
    public float verticalSpacing = 20f; // Spacing between rows
    public float horizontalSpacing = 10f;
    private int globalCounter = 0;
    ExerciseController controller;
    void PageController.onInit(Dictionary<string, object> data, Action<object> callback)
    {
        controller = (ExerciseController)data["controller"];
        float currentX = 0;
        float currentY = 0;
        List<string> bodyParts = GetUniqueBodyParts(ApiDataHandler.Instance.getExerciseData());
        Color itemColor= Color.white;
        
        foreach (string text in bodyParts)
        {
            // Instantiate text prefab
            GameObject newTextObj = Instantiate(prefab, content);
            TextMeshProUGUI textComponent = newTextObj.GetComponentInChildren<TextMeshProUGUI>();
            newTextObj.transform.GetChild(1).gameObject.SetActive(false);
            textComponent.text = text;
            textComponent.color = Color.white;
            textComponent.font = userSessionManager.Instance.lightPrimaryFontBold;
            newTextObj.GetComponent<Button>().onClick.AddListener(() => SelectAndDeselect(text, newTextObj, controller, itemColor));
            newTextObj.GetComponent<Image>().color = userSessionManager.Instance.lightButtonColor;
            LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform);
            float textWidth = textComponent.preferredWidth+15;
            if (currentX + textWidth > maxRowWidth)
            {
                currentX = 0;
                currentY -= textComponent.preferredHeight + verticalSpacing;
            }
            RectTransform textRect = newTextObj.GetComponent<RectTransform>();
            textRect.pivot = new Vector2(0, 1);
            textRect.anchorMin = new Vector2(0, 1);
            textRect.anchorMax = new Vector2(0, 1);
            textRect.sizeDelta = new Vector2(textWidth, textRect.sizeDelta.y);
            textRect.anchoredPosition = new Vector2(currentX, currentY);
            currentX += textWidth + horizontalSpacing;
        }
        float contentHeight = Mathf.Abs(currentY) + verticalSpacing;
        content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);
        back.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClose();
        }
    }
    public void SelectAndDeselect(string text, GameObject obj, ExerciseController controller,Color col)
    {
        if (controller.selectedBodyParts.Contains(text))
        {
            int matchingCount = GetMatchingCategoryCount(ApiDataHandler.Instance.getExerciseData(), text);
            globalCounter -= matchingCount;
            controller.selectedBodyParts.Remove(text);
            obj.GetComponent<Image>().color = userSessionManager.Instance.lightButtonColor;
            label.text = "Filter(" + globalCounter.ToString() + ")";
        }
        else
        {
            controller.selectedBodyParts.Add(text);
            int matchingCount = GetMatchingCategoryCount(ApiDataHandler.Instance.getExerciseData(), text);
            globalCounter += matchingCount;
            obj.GetComponent<Image>().color = new Color32(150, 0, 0,255);
            label.text = "Filter(" + globalCounter.ToString() + ")";
        }
    }
    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        if (controller.selectedBodyParts.Count != 0)
        {
            controller.LoadExercisesByBodyParts();
            controller.CreateBodyPartChecks();
        }
    }
    public List<string> GetUniqueBodyParts(ExerciseData excerciseData)
    {
        // Create a HashSet to store unique exercise names
        HashSet<string> uniqueExercises = new HashSet<string>();

        // Iterate over each HistoryTempleteModel in the historyData
        foreach (var template in excerciseData.exercises)
        {
            try{
                if (!template.category.Contains("/"))
                {
                    uniqueExercises.Add(template.category);
                }
            }catch(Exception ex){
                continue;
            }
        }

        // Convert HashSet to List and return it
        return uniqueExercises.ToList();
    }
    public int GetMatchingCategoryCount(ExerciseData exerciseData, string categoryToMatch)
    {
        // Initialize a counter for the matching categories
        int matchingCount = 0;

        // Iterate over each ExerciseDataItem in the exerciseData
        foreach (var template in exerciseData.exercises)
        {
            try{
            // Check if the template's category contains "/"
                if (template.category.Contains("/"))
                {
                    // Split the category into parts using "/"
                    var categories = template.category.Split('/');

                    // Trim each part and check if any matches the provided category string
                    if (categories.Any(cat => cat.Trim().Equals(categoryToMatch, StringComparison.OrdinalIgnoreCase)))
                    {
                        matchingCount++; // Increment the counter if there's a match
                    }
                }
                else
                {
                    // Check if the template's category matches the provided category string
                    if (template.category.Equals(categoryToMatch, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingCount++; // Increment the counter if there's a match
                    }
                }
            }catch(Exception ex){
                continue;
            }
        }

        // Return the total count of matching categories
        return matchingCount;
    }
}
