using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUISetting : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> headingText = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<TextMeshProUGUI> simpleText = new List<TextMeshProUGUI>();

    private void OnEnable()
    {
        switch(userSessionManager.Instance.gameTheme)
        {
            case Theme.Light:
                break;
            case Theme.Dark:
                break;
        }
    }
}
