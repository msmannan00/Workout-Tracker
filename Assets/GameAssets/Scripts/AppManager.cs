using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Ensure DOTween is installed

public class AppManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image internet; // Assign the "No Internet" icon here

    private bool isInternetErrorShown = false;

    private void Start()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            Screen.fullScreen = false;
            AndroidUtility.ShowStatusBar(new Color32(0, 0, 0, 255));
        #endif

        Application.targetFrameRate = 60;

        FirebaseManager.Instance.Load(OpenScreen);

        // Ensure the internet image is disabled at the start
        if (internet != null)
        {
            internet.gameObject.SetActive(false);
        }

        // Start monitoring internet connectivity
        StartCoroutine(CheckInternetConnection());
    }

    public void OpenScreen()
    {
        StartCoroutine(StartWait());
    }

    public void OnLogin()
    {
        PreferenceManager.Instance.SetBool("WelcomeScreensShown_v3", true);
        Dictionary<string, object> mData = new Dictionary<string, object>
        {
            { AuthKey.sAuthType, AuthConstant.sAuthTypeLogin }
        };
        StateManager.Instance.OpenStaticScreen("auth", null, "authScreen", mData);
    }

    IEnumerator StartWait()
    {
        yield return new WaitForSeconds(1);
        OnLogin();
    }

    IEnumerator CheckInternetConnection()
    {
        while (true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (!isInternetErrorShown)
                {
                    ShowInternetError();
                }
            }
            else
            {
                if (isInternetErrorShown)
                {
                    HideInternetError();
                }
            }
            yield return new WaitForSeconds(2); // Check every 2 seconds
        }
    }

    void ShowInternetError()
    {
        isInternetErrorShown = true;

        if (internet != null)
        {
            internet.gameObject.SetActive(true);

            // Start blinking the image using DOTween
            internet.DOFade(0.8f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

    void HideInternetError()
    {
        isInternetErrorShown = false;

        if (internet != null)
        {
            // Stop blinking and reset the alpha
            internet.DOKill();
            internet.color = new Color(internet.color.r, internet.color.g, internet.color.b, 1f);
            internet.gameObject.SetActive(false);
        }
    }
}
