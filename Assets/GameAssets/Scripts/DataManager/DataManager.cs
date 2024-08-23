using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;

public class DataManager : GenericSingletonClass<DataManager>
{
    private Dictionary<string, MealCategory> mealData = null;
    private const string MealDataPrefKey = "MealData";

    //Local Development
    //private const string MealDataUrl = "https://drive.google.com/uc?export=download&id=1A_AhmdhAXAxRbguW1encplQ6RNJ9GTlS";

    //Production Development
    //private const string MealDataUrl = "https://drive.google.com/uc?export=download&id=12Wgv_a_pz7bsxKHxVReBKoI7w0MhKf74";

    //Production Live
    private const string MealDataUrl = "https://drive.google.com/uc?export=download&id=1xgFSs-rC-qqf4WAnU5iWeudXSiafHvWW";

    private GameObject uiBlocker;
    private CanvasGroup uiBlockerCanvasGroup;

    void Start()
    {
        uiBlocker = GameObject.Find("UIBlocker");  // Make sure UIBlocker is correctly named in your scene
        if (uiBlocker != null)
        {
            uiBlockerCanvasGroup = uiBlocker.GetComponent<CanvasGroup>();
            if (uiBlockerCanvasGroup == null)
            {
                uiBlockerCanvasGroup = uiBlocker.AddComponent<CanvasGroup>();
            }
        }
        else
        {
            Debug.LogError("UIBlocker not found in the scene. Please ensure it is correctly named and active.");
        }
    }

    public bool IsMealLoaded()
    {
        return mealData != null;
    }

    public void OnServerInitialized()
    {
        ShowUIBlocker();
        StartCoroutine(InitializeMealData());
    }

    private IEnumerator InitializeMealData()
    {
        if (PreferenceManager.Instance.GetBool(MealDataPrefKey))
        {
            string jsonText = PreferenceManager.Instance.GetString(MealDataPrefKey);
            if (string.IsNullOrEmpty(jsonText))
            {
                yield return TryLoadMealDataFromWebOrLocal();
            }
            else
            {
                DeserializeMealData(jsonText, false);
            }
        }
        else
        {
            yield return TryLoadMealDataFromWebOrLocal();
        }
        HideUIBlocker();
    }

    private void ShowUIBlocker()
    {
        if (uiBlocker != null)
        {
            uiBlocker.SetActive(true);
            uiBlockerCanvasGroup.alpha = 1;  // Ensure the blocker is fully visible when shown
        }
    }

    private void HideUIBlocker()
    {
        if (uiBlocker != null)
        {
            uiBlockerCanvasGroup.DOFade(0, 0.5f).SetDelay(0.5f).OnComplete(() =>
            {
                uiBlocker.SetActive(false);
            });
        }
    }

    private IEnumerator TryLoadMealDataFromWebOrLocal()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(MealDataUrl))
        {
            yield return webRequest.SendWebRequest();
            if (!webRequest.isNetworkError && !webRequest.isHttpError)
            {
                string jsonText = webRequest.downloadHandler.text;
                if (DeserializeMealData(jsonText, true))
                {
                    SaveMealData(jsonText);
                }
                else
                {
                    Debug.LogError("Remote meal data corrupted. Loading locally.");
                    LoadLocalMealData();
                    mealOnlineParserError();
                }
            }
            else
            {
                Debug.Log("Failed to download meal data, loading locally.");
                LoadLocalMealData();
                mealOnlineParserError();
            }
        }
    }

    void mealOnlineParserError()
    {
        GameObject alertPrefab = Resources.Load<GameObject>("Prefabs/alerts/alertFailure");
        GameObject alertsContainer = GameObject.FindGameObjectWithTag("alerts");
        GameObject instantiatedAlert = Instantiate(alertPrefab, alertsContainer.transform);
        AlertController alertController = instantiatedAlert.GetComponent<AlertController>();
        alertController.InitController("Internal error occured, please try again later", pTrigger: "Continue", pHeader: "Server Error");
        GlobalAnimator.Instance.AnimateAlpha(instantiatedAlert, true);
    }

    private void LoadLocalMealData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("DataManager/mealdata");
        if (jsonData != null)
        {
            string jsonText = jsonData.text;
            if (DeserializeMealData(jsonText, true))
            {
                SaveMealData(jsonText);
            }
            else
            {
                Debug.LogError("Local meal data file also corrupted.");
            }
        }
        else
        {
            Debug.LogError("Local meal data file not found.");
        }
    }

    private bool DeserializeMealData(string jsonText, bool shouldLogError)
    {
        try
        {
            mealData = JsonConvert.DeserializeObject<Dictionary<string, MealCategory>>(jsonText);
            return true;
        }
        catch (JsonException ex)
        {
            if (shouldLogError)
            {
                Debug.LogError("JSON Parsing Error: " + ex.Message);
            }
            return false;
        }
    }

    private void SaveMealData(string jsonText)
    {
        PreferenceManager.Instance.SetString(MealDataPrefKey, jsonText);
        PreferenceManager.Instance.SetBool(MealDataPrefKey, true);
        PreferenceManager.Instance.Save();
    }
    public Dictionary<string, MealCategory> GetCategories()
    {
        return mealData;
    }

    public List<string> GetSubCategories(string category)
    {
        List<string> subCategories = new List<string>();
        if (mealData.ContainsKey(category))
        {
            var categoryData = mealData[category].SubCategories;
            foreach (var subCategory in categoryData)
            {
                subCategories.Add(subCategory.Title);
            }
        }
        return subCategories;
    }

    public List<MealItem> GetItems(string category, string subCategoryTitle)
    {
        List<MealItem> items = new List<MealItem>();
        if (mealData.ContainsKey(category))
        {
            var categoryData = mealData[category].SubCategories;
            foreach (var subCategory in categoryData)
            {
                if (subCategory.Title == subCategoryTitle)
                {
                    foreach (var item in subCategory.Dishes.Values)
                    {
                        items.Add(item);
                    }
                    break;
                }
            }
        }
        return items;
    }

    public ServingInfo GetEachServing(string category, string subCategoryTitle)
    {
        if (mealData.ContainsKey(category))
        {
            var categoryData = mealData[category].SubCategories;
            foreach (var subCategory in categoryData)
            {
                if (subCategory.Title == subCategoryTitle)
                {
                    return subCategory.EachServing;
                }
            }
        }
        return null;
    }
}
