using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalAnimator : GenericSingletonClass<GlobalAnimator>
{
    public float mFadeDuration = 0.3f;

    public void FadeIn(GameObject mAppObject)
    {
        CanvasGroup mCanvasGroup = mAppObject.GetComponent<CanvasGroup>();
        if (mCanvasGroup == null)
        {
            mCanvasGroup = mAppObject.AddComponent<CanvasGroup>();
        }

        mAppObject.SetActive(true);
        mCanvasGroup.DOFade(1, mFadeDuration);
    }

    public void FadeOut(GameObject mAppObject)
    {
        CanvasGroup mCanvasGroup = mAppObject.GetComponent<CanvasGroup>();
        if (mCanvasGroup == null)
        {
            mCanvasGroup = mAppObject.AddComponent<CanvasGroup>();
        }

        mCanvasGroup.DOFade(0, mFadeDuration).OnComplete(() =>
        {
            mAppObject.SetActive(false);
        });
    }

    public void AnimateAlpha(GameObject gameObject, bool fadeIn)
    {
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = fadeIn ? 0 : 1;
        float targetAlpha = fadeIn ? 1 : 0;
        float duration = 0.25f;

        canvasGroup.DOFade(targetAlpha, duration).OnComplete(() =>
        {
            if (!fadeIn)
            {
                Destroy(gameObject);
            }
        });
    }

    public void FadeInLoader()
    {
        GameObject overlayBlockerInstance = Resources.Load<GameObject>("Prefabs/shared/UIBlocker");
        if (overlayBlockerInstance != null)
        {
            GameObject instance = UnityEngine.Object.Instantiate(overlayBlockerInstance);
            CanvasGroup canvasGroup = instance.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(0.6f, 0.35f);
        }
    }

    public void FadeOutLoader()
    {
        GameObject overlayBlockerInstance = GameObject.FindWithTag("UIBlocker");
        if (overlayBlockerInstance != null)
        {
            CanvasGroup canvasGroup = overlayBlockerInstance.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
                {
                    Destroy(overlayBlockerInstance);
                });
            }
            else
            {
                Destroy(overlayBlockerInstance);
            }
        }
    }

    public void ApplyParallax(GameObject currentPage, GameObject targetPage, Action callbackSuccess, bool keepState = false)
    {
        var currentCanvas = currentPage.GetComponent<CanvasGroup>();
        var targetCanvas = targetPage.GetComponent<CanvasGroup>();

        var overlayBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/shared/overlayBlocker"));
        overlayBlocker.transform.SetParent(currentPage.transform, false);
        overlayBlocker.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        overlayBlocker.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        overlayBlocker.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        overlayBlocker.transform.SetAsLastSibling();

        float distanceFactor = 1.5f;
        targetPage.transform.position = new Vector3(Screen.width * distanceFactor, targetPage.transform.position.y, targetPage.transform.position.z);
        targetPage.SetActive(true);
        targetCanvas.alpha = 0.3f;
        targetPage.transform.SetAsLastSibling();

        DOTween.Sequence()
            .SetDelay(0.2f)
            .OnStart(() =>
            {
                currentCanvas.interactable = false;
                targetCanvas.interactable = false;
            })
            .Append(overlayBlocker.GetComponent<Image>().DOFade(0.7f, 0.3f).SetEase(Ease.Linear))
            .Join(targetPage.transform.DOMoveX(Screen.width / 2f, 0.3f).SetEase(Ease.OutQuad))
            .Join(targetCanvas.DOFade(1f, 0.2f).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                callbackSuccess?.Invoke();
                currentPage.SetActive(false);
                if (!keepState)
                {
                    Destroy(overlayBlocker);
                }
                targetCanvas.interactable = true;
            });
    }

    public void openSidebar(GameObject targetPage)
    {
        var overlayBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/shared/overlayBlocker"));
        overlayBlocker.transform.SetParent(targetPage.transform, false);
        overlayBlocker.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        overlayBlocker.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        overlayBlocker.GetComponent<Image>().color = new Color(0, 0, 0, 0); 
        overlayBlocker.transform.SetAsFirstSibling();
        var sidebarScreen = targetPage.transform.Find("sidebarScreen");
        sidebarScreen.transform.position = new Vector3(-Screen.width, targetPage.transform.position.y, targetPage.transform.position.z);
        targetPage.SetActive(true);

        DOTween.Sequence()
            .OnStart(() =>
            {
                overlayBlocker.GetComponent<Image>().DOFade(0.7f, 0.4f).SetEase(Ease.Linear);
            })
            .Append(sidebarScreen.transform.DOMoveX(Screen.width / 1f, 0.4f).SetEase(Ease.OutQuad))
            .OnComplete(() =>
            {
            });
    }

    public void closeSidebar(GameObject currentPage)
    {
        GameObject overlayBlocker = currentPage.transform.GetChild(0).gameObject;
        if (overlayBlocker != null && overlayBlocker.GetComponent<Image>() != null)
        {
            currentPage.SetActive(true);
            var sidebarScreen = currentPage.transform.Find("sidebarScreen");

            DOTween.Sequence()
                .OnStart(() =>
                {
                    overlayBlocker.SetActive(true);
                })
                .Append(sidebarScreen.transform.DOMoveX(-Screen.width, 0.4f).SetEase(Ease.OutQuad))
                .Join(overlayBlocker.GetComponent<Image>().DOFade(0, 0.4f).SetEase(Ease.OutQuad))
                .OnComplete(() =>
                {
                    GameObject.Destroy(overlayBlocker);
                });
        }
        else
        {
            currentPage.transform.DOMoveX(-Screen.width, 0.4f).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                });
        }
    }
    public void AnimateRectTransformX(RectTransform rectTransform, float targetX, float duration)
    {
        rectTransform.DOAnchorPosX(targetX, duration).SetEase(Ease.InOutCubic);
    }
    public void ShowTextForOneSecond(TextMeshProUGUI messageText, string message)
    {
        // Set the text and ensure it's fully visible (alpha = 1)
        messageText.text = message;
        messageText.alpha = 1;

        // Hide the text after 1 second using a fade-out animation
        messageText.DOFade(0, 1f).SetDelay(1f);  // Wait for 1 second, then fade out over 1 second
    }
    public void WobbleObject(GameObject pAppObject)
    {
        float mWobbleDuration = 0.45f;
        Vector3 mWobbleStrength = new Vector3(1.05f, 1.05f, 1f);
        pAppObject.transform.DOComplete();
        pAppObject.transform.DOPunchScale(Vector3.one - mWobbleStrength, mWobbleDuration, 1, 0);
    }
}

