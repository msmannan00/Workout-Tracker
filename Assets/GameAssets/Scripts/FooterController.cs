using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FooterController : MonoBehaviour
{
    public GameObject selfButtonObject;
    public Image body, sharing, dashboard, history, profile;
    public FooterButtons currentButton;
    //public List<Image> footerButtonImages = new List<Image>();

    private void Start()
    {
        body.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.Body));
        sharing.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.Share));
        dashboard.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.Dashboard));
        history.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.History));
        profile.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.Profile));

        // add sound
        body.gameObject.GetComponent<Button>().onClick.AddListener(()=>AudioController.Instance.OnButtonClick());
        sharing.gameObject.GetComponent<Button>().onClick.AddListener(()=>AudioController.Instance.OnButtonClick());
        dashboard.gameObject.GetComponent<Button>().onClick.AddListener(()=>AudioController.Instance.OnButtonClick());
        history.gameObject.GetComponent<Button>().onClick.AddListener(()=>AudioController.Instance.OnButtonClick());
        profile.gameObject.GetComponent<Button>().onClick.AddListener(()=>AudioController.Instance.OnButtonClick());

        BottomButtonSelectionSeter(FooterButtons.Dashboard);
    }
    private void OnEnable()
    {
        //BottomButtonSelectionSeter(dashboard.gameObject);
    }

    public void BottomButtonSelectionSeter(FooterButtons button)
    {
        if(currentButton == button) return;  
        switch (button)
        {
            case FooterButtons.Body:
                OnBody();
                currentButton = button;
                SetCollors(body.gameObject);
                break;
            case FooterButtons.History:
                OnHistory();
                currentButton = button;
                SetCollors(history.gameObject);
                break;
            case FooterButtons.Profile:
                currentButton = button;
                OnProfile();
                SetCollors(profile.gameObject);
                break;
            case FooterButtons.Dashboard:
                OnDashboard();
                currentButton = button;
                SetCollors(dashboard.gameObject);
                break;
            case FooterButtons.Share:
                OnSocial();
                currentButton = button;
                SetCollors(sharing.gameObject);
                break;
        }
    }
    void SetCollors(GameObject clickedObject)
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
    {print("dashboard");
        StateManager.Instance.OpenStaticScreen("dashboard", userSessionManager.Instance.currentScreen, "dashboardScreen", null,isfooter:true);
    }
    public void OnHistory()
    {print("history");
        StateManager.Instance.OpenStaticScreen("history", userSessionManager.Instance.currentScreen, "historyScreen", null, isfooter: true);
    }
    public void OnProfile()
    {
        print("profile");
        StateManager.Instance.OpenStaticScreen("profile", userSessionManager.Instance.currentScreen, "profileScreen", null, isfooter: true);
    }
    public void OnSocial()
    {
        print("social");
        StateManager.Instance.OpenStaticScreen("social", userSessionManager.Instance.currentScreen, "socialScreen", null, isfooter: true);
    }
    public void OnBody()
    {
        print("body");
        StateManager.Instance.OpenStaticScreen("character", userSessionManager.Instance.currentScreen, "characterScreen", null, isfooter: true);
    }
}
