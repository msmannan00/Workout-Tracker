using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;

public class DashboardItemController : MonoBehaviour, ItemController
{
    public TMP_Text templateName;
    public GameObject exerciseNamePrefab;
    public GameObject exerciseParent;
    public DefaultTempleteModel defaultTempleteModel;
    public Button createButton;
    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        defaultTempleteModel = (DefaultTempleteModel)data["data"];
        templateName.text = defaultTempleteModel.templeteName;

        foreach (var exercise in defaultTempleteModel.exerciseTemplete)
        {
            ExerciseTypeModel exerciseData = exercise;
            GameObject textLabelObject = new GameObject($"Exercise_{exerciseData.name}");
            //GameObject textLabelObject = Instantiate(exerciseNamePrefab, exerciseParent.transform);


            textLabelObject.transform.SetParent(templateName.transform, false);

            TextMeshProUGUI textMeshPro = textLabelObject.AddComponent<TextMeshProUGUI>();
            //TextMeshProUGUI textMeshPro = textLabelObject.GetComponent<TextMeshProUGUI>();
            textMeshPro.text = exerciseData.name.ToString();
            textMeshPro.fontSize = 17;
            textMeshPro.color = Color.white;
            textMeshPro.fontStyle = FontStyles.Normal;
            textMeshPro.alignment = TextAlignmentOptions.Left;
            // textMeshPro.margin = new Vector4(20, 0, 0, 0); // Optional
        }
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
