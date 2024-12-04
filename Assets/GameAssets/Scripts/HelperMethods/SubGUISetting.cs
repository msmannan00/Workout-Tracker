using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SubGUISetting : MonoBehaviour
{
    public TextMeshProUGUI exerciseText;
    public TextMeshProUGUI categoryText;
    public List<TextMeshProUGUI> historyText;
    public Shadow shadow;
    private void OnEnable()
    {
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                if (shadow != null)
                    shadow.enabled = true;
                SetExerciseText(userSessionManager.Instance.lightPrimaryFontMediumBold, userSessionManager.Instance.lightButtonTextColor);
                SetCategoryText(userSessionManager.Instance.lightPrimaryFontBold, userSessionManager.Instance.lightPlaceholder);
                SetHistoryText(userSessionManager.Instance.lightSecondaryFont, new Color32(92, 59, 28, 155));
                break;
            case Theme.Dark:
                if (shadow != null)
                    shadow.enabled = false;
                SetExerciseText(userSessionManager.Instance.darkPrimaryFont, Color.white);
                SetCategoryText(userSessionManager.Instance.lightPrimaryFontBold, new Color32(255, 255, 255, 150));
                SetHistoryText(userSessionManager.Instance.darkSecondaryFont, new Color32(217, 217, 217, 127));
                break;
        }
    }
    public void SetExerciseText(TMP_FontAsset font,Color color)
    {
        if (exerciseText != null)
        {
            exerciseText.font = font;
            exerciseText.color = color;
        }
    }
    public void SetCategoryText(TMP_FontAsset font, Color color)
    {
        if (categoryText != null)
        {
            categoryText.font = font;
            categoryText.color = color;
        }
    }
    public void SetHistoryText(TMP_FontAsset font, Color color)
    {
        foreach(TextMeshProUGUI text in historyText)
        {
            text.font = font;
            text.color = color;
        }
    }
}
