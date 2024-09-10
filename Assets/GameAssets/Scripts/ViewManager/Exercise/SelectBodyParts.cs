using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectBodyParts : MonoBehaviour,PageController
{
    public TextMeshProUGUI label;
    public GameObject prefab;
    public RectTransform content;
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
        List<string> bodyParts = GetUniqueBodyParts(DataManager.Instance.exerciseData);
        foreach (string text in bodyParts)
        {
            // Instantiate text prefab
            GameObject newTextObj = Instantiate(prefab, content);
            TextMeshProUGUI textComponent = newTextObj.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = text;
            newTextObj.GetComponent<Button>().onClick.AddListener(() => SelectAndDeselect(text, newTextObj, controller));
            // Force a layout rebuild to ensure the preferred width is correct
            LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform);

            // Get the preferred width of the text
            float textWidth = textComponent.preferredWidth+15;

            // Check if the text fits in the current row
            if (currentX + textWidth > maxRowWidth)
            {
                // Move to the next row
                currentX = 0;
                currentY -= textComponent.preferredHeight + verticalSpacing;
            }

            // Set the position of the text
            RectTransform textRect = newTextObj.GetComponent<RectTransform>();
            textRect.pivot = new Vector2(0, 1); // Align to top-left corner
            textRect.anchorMin = new Vector2(0, 1);
            textRect.anchorMax = new Vector2(0, 1);

            // Adjust the width of the RectTransform to fit the text length
            textRect.sizeDelta = new Vector2(textWidth, textRect.sizeDelta.y);

            // Set the anchored position starting from the left
            textRect.anchoredPosition = new Vector2(currentX, currentY);

            // Move the current X position for the next text (add horizontalSpacing)
            currentX += textWidth + horizontalSpacing;
        }

        // Optional: Adjust content area height based on the number of rows
        float contentHeight = Mathf.Abs(currentY) + verticalSpacing;
        content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);


        //foreach (string text in bodyParts)
        //{
        //// Instantiate text prefab
        //GameObject newTextObj = Instantiate(prefab, content);
        //    TextMeshProUGUI textComponent = newTextObj.GetComponentInChildren<TextMeshProUGUI>();
        //    textComponent.text = text;
        //    newTextObj.GetComponent<Button>().onClick.AddListener(() => SelectAndDeselect(text, newTextObj, controller));
        //    // Get the preferred width of the text
        //    float textWidth = textComponent.preferredWidth+10;

        //    // Check if the text fits in the current row
        //    if (currentX + textWidth > maxRowWidth)
        //    {
        //        // Move to the next row
        //        currentX = 0;
        //        currentY -= textComponent.preferredHeight + verticalSpacing;
        //    }

        //    // Set the position of the text
        //    RectTransform textRect = newTextObj.GetComponent<RectTransform>();
        //    textRect.anchoredPosition = new Vector2(currentX, currentY);

        //    // Adjust the width of the RectTransform to fit the text length
        //    textRect.sizeDelta = new Vector2(textWidth, textRect.sizeDelta.y);

        //    // Move the current X position for the next text (add horizontalSpacing to ensure gap)
        //    currentX += textWidth + horizontalSpacing;
        //}
    }
    public void SelectAndDeselect(string text, GameObject obj, ExerciseController controller)
    {
        if (controller.selectedBodyParts.Contains(text))
        {
            int matchingCount = GetMatchingCategoryCount(DataManager.Instance.exerciseData, text);
            globalCounter -= matchingCount;
            controller.selectedBodyParts.Remove(text);
            obj.GetComponent<Image>().color = userSessionManager.Instance.lightHeadingColor;
            label.text = "Filter(" + globalCounter.ToString() + ")";
        }
        else
        {
            controller.selectedBodyParts.Add(text);
            int matchingCount = GetMatchingCategoryCount(DataManager.Instance.exerciseData, text);
            globalCounter += matchingCount;
            obj.GetComponent<Image>().color = Color.blue;
            label.text = "Filter(" + globalCounter.ToString() + ")";
        }
    }
    public void OnClose()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        if (controller.selectedBodyParts.Count != 0)
        {
            controller.LoadExercisesByBodyParts();
            print("Plat");
        }
    }
    public List<string> GetUniqueBodyParts(ExerciseData excerciseData)
    {
        // Create a HashSet to store unique exercise names
        HashSet<string> uniqueExercises = new HashSet<string>();

        // Iterate over each HistoryTempleteModel in the historyData
        foreach (var template in excerciseData.exercises)
        {
            uniqueExercises.Add(template.category);
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
            // Check if the template's category matches the provided category string
            if (template.category.Equals(categoryToMatch, StringComparison.OrdinalIgnoreCase))
            {
                matchingCount++; // Increment the counter if there's a match
            }
        }

        // Return the total count of matching categories
        return matchingCount;
    }
}
