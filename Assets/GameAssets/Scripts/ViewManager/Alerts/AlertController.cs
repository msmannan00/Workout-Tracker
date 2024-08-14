using System;
using TMPro;
using UnityEngine;

public class AlertController : MonoBehaviour
{
    public TMP_Text aHeader;
    public TMP_Text aTrigger;
    public TMP_Text aSecondaryTrigger;
    public TMP_Text aMessage;
    public Action mCallbackSuccess;


    public void InitController(string pMessage, Action pCallbackSuccess = null, string pHeader = "Success", string pTrigger = "Proceed", string pSecondaryTrigger = "Dismiss")
    {
        aHeader.text = pHeader;
        aTrigger.text = pTrigger;
        aSecondaryTrigger.text = pSecondaryTrigger;
        aMessage.text = pMessage;
        mCallbackSuccess = pCallbackSuccess;
    }

    public void OnTriggerPrimary()
    {
        if(mCallbackSuccess != null)
        {
            mCallbackSuccess.Invoke();
        }
        GlobalAnimator.Instance.AnimateAlpha(gameObject, false);
    }

    public void OnTriggerSecondary()
    {
        GlobalAnimator.Instance.AnimateAlpha(gameObject, false);
    }

    public void OnClose()
    {
        GlobalAnimator.Instance.AnimateAlpha(gameObject, false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
