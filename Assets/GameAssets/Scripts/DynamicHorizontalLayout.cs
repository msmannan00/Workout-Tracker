using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;

public class DynamicHorizontalLayout : MonoBehaviour
{
    public GameObject scrollbar;
    public float scroll_pos = 0;
    public float[] pos;

    private HorizontalLayoutGroup layoutGroup;
    public int childWidth = 75; // Width of each child
    public int spacing = 10; // Spacing between children

    private void Start()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
        RectTransform canvas = GameObject.Find("canvas").GetComponent<RectTransform>();
        int padding = Mathf.RoundToInt((canvas.rect.width - childWidth) / 2);
        layoutGroup.padding.left = padding;
        layoutGroup.padding.right = padding;

        // Set up clickable items
        for (int i = 0; i < transform.childCount; i++)
        {
            int index = i; // Capture index for the callback
            Transform child = transform.GetChild(i);
            Button button = child.gameObject.AddComponent<Button>();
            button.onClick.AddListener(() => OnItemClick(index));
        }
    }

    private void Update()
    {
        int itemCount = transform.childCount;
        pos = new float[itemCount];
        float distance = 1f / (itemCount - 1f);

        // Calculate positions for each item
        for (int i = 0; i < itemCount; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            // Smooth scroll to the nearest position
            for (int i = 0; i < itemCount; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    SmoothScrollTo(pos[i]);
                    HighlightItem(i);
                }
            }
        }
    }

    private void SmoothScrollTo(float targetPos)
    {
        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, targetPos, 0.1f);
    }

    private void HighlightItem(int index)
    {
        // Highlight the selected item and adjust others
        for (int i = 0; i < pos.Length; i++)
        {
            TextMeshProUGUI textComponent = transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();

            if (i == index)
            {
                textComponent.fontSize = Mathf.Lerp(textComponent.fontSize, 36f, 0.1f);
                textComponent.color = Color.red;
                UpdateWeightData(textComponent.text);
            }
            else if (i == index - 1 || i == index + 1)
            {
                textComponent.fontSize = Mathf.Lerp(textComponent.fontSize, 32f, 0.1f);
                textComponent.color = new Color32(92, 59, 28, 155);
            }
            else
            {
                textComponent.fontSize = Mathf.Lerp(textComponent.fontSize, 24f, 0.1f);
                textComponent.color = new Color32(92, 59, 28, 80);
            }
        }
    }

    private void OnItemClick(int index)
    {
        // Update scroll position to center the clicked item
        scroll_pos = pos[index];
    }

    private void UpdateWeightData(string weightText)
    {
        int result;
        if (int.TryParse(weightText, out result))
        {
            userSessionManager.Instance.currentWeight = result;
            ApiDataHandler.Instance.SaveWeight(result);
        }
    }
}