using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactUsScreen : MonoBehaviour, PageController
{
    public Button backButton;
 
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        backButton.onClick.AddListener(OnBack);
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);

    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            OnBack();
        }
    }
    public void OnBack()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }

}
