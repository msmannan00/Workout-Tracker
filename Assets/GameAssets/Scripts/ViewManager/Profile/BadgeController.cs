using Firebase.Database;
using System;
using System.Collections;
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
        currentBadge = badge.Replace(" ","");
    }
    public void Continu()
    {
        StartCoroutine(SetBadgeName(currentBadge));
        //ApiDataHandler.Instance.SetBadgeNameToFirebase(currentBadge);
        //Back();
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null,null,false);
    }
    public IEnumerator SetBadgeName(string name)
    {
        GlobalAnimator.Instance.FadeInLoader();
        // Build the reference path for the 'friends' node
        string path = $"users/{FirebaseManager.Instance.user.UserId}/BadgeName/";

        // Start deleting the friend from Firebase
        //var deleteTask = FirebaseDatabase.DefaultInstance.RootReference.Child(path).RemoveValueAsync();
        var dataTask = FirebaseDatabase.DefaultInstance.RootReference.Child(path).SetValueAsync(name);

        // Wait until the task completes
        yield return new WaitUntil(() => dataTask.IsCompleted);

        // Check for errors
        if (dataTask.Exception != null)
        {
            Debug.LogError("Error while saving badge: " + dataTask.Exception);
        }
        else
        {
            userSessionManager.Instance.badgeName = name;
        }
        GlobalAnimator.Instance.FadeOutLoader();
        Back();
    }
}
