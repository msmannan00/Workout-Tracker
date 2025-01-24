using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendPresonalBestController : MonoBehaviour, PageController
{
    public Transform content;
    public Button backButton;
    public PersonalBestData _data;
    public List<string> haveExercises = new List<string>();
    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        _data = (PersonalBestData)data["data"];
        LoadData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    private void Start()
    {
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        backButton.onClick.AddListener(Back);
    }
    public void LoadData()
    {
        foreach (PersonalBestDataItem item in _data.exercises)
        {
            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/profile/personalBestSubItem");
            GameObject exerciseObject = Instantiate(exercisePrefab, content);
            Dictionary<string, object> mData = new Dictionary<string, object>
            {
                { "data", item }, { "social", true }
            };
            exerciseObject.GetComponent<ItemController>().onInit(mData, null);
            haveExercises.Add(item.exerciseName.ToLower());
        }
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        //StateManager.Instance.OpenFooter(null, null, false);
    }
}
