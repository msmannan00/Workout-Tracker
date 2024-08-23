using UnityEngine;

public class widthAdjuster : MonoBehaviour
{
    private void Start()
    {
        AdjustChildrenWidth();
    }

    private void AdjustChildrenWidth()
    {
        RectTransform parentRect = GetComponent<RectTransform>();
        float parentWidth = parentRect.rect.width;  // Get the current width of the parent

        foreach (Transform child in transform)
        {
            RectTransform childRect = child.GetComponent<RectTransform>();
            if (childRect != null)
            {
                // Set the width of the child's RectTransform to the parent's width
                childRect.sizeDelta = new Vector2(0, childRect.sizeDelta.y);
            }
        }
    }
}
