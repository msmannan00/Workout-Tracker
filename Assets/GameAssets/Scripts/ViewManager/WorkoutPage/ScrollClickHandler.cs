using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollClickHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IDragHandler
{
    public bool isScrollingHorizontal;
    public bool isScrollingVertical;
    public ScrollRect parentScrollRect; // Parent ScrollRect for horizontal scrolling
    public DashboardItemController dashboardItemController;

    private void Start()
    {
        if (dashboardItemController != null)
        {
            parentScrollRect = dashboardItemController.parentScroll; // Initialize parent scroll rect
        }
        else
        {
            Debug.LogError("DashboardItemController is not assigned!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("onbegin");
        // Determine the drag direction
        if (Mathf.Abs(eventData.delta.y) > Mathf.Abs(eventData.delta.x))
        {
            isScrollingHorizontal = false; // Vertical scrolling
            isScrollingVertical = true;
        }
        else
        {
            isScrollingHorizontal = true; // Horizontal scrolling
            isScrollingVertical = false;
        }

        // Pass drag events to the parent if it's horizontal scrolling
        if (isScrollingHorizontal && parentScrollRect != null)
        {
            parentScrollRect.OnBeginDrag(eventData);
            parentScrollRect.gameObject.GetComponent<ScrollSnapRect>().OnBeginDrag(eventData);
            print("scroll hor");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("onend drag");
        // Reset scrolling flags
        if (isScrollingHorizontal && parentScrollRect != null)
        {
            print("scroll hor");
            parentScrollRect.OnEndDrag(eventData);
            parentScrollRect.gameObject.GetComponent<ScrollSnapRect>().OnEndDrag(eventData);
        }

        // Reset both flags
        isScrollingHorizontal = false;
        isScrollingVertical = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("on drag");
        if (isScrollingHorizontal && parentScrollRect != null)
        {
            print("scroll hor");
            parentScrollRect.OnDrag(eventData); // Pass horizontal scroll to parent
            parentScrollRect.gameObject.GetComponent<ScrollSnapRect>().OnDrag(eventData);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("on click");
        // Only handle button click if no scrolling occurred
        if (!isScrollingHorizontal && !isScrollingVertical)
        {
            if (dashboardItemController != null)
            {
                dashboardItemController.PlayButton(); // Call the button logic
            }
            else
            {
                Debug.LogError("DashboardItemController is not assigned!");
            }
        }
    }
}