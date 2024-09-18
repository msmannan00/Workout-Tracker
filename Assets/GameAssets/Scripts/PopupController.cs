using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class PopupController : GenericSingletonClass<PopupController>
{
    public void OpenPopup(string folderName, string popupTagName, Action<List<object>> onFinish, List<object> data)
    {
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
