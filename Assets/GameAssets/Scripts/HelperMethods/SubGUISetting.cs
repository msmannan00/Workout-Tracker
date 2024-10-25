using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SubGUISetting : MonoBehaviour
{
    public TextMeshProUGUI exerciseText;
    public TextMeshProUGUI categoryText;
    private void OnEnable()
    {
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                exerciseText.font = userSessionManager.Instance.lightPrimaryFont;
                exerciseText.color = userSessionManager.Instance.lightButtonTextColor;
                categoryText.font=userSessionManager.Instance.lightPrimaryFont;
                categoryText.color=userSessionManager.Instance.lightPlaceholder;
                break;
            case Theme.Dark:
                exerciseText.font = userSessionManager.Instance.darkPrimaryFont;
                exerciseText.color = Color.white;
                categoryText.font = userSessionManager.Instance.lightPrimaryFont;
                categoryText.color = new Color32(255, 255, 255, 150);
                break;
        }
    }
}
