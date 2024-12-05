using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockInputWhileScrolling : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect; // Reference to your ScrollRect
    public CanvasGroup contentCanvasGroup; // CanvasGroup for the scrollable content (child of Viewport)

    public void OnBeginDrag(PointerEventData eventData)
    {
        contentCanvasGroup.blocksRaycasts = false; // Block interaction with child elements
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        contentCanvasGroup.blocksRaycasts = true; // Allow interaction with child elements
    }
}
