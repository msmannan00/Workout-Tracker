using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicSpacing : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private RectTransform parentRect; // The parent container of the layout group (e.g., Canvas or Panel)
    [SerializeField] private RectTransform[] childItems; // Manually assign or fetch child items dynamically

    private void Start()
    {
        AdjustSpacing();
    }

    private void Update()
    {
        // Recalculate spacing if screen size changes
        AdjustSpacing();
    }

    private void AdjustSpacing()
    {
        if (layoutGroup == null || parentRect == null || childItems.Length == 0)
            return;

        // Get the width of the parent container
        float parentWidth = parentRect.rect.width;

        // Calculate the total width of all child items
        float totalChildWidth = 0f;
        int activeItems = 0;
        foreach (RectTransform child in childItems)
        {
            if (child.gameObject.activeInHierarchy)
            {
                totalChildWidth += child.rect.width;
                activeItems++;
            }
        }

        // Calculate remaining space (avoid negative values)
        float remainingSpace = Mathf.Max(0, parentWidth - totalChildWidth);

        // Adjust spacing
        int numberOfSpaces = activeItems;
        layoutGroup.spacing = numberOfSpaces > 0 ? remainingSpace / numberOfSpaces : 0;

    }

}
