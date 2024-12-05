using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class PopupController : GenericSingletonClass<PopupController>
{
    public void OpenPopup(string folderName, string popupTagName, Action<List<object>> onFinish, List<object> data)
    {
        GameObject allreadyHas = GameObject.FindWithTag(popupTagName);
        if(allreadyHas != null) { return; }
        GameObject popupPrefab = Resources.Load<GameObject>("Prefabs/" + folderName + "/" + popupTagName);
        GameObject root = GameObject.FindGameObjectWithTag("rootController");
        GameObject popup = Instantiate(popupPrefab, root.transform);
        popup.name = popupTagName;
        popup.SetActive(true);
        popup.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Sequence seq = DOTween.Sequence();
        seq.Append(popup.transform.DOScale(1.0f, 0.25f));
        seq.Join(popup.GetComponent<CanvasGroup>().DOFade(1, 0.15f));

        var popupInitializer = popup.GetComponent<IPrefabInitializer>();
        if (popupInitializer != null)
        {
            popupInitializer.InitPrefab(onFinish, data);
        }
    }
    public void OpenSidePopup(string folderName, string popupTagName, Action<List<object>> onFinish, List<object> data, RectTransform buttonTransform)
    {
        GameObject popupPrefab = Resources.Load<GameObject>("Prefabs/" + folderName + "/" + popupTagName);
        GameObject root = GameObject.FindGameObjectWithTag("rootController");
        GameObject popup = Instantiate(popupPrefab, root.transform);
        popup.name = popupTagName;
        popup.SetActive(true);

        // Set the scale and fade as before
        popup.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        Sequence seq = DOTween.Sequence();
        seq.Append(popup.transform.DOScale(1.0f, 0.25f));
        seq.Join(canvasGroup.DOFade(1, 0.15f));

        Canvas canvas = root.transform.parent.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.worldCamera; // Ensure you have assigned a camera to the canvas

        // Convert button's world position to screen position
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, buttonTransform.position);

        // Convert screen position to local canvas position
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out localPoint);

        // Apply the local position to the popup's RectTransform
        popup.GetComponent<RectTransform>().anchoredPosition = localPoint;

        // Optionally, apply an offset to adjust where the popup opens relative to the button
        Vector2 offset = new Vector2(-100, -10f);  // Adjust based on where you want the popup to appear
        popup.GetComponent<RectTransform>().anchoredPosition += offset;

        // Initialize the popup with data if needed
        var popupInitializer = popup.GetComponent<IPrefabInitializer>();
        if (popupInitializer != null)
        {
            popupInitializer.InitPrefab(onFinish, data);
        }
    }
    public void ClosePopup(string popupTagName)
    {
        GameObject popup = GameObject.Find(popupTagName);
        if (popup != null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(popup.transform.DOScale(0.5f, 0.1f));
            seq.Join(popup.GetComponent<CanvasGroup>().DOFade(0, 0.15f));
            seq.OnComplete(() =>
            {
                Destroy(popup);
            });
        }
    }
}
