using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnapScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform viewport;
    public TextMeshProUGUI focusText;

    private int totalItems;

    void Start()
    {
        totalItems = content.childCount;
    }

    void Update()
    {
        RectTransform focusedItem = GetFocusedItem();
        if (focusedItem != null)
        {
            CenterOnItem(focusedItem);
            UpdateFocusText(focusedItem);
        }
    }

    RectTransform GetFocusedItem()
    {
        float minDistance = float.MaxValue;
        RectTransform closestItem = null;

        foreach (RectTransform item in content)
        {
            Vector3 itemWorldPos = item.transform.position;
            Vector3 itemViewportPos = viewport.InverseTransformPoint(itemWorldPos);

            float distance = Mathf.Abs(itemViewportPos.x);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestItem = item;
            }
        }

        return closestItem;
    }

    void CenterOnItem(RectTransform item)
    {
        //Vector2 viewportCenter = viewport.rect.center;
        //Vector2 itemCenter = item.rect.center;

        //Vector2 offset = new Vector2(itemCenter.x - viewportCenter.x, 0);
        //content.anchoredPosition -= offset;
    }

    void UpdateFocusText(RectTransform focusedItem)
    {
        int index = focusedItem.GetSiblingIndex() + 1;
        focusText.text = $"{index}/{totalItems}";
    }
}
