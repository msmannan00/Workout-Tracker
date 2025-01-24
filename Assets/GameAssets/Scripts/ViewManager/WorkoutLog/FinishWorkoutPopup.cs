using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishWorkoutPopup : MonoBehaviour,IPrefabInitializer
{
    public TextMeshProUGUI messageText;
    public Button saveButton, discardButton, fade;
    public GameObject workoutScreen;
    private bool isTemplateCreator;
    private DefaultTempleteModel modifiedModel;
    private HistoryTempleteModel historyData;
    Action<List<object>> callback;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {
        callback = onFinish;
        workoutScreen = (GameObject)data[0];
        modifiedModel = (DefaultTempleteModel)data[1];
        this.callback = (Action<object>)data[2];
        messageText.text = (string)data[3];
        isTemplateCreator = (bool)data[4];
        historyData = (HistoryTempleteModel)data[5];
    }
    private void Start()
    {
        saveButton.onClick.AddListener(Save);
        saveButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        discardButton.onClick.AddListener(Discard);
        discardButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        fade.onClick.AddListener(Discard);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Discard();
        }
    }
    void Discard()
    {
        callback?.Invoke(null);
        PopupController.Instance.ClosePopup("FinishWorkoutPopup");
        //StateManager.Instance.HandleBackAction(workoutScreen);
        //StateManager.Instance.OpenFooter(null, null, false);
    }
    void Save()
    {
        if (historyData.exerciseTypeModel.Count > 0)
        {
            ApiDataHandler.Instance.SaveHistory(historyData,ApiDataHandler.Instance.getHistoryData().exerciseTempleteModel.Count);
            ApiDataHandler.Instance.AddItemToHistoryData(historyData);
        }
        if (isTemplateCreator)
        {
            modifiedModel.exerciseTemplete.RemoveAll(model => model.exerciseModel.Count == 0);
            if (modifiedModel.exerciseTemplete.Count > 0)
            {
                ApiDataHandler.Instance.AddExerciseTemplate(modifiedModel,ApiDataHandler.Instance.getTemplateData().exerciseTemplete.Count);
                ApiDataHandler.Instance.AddItemToTemplateData(modifiedModel);
            }
        }
        
        //int index = GetIndexByTempleteName(modifiedModel.templeteName);
        //ApiDataHandler.Instance.RemoveItemFromTempleteData(index);
        //ApiDataHandler.Instance.InsertItemToTemplateData(index, modifiedModel);//.exerciseTemplete.Insert(index, modifiedModel);
        //ApiDataHandler.Instance.SaveTemplateData();

        PopupController.Instance.ClosePopup("FinishWorkoutPopup");


        List<object> initialData = new List<object> { historyData, workoutScreen };
        PopupController.Instance.OpenPopup("complete", "completeWorkoutPopup", null, initialData);
        if(workoutScreen!=null)
            Destroy(workoutScreen);
        //StateManager.Instance.onRemoveBackHistory();
        //Dictionary<string, object> mData = new Dictionary<string, object>
        //        {
        //            { "history", historyData }, { "workoutScreen", workoutScreen }
        //        };
        //StateManager.Instance.OpenStaticScreen("complete", workoutScreen, "completeWorkoutPopup", mData);
        //StateManager.Instance.HandleBackAction(workoutScreen);
        
    }

    public int GetIndexByTempleteName(string name)
    {
        return ApiDataHandler.Instance.getTemplateData().exerciseTemplete.FindIndex(t => t.templeteName == name);
    }
}
