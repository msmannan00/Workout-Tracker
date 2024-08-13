using AwesomeCharts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealDetailController : MonoBehaviour, PageController
{
    string mTitle;
    ServingInfo mEachServing;
    MealItem mDish;

    public Image aHeaderImage;
    public GameObject aPieChart;
    public GameObject aImageLoader;
    public GameObject aOpenRecipe;
    public TMP_Text aTitle;
    public TMP_Text aQuantityValue;
    public TMP_Text aServingSizeValue;
    public TMP_Text aCarbValue;
    public TMP_Text aCarbPercentage;
    public TMP_Text aFatValue;
    public TMP_Text aFatPercentage;
    public TMP_Text aProteinValue;
    public TMP_Text aProteinPercentage;

    public TMP_Text aKiloCalories;
    public TMP_Text aFat;
    public TMP_Text aProteins;
    public TMP_Text aCarbs;
    private PieChart pieChart;

    public void onInit(Dictionary<string, object> data)
    {
        mTitle = (string)data["title"];
        mEachServing = (ServingInfo)data["eachServing"];
        mDish = (MealItem)data["dish"];
        pieChart = aPieChart.GetComponent<PieChart>();
    }

    void Start()
    {
        string pImagePath = mDish.ItemSourceImage;
        if (pImagePath.StartsWith("http://") || pImagePath.StartsWith("https://"))
        {
            StartCoroutine(HelperMethods.Instance.LoadImageFromURL(pImagePath, aHeaderImage, aImageLoader));
        }
        else
        {
            HelperMethods.Instance.LoadImageFromResources("UIAssets/mealExplorer/Categories/" + pImagePath, aHeaderImage);
            aImageLoader.SetActive(false);
        }

        double total = mEachServing.Carb + mEachServing.Fat + mEachServing.Protein;
        double carbsPercentage = (mEachServing.Carb / total) * 100;
        double fatsPercentage = (mEachServing.Fat / total) * 100;
        double proteinsPercentage = (mEachServing.Protein / total) * 100;

        aTitle.text = mTitle;
        aQuantityValue.text = mDish.Amount.ToString("N2") + "g";
        aServingSizeValue.text = mDish.Measure + " cup";  // Ensure 'Measure' is a numeric value before formatting
        aCarbValue.text = mEachServing.Carb.ToString("N2") + "g";
        aFatValue.text = mEachServing.Fat.ToString("N2") + "g";
        aProteinValue.text = mEachServing.Protein.ToString("N2") + "g";
        aCarbPercentage.text = carbsPercentage.ToString("N2") + "%";
        aFatPercentage.text = fatsPercentage.ToString("N2") + "%";
        aProteinPercentage.text = proteinsPercentage.ToString("N2") + "%";
        aKiloCalories.text = mEachServing.KiloCal.ToString("N2");

        aFat.text = fatsPercentage.ToString("N2") + "%";
        aProteins.text = proteinsPercentage.ToString("N2") + "%";
        aCarbs.text = carbsPercentage.ToString("N2") + "%";

        if (total == 0)
        {
            pieChart.UpdateEntry("Other", 100);
        }
        else
        {
            pieChart.UpdateEntry("Carbs", carbsPercentage);
            pieChart.UpdateEntry("Fats", fatsPercentage);
            pieChart.UpdateEntry("Proteins", proteinsPercentage);
            pieChart.UpdateEntry("Other", 100 - carbsPercentage - fatsPercentage - proteinsPercentage);
        }
        if (mDish.RecipeURL.Equals(""))
        {
            aOpenRecipe.SetActive(false);
        }

    }

    public void openRecipe()
    {
        Application.OpenURL(mDish.RecipeURL);
    }

    public void onGoBack()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }

    public void onOpenSideBar()
    {
        StateManager.Instance.openSidebar("sidebar", gameObject, "sidebarScreen");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!userSessionManager.Instance.mSidebar)
            {
                StateManager.Instance.HandleBackAction(gameObject);
            }
        }
    }
}
