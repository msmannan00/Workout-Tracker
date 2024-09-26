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
    public int childCount;
    void Start()
    {
        layoutGroup = this.GetComponent<HorizontalLayoutGroup>();
        RectTransform canvas= GameObject.Find("canvas").GetComponent<RectTransform>();
        int padding = Mathf.RoundToInt((canvas.rect.width - childWidth) / 2);
        // Apply the calculated padding to the Horizontal Layout Group
        layoutGroup.padding.left = padding;
        layoutGroup.padding.right = padding;
    }

    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(
                    transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().fontSize, 36f, 0.1f);
                transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
                int result;
                bool success = int.TryParse(transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text, out result);
                if (success)
                {
                    userSessionManager.Instance.currentWeight = result;
                }
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a == i - 1 || a == i + 1)
                    {
                        // Set the font size of the neighbors
                        transform.GetChild(a).gameObject.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(
                            transform.GetChild(a).gameObject.GetComponent<TextMeshProUGUI>().fontSize, 32f, 0.1f);
                        transform.GetChild(a).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(92, 59, 28, 155);
                    }
                    else if (a != i) // For all other items, set the font size to 10
                    {
                        transform.GetChild(a).gameObject.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(
                            transform.GetChild(a).gameObject.GetComponent<TextMeshProUGUI>().fontSize, 24f, 0.1f);
                        transform.GetChild(a).gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(92, 59, 28, 80);
                    }
                }
            }
        }
    }
}