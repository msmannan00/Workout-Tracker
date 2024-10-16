using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUISetting : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> headingText = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TextMeshProUGUI> simpleText = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TMP_InputField> inputFields= new List<TMP_InputField>();
    [SerializeField]
    private List<Image> buttons = new List<Image>();

    private void OnEnable()
    {
        switch(ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Light:
                break;
            case Theme.Dark:
                break;
        }
    }
}
