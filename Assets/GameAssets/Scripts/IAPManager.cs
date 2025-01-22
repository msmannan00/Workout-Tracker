
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.Purchasing;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;
using System.Runtime.Serialization;
using LukeWaffel.AndroidGallery;


[Serializable]
public class Subscription
{
    public string Name;
    public string Id;
    public string desc;
    public float price;
    public float timeDuration; // in days
}


public class IAPManager : GenericSingletonClass<IAPManager>, IDetailedStoreListener
{
    IStoreController m_StoreContoller;

    IAppleExtensions m_AppleExtensions;
    //public Text restoreText;

    public Subscription subItem;

    public TextMeshProUGUI noInternet;
    bool check;
    public bool IAPInitialized;

    public bool isSubscripted;

    private void Start()
    {
        //if (PlayerPrefs.GetInt("Remove Ads") == 0)
        //{
        //    removedButton.SetActive(false);
        //    removeAdsButton.SetActive(true);
        //}
        //else
        //{
        //    removedButton.SetActive(true);
        //    removeAdsButton.SetActive(false);
        //}
        check = true;
        SetupBuilder();
    }

    #region setup and initialize
    void SetupBuilder()
    {
        if (CheckInterNet())
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(subItem.Id, ProductType.Subscription);

            UnityPurchasing.Initialize(this, builder);
            IAPInitialized = true;
            //m_AppleExtensions.Initialize();
            

        }
        else
        {
            StartCoroutine(Wait());
        }
    }

    public void Refresh()
    {
        m_AppleExtensions.RefreshAppReceipt(OnRefreshSuccess, OnRefreshFailure);
    }
    private void OnRefreshFailure(string obj)
    {

    }

    private void OnRefreshSuccess(string obj)
    {

    }
    //public void Restore()
    //{
    //    if (CheckInterNet())
    //    {
    //        restoreText.text = "Restore Purchase Loading";
    //        StartCoroutine(RestoreText());
    //        m_AppleExtensions.RestoreTransactions(OnRestore);
    //    }
    //    else
    //    {
    //        restoreText.text = "Check InterNet Connection";
    //        StartCoroutine(RestoreText());
    //    }
    //}
    //void OnRestore(bool success, string error)
    //{
    //    if (success)
    //    {
    //        // merely that the restoration process succeeded.
    //        PlayerPrefs.SetInt("Remove Ads", 1);
    //        //print("Success");
    //        restoreText.text = "Restore Purchase successfully";
    //        StartCoroutine(RestoreText());
    //        removedButton.SetActive(true);
    //        removeAdsButton.SetActive(false);
    //        //cameraController.inst.removeadsbtn.SetActive(false);
    //        //Debug.Log("In-App Purchasing successfully Restore");
    //    }
    //    else
    //    {
    //        //Debug.Log("In-App Purchasing Restore Fail");
    //        restoreText.text = "Restore Failed";
    //        StartCoroutine(RestoreText());
    //        // Restoration failed.
    //    }
    //}
    //IEnumerator RestoreText()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    restoreText.text = "";
    //}
    IEnumerator Wait()
    {
        // Wait for some time before retrying
        yield return new WaitForSeconds(5); // Adjust the delay time as needed

        // Check internet connectivity again
        if (!IAPInitialized)
            SetupBuilder();
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Success");
        m_StoreContoller = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        Refresh();
        CheckSubscripted(subItem.Id);
    }
    #endregion


    #region button clicks 


    public void Subscription_Btn_Pressed()
    {
        //RemoveAds();
        if (Application.internetReachability != NetworkReachability.NotReachable ||
               Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
               Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
               )
        {
            if (IAPInitialized)
            {
                m_StoreContoller.InitiatePurchase(subItem.Id);
            }
            else
            {
                SetupBuilder();
            }
        }
        else
        {
            if (check)
            {
                check = false;
                noInternet.transform.DOScale(1, 2).SetEase(Ease.OutBounce).OnComplete(() =>
                noInternet.transform.DOScale(0, 1).SetEase(Ease.Linear).OnComplete(() =>
                check = true));
            }
        }
    }

    #endregion


    #region main
    //processing purchase
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //Retrive the purchased product
        var product = purchaseEvent.purchasedProduct;

        print("Purchase Complete" + product.definition.id);
        if (product.definition.id == subItem.Id)//non consumable
        {
            isSubscripted = true;
            //RemoveAds();
        }

        return PurchaseProcessingResult.Complete;
    }
    #endregion




    void CheckSubscripted(string id)
    {
        if (m_StoreContoller != null)
        {
            var subProduct = m_StoreContoller.products.WithID(id);
            if (subProduct != null)
            {
                try
                {
                    if (subProduct.hasReceipt)
                    {
                        var subManager = new SubscriptionManager(subProduct, null);
                        var info = subManager.getSubscriptionInfo();
                        if (info.isSubscribed() == Result.True)
                        {
                            isSubscripted = true;
                        }
                        else
                        {
                            isSubscripted = false;
                        }
                    }
                    else
                    {
                        print("receipt not found");
                    }
                }
                catch (Exception)
                {
                    print("not check in editor");
                }
            }
        }
    }


    bool CheckInterNet()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable ||
               Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
               Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
               ) { return true; }
        else
        {
            return false;
        }
    }
    #region error handeling
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("failed" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("initialize failed" + error + message);
    }



    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("purchase failed" + failureReason);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        print("purchase failed" + failureDescription);
    }
    #endregion


    #region extra 



    [Header("Non Consumable")]
    public GameObject removedButton;
    public GameObject removeAdsButton;

    public bool simulateAskToBuy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    void RemoveAds()
    {
        DisplayAds(false);
    }
    void ShowAds()
    {
        DisplayAds(true);
        print("ded");
    }
    void DisplayAds(bool x)
    {
        if (!x)
        {
            removedButton.SetActive(true);
            removeAdsButton.SetActive(false);
            PlayerPrefs.SetInt("Remove Ads", 1);
            print(PlayerPrefs.GetInt("Remove Ads"));
            //FindObjectOfType<AdmobAdsScript>().DestroyBannerAd();
        }

    }







    #endregion

}


