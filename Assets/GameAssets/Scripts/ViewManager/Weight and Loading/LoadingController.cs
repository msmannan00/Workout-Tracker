using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadingController : MonoBehaviour,PageController
{
    public RectTransform imageRectTransform;  // Assign the RectTransform of the image
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {

    }

    void Start()
    {
        ApiDataHandler.Instance.LoadDataFromFirebase();
        imageRectTransform.DOSizeDelta(new Vector2(200f, imageRectTransform.sizeDelta.y), 5f).OnComplete(() => {
            //StateManager.Instance.OpenStaticScreen("dashboard", gameObject, "dashboardScreen", null, isfooter: true);
            StateManager.Instance.OpenFooter("shared", "footer",true);
        });
    }
}
