using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUnitPopup : MonoBehaviour,IPrefabInitializer
{
    public Toggle kg, lbs;
    public Button offPopupButtons, saveButton;
    public void InitPrefab(Action<List<object>> onFinish, List<object> data)
    {

    }
    void Start()
    {

        switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
        {
            case WeightUnit.kg:
                kg.isOn = true;
                kg.targetGraphic.color = new Color32(251, 127, 127, 255);
                lbs.isOn = false;
                kg.interactable = false;
                lbs.interactable = true;
                break;
            case WeightUnit.lbs:
                lbs.isOn = true;
                lbs.targetGraphic.color = new Color32(251, 127, 127, 255);
                kg.isOn = false;
                kg.interactable = true;
                lbs.interactable = false;
                break;
        }
        kg.onValueChanged.AddListener(OnKgChanged);
        lbs.onValueChanged.AddListener(OnLbsChanged);
        offPopupButtons.onClick.AddListener(OffPopup);
        saveButton.onClick.AddListener(Save);
    }
    public void Save()
    {
        if(kg.isOn)
        {
            ApiDataHandler.Instance.SetWeightUnit((int)WeightUnit.kg);
        }
        else if (lbs.isOn)
        {
            ApiDataHandler.Instance.SetWeightUnit((int)WeightUnit.lbs);
        }
        OffPopup();
    }
    public void OffPopup()
    {
        PopupController.Instance.ClosePopup("ChangeUnitPopup");
    }
    private void OnKgChanged(bool isOn)
    {
        if (isOn)
        {
            lbs.isOn = false;
            lbs.targetGraphic.color = Color.white;
            kg.interactable = false;
            lbs.interactable = true;
        }
        kg.targetGraphic.color = new Color32(251, 127, 127, 255);
    }

    private void OnLbsChanged(bool isOn)
    {
        if (isOn)
        {
            kg.isOn = false;
            kg.targetGraphic.color= Color.white;
            kg.interactable = true;
            lbs.interactable = false;
        }
        lbs.targetGraphic.color = new Color32(251, 127, 127, 255);
    }
}
