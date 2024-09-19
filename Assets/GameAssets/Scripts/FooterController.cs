using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FooterController : MonoBehaviour
{
    public GameObject selfButtonObject;
    public List<Image> footerButtonImages = new List<Image>();
    private void OnEnable()
    {
        BottomButtonSelectionSeter(selfButtonObject);
    }

    public void BottomButtonSelectionSeter(GameObject clickedObject)
    {
        switch (userSessionManager.Instance.gameTheme)
        {
            case Theme.Dark:
                foreach (Image img in footerButtonImages)
                {
                    if (img.gameObject == clickedObject)
                    {
                        img.color = Color.red;
                        img.enabled = true;
                        foreach (Transform child in img.gameObject.transform)
                        {
                            child.GetComponent<Image>().color = Color.white;
                        }
                    }
                    else
                        img.enabled = false;
                }
                break;
            case Theme.Light:
                foreach (Image img in footerButtonImages)
                {
                    if (img.gameObject == clickedObject)
                    {
                        foreach (Transform child in img.gameObject.transform)
                        {
                            child.GetComponent<Image>().color = Color.red;
                        }
                    }

                    else
                    {
                        foreach (Transform child in img.gameObject.transform)
                        {
                            child.GetComponent<Image>().color = Color.white;
                        }
                    }
                }
                break;
        }
    }
    public void OnDashboard()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("dashboard", null, "dashboardScreen", mData);
    }
    public void OnHistory()
    {
        StateManager.Instance.OpenStaticScreen("history", null, "historyScreen", null,true);
    }
    public void OnProfile()
    {

    }
    public void OnSocial()
    {

    }
    public void OnBody()
    {

    }
}
