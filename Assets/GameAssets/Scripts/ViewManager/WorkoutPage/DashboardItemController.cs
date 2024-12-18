using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DashboardItemController : MonoBehaviour, ItemController
{
    public TMP_Text workoutNameText;
    //public TMP_InputField editWorkoutName;
    public Transform exerciseParent;
    public Button playButton;
    public Button editButton;
    public Image spriteImage;
    public Sprite darkTheme, lightTheme;
    public DefaultTempleteModel defaultTempleteModel;
    private Action<object> callback;
    List<TextMeshProUGUI> exerciseText=new List<TextMeshProUGUI>();
    private GameObject parent;
    public void onInit(Dictionary<string, object> data, Action<object> callback = null)
    {
        this.callback = callback;
        defaultTempleteModel = (DefaultTempleteModel)data["data"];
        parent = (GameObject)data["parent"];
        workoutNameText.text = userSessionManager.Instance.FormatStringAbc(defaultTempleteModel.templeteName);
        //editWorkoutName.textComponent.text = defaultTempleteModel.templeteNotes;
        //editWorkoutName.onEndEdit.AddListener(OnNameChanged);
        playButton.onClick.AddListener(PlayButton);
        playButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        //editButton.onClick.AddListener(EditWorkoutName);
        editButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        editButton.onClick.AddListener(EditWorkout);


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
            textMeshPro.fontStyle = FontStyles.Normal;
            textMeshPro.alignment = TextAlignmentOptions.Left;
            exerciseText.Add(textMeshPro);
            SetColor(textMeshPro);
            // textMeshPro.margin = new Vector4(20, 0, 0, 0); // Optional
        }
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
    private void OnEnable()
    {
        foreach(TextMeshProUGUI text in exerciseText)
        {
            SetColor(text);
        }
    }
    public void EditWorkout()
    {
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
                { "templeteModel", defaultTempleteModel},
                { "editWorkout", true }
            };
        StateManager.Instance.OpenStaticScreen("createWorkout", parent, "createNewWorkoutScreen", mData, true, null, true);
        StateManager.Instance.CloseFooter();
    }
    public void PlayButton()
    {
        callback?.Invoke(defaultTempleteModel);
        StateManager.Instance.CloseFooter();
    }
    //public void EditWorkoutName()
    //{
    //    workoutNameText.gameObject.SetActive(false);
    //    editWorkoutName.gameObject.SetActive(true);
    //    editWorkoutName.ActivateInputField();
    //    editWorkoutName.text = workoutNameText.text;
    //    editButton.gameObject.SetActive(false);
    //}
    //public void OnNameChanged(string name)
    //{
    //    if (string.IsNullOrWhiteSpace(name))
    //    {
    //        workoutNameText.gameObject.SetActive(true);
    //        editWorkoutName.gameObject.SetActive(false);
    //        editButton.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        defaultTempleteModel.templeteName = name.ToUpper();
    //        workoutNameText.text = name.ToUpper();
    //        //float textWidth = workoutNameText.preferredWidth;
    //        //workoutNameText.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(textWidth, workoutNameText.transform.GetComponent<RectTransform>().sizeDelta.y);
    //        workoutNameText.gameObject.SetActive(true);
    //        editWorkoutName.gameObject.SetActive(false);
    //        editButton.gameObject.SetActive(true);
    //        ApiDataHandler.Instance.SaveTemplateData();
    //    }
    //}
    public void SetColor(TextMeshProUGUI text)
    {
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                text.color = new Color(150, 0, 0, 255);
                text.font = userSessionManager.Instance.lightSecondaryFont;
                break;
            case Theme.Dark:
                text.color = new Color(255, 255, 255, 255);
                text.font = userSessionManager.Instance.darkSecondaryFont;
                break;
        }
    }
}
