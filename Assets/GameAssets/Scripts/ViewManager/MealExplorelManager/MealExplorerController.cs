using AwesomeCharts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MealExplorerController : MonoBehaviour, PageController
{
    public List<SubCategory> mSubCategory;
    public GameObject aScrollViewContent;
    public TMP_InputField aSearchBar;
    public List<string> mSubCategoryTitle;
    public TMP_Dropdown sDropdown;
    public TMP_Text aServingText;
    public GridLayoutGroup gridLayoutGroup;

    string mSearchText = "";
    string mTitle;

    public void onInit(Dictionary<string, object> data)
    {

        mSubCategory = (List<SubCategory>)data["data"];
        mTitle = (string)data["title"];
        aSearchBar.onValueChanged.AddListener(HandleInputChanged);
        sDropdown.onValueChanged.AddListener(delegate { initFoodCategories(); });
        PopulateDropdown();
        initFoodCategories();
        UpdateCellSize();
    }
    private void HandleInputChanged(string text)
    {
        mSearchText = text;
        initFoodCategories();
    }

    public void initFoodCategories()
    {
        foreach (Transform child in aScrollViewContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        aScrollViewContent.SetActive(false);

        int index = 0;
        foreach (var categoryItem in mSubCategory)
        {
            if (categoryItem.Title.Equals("root")  || categoryItem.Title.Equals(sDropdown.options[sDropdown.value].text))
            {
                foreach (var mDishItem in categoryItem.Dishes)
                {
                    string imagePath = mDishItem.Value.ItemSourceImage;
                    string description = categoryItem.EachServing.KiloCal + " / " + mDishItem.Value.Amount;
                    if (mDishItem.Key.ToLower().Contains(mSearchText.ToLower()) || description.ToLower().Contains(mSearchText.ToLower()))
                    {
                        GameObject dish = Instantiate(Resources.Load<GameObject>("Prefabs/mealExplorer/mealExplorerCategory"));
                        dish.name = "Category_" + index++;
                        dish.transform.SetParent(aScrollViewContent.transform, false);
                        MealExplorerCategoryController categoryController = dish.GetComponent<MealExplorerCategoryController>();
                        categoryController.InitCategory(mDishItem.Key, description, mDishItem.Value, categoryItem.EachServing, imagePath, gameObject);
                        aServingText.SetText(categoryItem.EachServing.Carb + "g carbs, " + categoryItem.EachServing.Protein + "g proteins, " + categoryItem.EachServing.Fat + "g fats, " + categoryItem.EachServing.KiloCal + " kcal");
                    }
                    aScrollViewContent.SetActive(false);
                    aScrollViewContent.SetActive(true);
                }
            }
        }
        for (int i = 0; i < 2; i++)
        {
            GameObject bottomSpace = new GameObject("BottomSpace_" + i);
            LayoutElement layoutElement = bottomSpace.AddComponent<LayoutElement>();
            layoutElement.minHeight = 50f;
            bottomSpace.transform.SetParent(aScrollViewContent.transform, false);
        }

    }

    void PopulateDropdown()
    {
        sDropdown.options.Clear();
        foreach (var categoryItem in mSubCategory)
        {
            if (!categoryItem.Title.Equals("root"))
            {
                sDropdown.options.Add(new TMP_Dropdown.OptionData(categoryItem.Title));
            }
            else
            {
                sDropdown.options.Add(new TMP_Dropdown.OptionData(mTitle));
            }

        }

        sDropdown.value = 0;
        sDropdown.RefreshShownValue();
    }

    void Start()
    {
        
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

    void UpdateCellSize()
    {
        gridLayoutGroup.cellSize = new Vector2(gameObject.GetComponent<RectTransform>().rect.width / 2.2f, gridLayoutGroup.cellSize.y);
    }

    public void onOpenSideBar()
    {
        StateManager.Instance.openSidebar("sidebar", gameObject, "sidebarScreen");
    }

    public void onGoBack()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }

}
