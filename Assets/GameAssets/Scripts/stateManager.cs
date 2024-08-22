using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : GenericSingletonClass<StateManager>
{
    private List<GameObject> inactivePages = new List<GameObject>();
    private bool isProcessing = false;

    public void OpenStaticScreen(string folderPath, GameObject currentPage, string newPage, Dictionary<string, object> data, bool keepState = false)
    {
        if (isProcessing) return;
        isProcessing = true;

        if (!keepState)
        {
            onRemoveBackHistory();
        }

        var prefabPath = "Prefabs/"+ folderPath + "/" + newPage;
        var prefabResource = Resources.Load<GameObject>(prefabPath);
        print(prefabResource);
        var prefab = Instantiate(prefabResource);
        var container = GameObject.FindGameObjectWithTag(newPage);

        prefab.transform.SetParent(container.transform, false);
        var mController = prefab.GetComponent<PageController>();
        mController.onInit(data);

        if (currentPage != null)
        {
            Action callbackSuccess = () =>
            {
                if (keepState)
                {
                    currentPage.SetActive(false);
                    inactivePages.Add(currentPage);
                }
                else
                {
                    Destroy(currentPage);
                }
                isProcessing = false;
            };

            GlobalAnimator.Instance.ApplyParallax(currentPage, prefab, callbackSuccess, keepState);
        }
        else
        {
            isProcessing = false;
        }
    }

    public void openSidebar(string folderPath, GameObject currentPage, string newPage)
    {
        var prefabPath = "Prefabs/" + folderPath + "/" + newPage;
        var prefabResource = Resources.Load<GameObject>(prefabPath);
        var prefab = Instantiate(prefabResource);
        var container = GameObject.FindGameObjectWithTag(newPage);

        prefab.transform.SetParent(container.transform, false);

        GlobalAnimator.Instance.openSidebar(prefab);

    }

    public void HandleSidebarBackAction(GameObject currentActivePage)
    {
        GlobalAnimator.Instance.closeSidebar(currentActivePage);
    }

    public void HandleBackAction(GameObject currentActivePage)
    {
        float moveTargetX = currentActivePage.transform.position.x + Screen.width;  

        currentActivePage.transform.DOMoveX(moveTargetX, 0.3f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            Destroy(currentActivePage);
        });

        if (inactivePages.Count > 0)
        {
            GameObject lastPage = inactivePages[inactivePages.Count - 1];
            CanvasGroup canvasGroup = lastPage.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = lastPage.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 1;  
            lastPage.SetActive(true);
            lastPage.GetComponent<CanvasGroup>().interactable = true;

            lastPage.transform.position = new Vector3(lastPage.transform.position.x - 500, lastPage.transform.position.y, lastPage.transform.position.z);
            lastPage.transform.DOMoveX(lastPage.transform.position.x + 500, 0f).SetEase(Ease.InOutQuad);

            GameObject overlayBlocker = lastPage.transform.Find("overlayBlocker(Clone)").gameObject;
            if (overlayBlocker != null)
            {
                CanvasGroup overlayCanvasGroup = overlayBlocker.GetComponent<CanvasGroup>();
                if (overlayCanvasGroup == null)
                {
                    overlayCanvasGroup = overlayBlocker.AddComponent<CanvasGroup>();
                }
                overlayCanvasGroup.DOFade(0, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
                {
                    Destroy(overlayBlocker);
                });
            }

            inactivePages.RemoveAt(inactivePages.Count - 1);
        }
    }


    public void onRemoveBackHistory()
    {
        foreach (GameObject page in inactivePages)
        {
            Destroy(page);
        }
        inactivePages.Clear();
    }

    public int getInactivePagesCount()
    {
        return inactivePages.Count;
    }
    public bool checkPageByTag(string tag)
    {
        foreach(GameObject page in inactivePages)
        {
            if(page.tag == tag)
            {
                return true;
            }
        }
        return false;
    }
}
