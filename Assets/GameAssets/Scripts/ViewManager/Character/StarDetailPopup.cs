using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarDetailPopup : MonoBehaviour,IPrefabInitializer
{
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI coinText;
    public Button continouButton, fadeButton;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        descriptionText.text = (string)data[0];
        coinText.text = (string)data[1];
        continouButton.onClick.AddListener(ClossPopup);
        fadeButton.onClick.AddListener(ClossPopup);
    }
    public void ClossPopup()
    {
        PopupController.Instance.ClosePopup("StarDetailPopup");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
