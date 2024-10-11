using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour, PageController
{
    public RectTransform selectionLine;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {

    }
    private void Start()
    {
        Rank();
    }

    public void Rank()
    {
        GlobalAnimator.Instance.AnimateRectTransformX(selectionLine, -100f, 0.25f);
    }
    public void Milestone()
    {
        GlobalAnimator.Instance.AnimateRectTransformX(selectionLine, 100f, 0.25f);
    }

    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null, null, false);
    }
}