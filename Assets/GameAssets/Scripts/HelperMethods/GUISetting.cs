using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUISetting : MonoBehaviour
{
    public Image background;
    public Image playButton;
    [SerializeField]
    private List<TextMeshProUGUI> primaryText = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TextMeshProUGUI> secondaryText = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TextMeshProUGUI> buttonText = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TMP_InputField> inputFields= new List<TMP_InputField>();
    [SerializeField]
    private List<Image> inputFieldSearchIcon= new List<Image>();
    [SerializeField]
    private List<Image> buttons = new List<Image>();
    [SerializeField]
    private List<Image> button2 = new List<Image>();
    [SerializeField]
    private List<Image> line = new List<Image>();
    [SerializeField]
    private List<Outline> outlines = new List<Outline>();
    public TextMeshProUGUI switchButton1,switchButton2;

    public Sprite darkbackground, lightbackground;
    public Sprite lightPlay, darkPlay;
    private void OnEnable()
    {
        SetTheme();
    }
    public void SetTheme()
    {
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                if (playButton != null)
                    playButton.sprite = lightPlay;
                setBackground(lightbackground);
                SetPrimaryText(userSessionManager.Instance.lightPrimaryFont, userSessionManager.Instance.lightButtonTextColor);
                SetSecondaryText(userSessionManager.Instance.lightSecondaryFont);
                SetButtonColor(userSessionManager.Instance.lightButtonColor);
                SetButtonTextColor(Color.white);
                SetButton2Color(userSessionManager.Instance.lightInputFieldColor);
                SetInputFields(userSessionManager.Instance.lightSecondaryFont, userSessionManager.Instance.lightButtonTextColor, userSessionManager.Instance.lightPlaceholder,userSessionManager.Instance.lightInputFieldColor);
                break;
            case Theme.Dark:
                if(playButton != null)
                    playButton.sprite = darkPlay;
                setBackground(darkbackground);
                SetPrimaryText(userSessionManager.Instance.darkPrimaryFont, Color.white);
                SetSecondaryText(userSessionManager.Instance.darkSecondaryFont);
                SetButtonColor(userSessionManager.Instance.darkButtonColor);
                SetButtonTextColor(userSessionManager.Instance.darkButtonTextColor);
                SetButton2Color(userSessionManager.Instance.darkButtonTextColor);
                SetInputFields(userSessionManager.Instance.darkSecondaryFont, new Color32(255, 255, 255, 255), userSessionManager.Instance.darkPlaceholder,userSessionManager.Instance.darkInputFieldColor);
                break;
        }
    }
    public void setBackground(Sprite sprite)
    {
        if(background != null) background.sprite = sprite;
    }
    public void SetPrimaryText(TMP_FontAsset font, Color col)
    {
        foreach (TextMeshProUGUI text in primaryText)
        {
            text.font = font;
            text.color = col;
        }
    }
    public void SetSecondaryText(TMP_FontAsset font)
    {
        foreach (TextMeshProUGUI text in secondaryText)
        {
            text.font = font;
        }
    }
    public void SetButtonTextColor(Color col)
    {
        foreach (TextMeshProUGUI text in buttonText)
        {
            text.color = col;
        }
    }
    public void SetButtonColor(Color col)
    {
        foreach(Image img in buttons)
        {
            img.color = col;
        }
    }
    public void SetButton2Color(Color col)
    {
        foreach (Image img in button2)
        {
            img.color = col;
        }
    }
    public void SetInputFields(TMP_FontAsset font,Color textCol, Color placeHolderCol,Color imageColor)
    {
        foreach(TMP_InputField inputField in inputFields)
        {
            inputField.GetComponent<Image>().color = imageColor;
            inputField.fontAsset = font;
            inputField.textComponent.color = textCol;
            inputField.placeholder.color= placeHolderCol;
        }
        foreach(Image img in inputFieldSearchIcon)
        {
            img.color = placeHolderCol;
        }
    }
}
