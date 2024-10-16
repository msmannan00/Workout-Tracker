using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FooterController : MonoBehaviour
{
    public GameObject selfButtonObject;
    public Image body, sharing, dashboard, history, profile;
    //public List<Image> footerButtonImages = new List<Image>();

    private void Start()
    {
        BottomButtonSelectionSeter(dashboard.gameObject);
    }

    public void BottomButtonSelectionSeter(GameObject clickedObject)
    {
        List<Image> buttonImages = new List<Image> { body, sharing, dashboard, history, profile };
        
        switch (ApiDataHandler.Instance.gameTheme)
        {
            case Theme.Dark:
                foreach (Image img in buttonImages)
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
                    {
                        img.enabled = false;
                        foreach (Transform child in img.gameObject.transform)
                        {
                            child.GetComponent<Image>().color = Color.white;
                        }
                    }
                }
                break;
            case Theme.Light:
                foreach (Image img in buttonImages)
                {
                    if (img.gameObject == clickedObject)
                    {
                        foreach (Transform child in img.gameObject.transform)
                        {
                            child.GetComponent<Image>().color = Color.red;
                        }
                        img.enabled = false;
                    }
                    else
                    {
                        foreach (Transform child in img.gameObject.transform)
                        {
                            child.GetComponent<Image>().color = Color.white;
                        }
                        img.enabled = false;
                    }
                }
                break;
        }
    }
    public void OnDashboard()
    {
        StateManager.Instance.OpenStaticScreen("dashboard", userSessionManager.Instance.currentScreen, "dashboardScreen", null,isfooter:true);
    }
    public void OnHistory()
    {
        StateManager.Instance.OpenStaticScreen("history", userSessionManager.Instance.currentScreen, "historyScreen", null, isfooter: true);
    }
    public void OnProfile()
    {
        StateManager.Instance.OpenStaticScreen("profile", userSessionManager.Instance.currentScreen, "profileScreen", null, isfooter: true);
    }
    public void OnSocial()
    {
        StateManager.Instance.OpenStaticScreen("social", userSessionManager.Instance.currentScreen, "socialScreen", null, isfooter: true);
    }
    public void OnBody()
    {
        StateManager.Instance.OpenStaticScreen("character", userSessionManager.Instance.currentScreen, "characterScreen", null, isfooter: true);
    }
}
