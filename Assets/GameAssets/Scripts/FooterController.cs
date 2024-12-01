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
    bool isFirstTime = true;
    //public List<Image> footerButtonImages = new List<Image>();

    private void Start()
    {
        body.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.Body));
        sharing.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.Share));
        dashboard.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.Dashboard));
        history.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.History));
        profile.gameObject.GetComponent<Button>().onClick.AddListener(()=>BottomButtonSelectionSeter(FooterButtons.Profile));


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
                if (!StateManager.Instance.isProcessing)
                    OnBody(button);
                break;
            case FooterButtons.History:
                if (!StateManager.Instance.isProcessing)
                    OnHistory(button);
                break;
            case FooterButtons.Profile:
                if (!StateManager.Instance.isProcessing)
                    OnProfile(button);
                break;
            case FooterButtons.Dashboard:
                if (!StateManager.Instance.isProcessing)
                    OnDashboard(button);
                break;
            case FooterButtons.Share:
                if (!StateManager.Instance.isProcessing)
                    OnSocial(button);
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
    public void OnDashboard(FooterButtons button)
    {
        StateManager.Instance.OpenStaticScreen("dashboard", userSessionManager.Instance.currentScreen, "dashboardScreen", null, isfooter:true);
        currentButton = button;
        SetCollors(dashboard.gameObject);
        if (!isFirstTime)
        {
            AudioController.Instance.OnButtonClick();
            this.GetComponent<GlobalRawAnimator>().WobbleObject(dashboard.gameObject);
        }
        isFirstTime= false;
    }
    public void OnHistory(FooterButtons button)
    {
        StateManager.Instance.OpenStaticScreen("history", userSessionManager.Instance.currentScreen, "historyScreen", null, isfooter: true);
        currentButton = button;
        SetCollors(history.gameObject);
        AudioController.Instance.OnButtonClick();
        this.GetComponent<GlobalRawAnimator>().WobbleObject(history.gameObject);
    }
    public void OnProfile(FooterButtons button)
    {
        StateManager.Instance.OpenStaticScreen("profile", userSessionManager.Instance.currentScreen, "profileScreen", null, isfooter: true);
        currentButton = button;
        SetCollors(profile.gameObject);
        AudioController.Instance.OnButtonClick();
        this.GetComponent<GlobalRawAnimator>().WobbleObject(profile.gameObject);
    }
    public void OnSocial(FooterButtons button)
    {
        StateManager.Instance.OpenStaticScreen("social", userSessionManager.Instance.currentScreen, "socialScreen", null, isfooter: true);
        currentButton = button;
        SetCollors(sharing.gameObject);
        AudioController.Instance.OnButtonClick();
        this.GetComponent<GlobalRawAnimator>().WobbleObject(sharing.gameObject);
    }
    public void OnBody(FooterButtons button)
    {
        StateManager.Instance.OpenStaticScreen("character", userSessionManager.Instance.currentScreen, "characterScreen", null, isfooter: true);
        currentButton = button;
        SetCollors(body.gameObject);
        AudioController.Instance.OnButtonClick();
        this.GetComponent<GlobalRawAnimator>().WobbleObject(body.gameObject);
    }

}
