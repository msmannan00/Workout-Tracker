using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldManager : MonoBehaviour
{
    public List<TMP_InputField> inputFields; // Assign all TMP InputFields here
    public ScrollRect scrollRect;            // Assign your ScrollView here
    //public Button backButton;                // Assign the dismiss keyboard button here

    private TMP_InputField activeInputField;
    private void Start()
    {
        // Link the back button to dismiss the keyboard
        //backButton.onClick.AddListener(DismissKeyboard);
    }

    private void Update()
    {
        activeInputField = inputFields.Find(field => field.isFocused);

        if (activeInputField != null)
        {
            scrollRect.enabled = true; // Enable scrolling when any input field is active
        }
    }

    private void DismissKeyboard()
    {
        if (activeInputField != null)
        {
            activeInputField.DeactivateInputField(true);
            activeInputField = null;
        }
    }
}
