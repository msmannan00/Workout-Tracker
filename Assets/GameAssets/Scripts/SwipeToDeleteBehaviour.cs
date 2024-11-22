using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class SwipeToDeleteBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform topView;
    public bool isSubItem;
    private float smoothSpeed = 10f; 

    private Vector3 mouseStartPosition;
    private Vector3 topViewStartPosition;

    private float horizontalDragMinDistance = -30f;
    private float verticalDragMinDistance = 8f;

    private bool horizontalDrag = false;
    private bool verticalDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseStartPosition = Input.mousePosition;
        topViewStartPosition = topView.localPosition;
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 distance = Input.mousePosition - mouseStartPosition;

        if (!horizontalDrag && !verticalDrag)
        {
            if (distance.x < horizontalDragMinDistance)
            {
                horizontalDrag = true;
                return;
            }
            else if (Mathf.Abs(distance.y) > verticalDragMinDistance)
            {
                verticalDrag = true;
                return;
            }
        }

        if (horizontalDrag)
        {
            float targetX = topViewStartPosition.x + Mathf.Min(0f, distance.x);
            targetX = targetX / 2;
            float smoothX = Mathf.Lerp(topView.localPosition.x, targetX, Time.deltaTime * smoothSpeed);
            topView.localPosition = new Vector3(smoothX, topViewStartPosition.y, topViewStartPosition.z);
        }
        else if (verticalDrag)
        {
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (horizontalDrag)
        {
            Vector3 distance = Input.mousePosition - mouseStartPosition;

            if (distance.x < -400.0f)
            {
                if (isSubItem)
                {
                    workoutLogScreenDataModel workoutModel = transform.parent.parent.GetComponent<workoutLogScreenDataModel>();
                    int index = workoutModel.workoutLogSubItems.IndexOf(this.GetComponent<WorkoutLogSubItem>());
                    workoutModel.workoutLogSubItems.Remove(this.GetComponent<WorkoutLogSubItem>());
                    workoutModel.exerciseTypeModel.exerciseModel.RemoveAt(index);

                    for (int i = 0; i < workoutModel.workoutLogSubItems.Count; i++)
                    {
                        workoutModel.workoutLogSubItems[i].sets.text = (i + 1).ToString();
                    }
                }
                else
                {
                    workoutLogScreenDataModel workoutModel = this.GetComponent<workoutLogScreenDataModel>();
                    workoutModel.templeteModel.exerciseTemplete.Remove(workoutModel.exerciseTypeModel);
                }
                Destroy(gameObject);
            }
            else
            {
                //topView.localPosition = topViewStartPosition;
                StartCoroutine(SmoothReset());
            }
        }

        horizontalDrag = false;
        verticalDrag = false;
        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
    }
    public float resetSmoothTime = 0.2f;
    private IEnumerator SmoothReset()
    {
        Vector3 startPosition = topView.localPosition; // Current position
        Vector3 endPosition = topViewStartPosition;   // Target reset position

        float elapsedTime = 0f;

        while (elapsedTime < resetSmoothTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / resetSmoothTime;

            // Smooth interpolation using Lerp
            topView.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }

        // Snap to the final position to avoid precision errors
        topView.localPosition = endPosition;
    }
}
