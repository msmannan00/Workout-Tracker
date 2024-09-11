using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class DashboardItemController : MonoBehaviour, ItemController
{
    public TMP_Text templateName;
    public Transform exerciseParent;
    public Image line, edit;
    public Button playButton;
    public Image spriteImage;
    public Sprite darkTheme, lightTheme;
    public DefaultTempleteModel defaultTempleteModel;
    private Action<object> callback;
    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        this.callback = callback;
        defaultTempleteModel = (DefaultTempleteModel)data["data"];
        templateName.text = defaultTempleteModel.templeteName;
        playButton.onClick.AddListener(PlayButton);
        TMP_FontAsset exerciseFont = null;
        Color exerciseColor = Color.white;
        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Dark:
                exerciseFont= userSessionManager.Instance.darkTextFont;
                exerciseColor= Color.white;
                spriteImage.sprite= darkTheme;
                templateName.font = userSessionManager.Instance.darkHeadingFont;
                templateName.color = Color.white;
                line.color = Color.white;
                edit.color = Color.white;
                playButton.GetComponent<Image>().color=Color.white;
                playButton.transform.GetChild(0).GetComponent<Image>().color= new Color32(51, 23, 23, 255);
                break;
            case Theme.Light:
                exerciseFont = userSessionManager.Instance.lightTextFont;
                exerciseColor = userSessionManager.Instance.lightHeadingColor;
                spriteImage.sprite = lightTheme;
                templateName.font = userSessionManager.Instance.lightHeadingFont;
                templateName.color = exerciseColor;
                line.color = userSessionManager.Instance.lightTextColor;
                edit.color = Color.red;
                playButton.GetComponent<Image>().color = Color.red;
                playButton.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                break;
        }
        foreach (var exercise in defaultTempleteModel.exerciseTemplete)
        {
            ExerciseTypeModel exerciseData = exercise;
            GameObject textLabelObject = new GameObject($"Exercise_{exerciseData.name}");
            //GameObject textLabelObject = Instantiate(exerciseNamePrefab, exerciseParent.transform);

            textLabelObject.transform.SetParent(exerciseParent, false);

            TextMeshProUGUI textMeshPro = textLabelObject.AddComponent<TextMeshProUGUI>();
            textLabelObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 20);
            //TextMeshProUGUI textMeshPro = textLabelObject.GetComponent<TextMeshProUGUI>();
            textMeshPro.text = exerciseData.name + " x " + exerciseData.exerciseModel.Count.ToString();
            textMeshPro.fontSize = 16;
            textMeshPro.color = exerciseColor;
            textMeshPro.font = exerciseFont;
            textMeshPro.fontStyle = FontStyles.Normal;
            textMeshPro.alignment = TextAlignmentOptions.Left;
            // textMeshPro.margin = new Vector4(20, 0, 0, 0); // Optional
        }
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
    public void PlayButton()
    {
        callback?.Invoke(defaultTempleteModel);
    }
}
