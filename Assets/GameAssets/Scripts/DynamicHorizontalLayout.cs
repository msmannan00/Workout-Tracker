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
        //layoutGroup = GetComponent<HorizontalLayoutGroup>();
        //SetDynamicPadding();

        float basePadding = 188f;  // The original padding value you used for a 1080p screen
        float baseScreenWidth = 1080f;  // Your base screen resolution width
        float currentScreenWidth = Screen.width;  // Get current screen width
        float scalingFactor = currentScreenWidth / baseScreenWidth;  // Calculate scaling factor

        // Dynamically adjust the padding
        int dynamicPadding = Mathf.RoundToInt(basePadding * scalingFactor);

        // Apply the calculated padding to the Horizontal Layout Group
        this.gameObject.GetComponent<HorizontalLayoutGroup>().padding.left = dynamicPadding;
        this.gameObject.GetComponent<HorizontalLayoutGroup>().padding.right = dynamicPadding;


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
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a == i - 1 || a == i + 1) // Check if it's a neighbor
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
    void SetDynamicPadding()
    {
        // Calculate the padding based on screen width
        childCount = transform.childCount;

        // Total width of all children and spacing
        float totalChildrenWidth = (childWidth * childCount) + (spacing * (childCount - 1));

        // Padding will be half the remaining width after accounting for children and spacing
        float remainingWidth = Screen.width - totalChildrenWidth;
        float padding = remainingWidth / 2;

        layoutGroup.padding.left = (int)padding;
        layoutGroup.padding.right = (int)padding;
    }
}