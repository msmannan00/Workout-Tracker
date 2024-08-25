using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashboardItemController : MonoBehaviour, ItemController
{
    public TMP_Text templateName;
    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        DefaultTempleteModel defaultTempleteModel = (DefaultTempleteModel)data["data"];
        templateName.text = defaultTempleteModel.templeteName;
    }
}
