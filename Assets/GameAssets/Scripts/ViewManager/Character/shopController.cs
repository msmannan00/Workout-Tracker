using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopController : MonoBehaviour, PageController
{
    public Button backButton;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {

    }
    private void Start()
    {
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null, null, false);
    }
}