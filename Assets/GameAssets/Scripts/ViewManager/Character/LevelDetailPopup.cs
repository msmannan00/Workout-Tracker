using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDetailPopup : MonoBehaviour, IPrefabInitializer
{
    public Button fadeButton;
    
    void IPrefabInitializer.InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        fadeButton.onClick.AddListener(OffPopup);
    }
    public void OffPopup()
    {
        PopupController.Instance.ClosePopup("levelDetailPopup");
    }
}
