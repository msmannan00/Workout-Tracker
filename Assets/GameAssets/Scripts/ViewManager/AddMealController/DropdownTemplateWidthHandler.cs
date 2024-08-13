using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownTemplateWidthHandler : MonoBehaviour
{
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SetWidthToScreenWidth();
        PositionAtScreenStart();
    }

    void SetWidthToScreenWidth()
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width/3);
    }

    void PositionAtScreenStart()
    {
        rectTransform.anchoredPosition = new Vector2(0, rectTransform.anchoredPosition.y);
    }
}
