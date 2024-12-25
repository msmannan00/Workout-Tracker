using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BadgeController : MonoBehaviour,PageController
{
    public Button continuButton;
    public Button backButton;
    [SerializeField]
    private string currentBadge;
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        continuButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        continuButton.onClick.AddListener(Continu);
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        backButton.onClick.AddListener(Back);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public void SetBadge(string badge)
    {
        currentBadge = badge;
    }
    public void Continu()
    {
        ApiDataHandler.Instance.SetBadgeNameToFirebase(currentBadge);
        Back();
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null,null,false);
    }
}
