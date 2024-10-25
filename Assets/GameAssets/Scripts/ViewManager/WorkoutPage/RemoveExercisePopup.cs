using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveExercisePopup : MonoBehaviour,IPrefabInitializer
{
    public Button fade;
    public Button removeExercise;
    Action<List<object>> callback;

    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        callback = onFinish;
        //workoutScreen = (GameObject)data[0];
        //modifiedModel = (DefaultTempleteModel)data[1];
        //this.callback = (Action<object>)data[2];
        //messageText.text = (string)data[3];
        //isTemplateCreator = (bool)data[4];
        //historyData = (HistoryTempleteModel)data[5];
    }
    private void Start()
    {
        fade.onClick.AddListener(Closs);
        removeExercise.onClick.AddListener(RemoveExercise);
    }
    public void Closs()
    {
        PopupController.Instance.ClosePopup("RemoveExercisePopup");
    }
    public void RemoveExercise()
    {
        callback?.Invoke(null);
        Closs();
    }
}
