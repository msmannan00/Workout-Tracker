using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightController : MonoBehaviour,PageController
{
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {

    }
    public void Next()
    {
        StateManager.Instance.OpenStaticScreen("loading", gameObject, "loadingScreen", null);
    }
}
