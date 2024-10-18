using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUISetting : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> primaryText = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TextMeshProUGUI> secondaryText = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TMP_InputField> inputFields= new List<TMP_InputField>();
    [SerializeField]
    private List<Image> buttons = new List<Image>();
    [SerializeField]
    private List<Outline> outlines = new List<Outline>();

    private void OnEnable()
    {
        SetTheme();
    }
    public void SetTheme()
    {
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                foreach(TextMeshProUGUI text in primaryText)
                {
                    text.font = userSessionManager.Instance.lightPrimaryFont;
                }
                foreach (TextMeshProUGUI text in secondaryText)
                {
                    text.font = userSessionManager.Instance.lightSecondaryFont;
                }
                break;
            case Theme.Dark:
                foreach (TextMeshProUGUI text in primaryText)
                {
                    text.font = userSessionManager.Instance.darkPrimaryFont;
                }
                foreach (TextMeshProUGUI text in secondaryText)
                {
                    text.font = userSessionManager.Instance.darkSecondaryFont;
                }
                break;
        }
    }

}
