using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddMealController : MonoBehaviour, PageController
{
    public GameObject aScrollViewContent;
    public List<SubCategory> mSubCategory = new List<SubCategory>();
    public TMP_Dropdown sDropdown;
    public TMP_Dropdown sCategoryDropdown;
    public TMP_InputField aSearchBar;
    public TMP_Text aServingText;
    public GridLayoutGroup gridLayoutGroup;
    public GridLayoutGroup gridLayoutFilterGroup;
    string mSearchText = "";

    int mDayState;
    DateTime mDate;


    public void onInit(Dictionary<string, object> data)
    {
        mDayState = (int)data["state"];
        mDate = (DateTime)data["date"];

        aSearchBar.onValueChanged.AddListener(HandleInputChanged);
        sCategoryDropdown.onValueChanged.AddListener(delegate { PopulateDropdownCategory(); });
        sDropdown.onValueChanged.AddListener(delegate { initFoodCategories(); });

        PopulateDropdownCategory();
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
        aScrollViewContent.SetActive(false);
        foreach (Transform child in aScrollViewContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        int index = 0;
        aServingText.SetText("category not selected !");
        Dictionary<string, MealCategory> mCategories = DataManager.Instance.GetCategories();
        foreach (var category in mCategories)
        {
            if (sCategoryDropdown.options[sCategoryDropdown.value].text.Equals(category.Key) || mSearchText.Length>0)
            {
                foreach (var subCategory in category.Value.SubCategories)
                {
                    if (subCategory.Title.Equals(sDropdown.options[sDropdown.value].text) || mSearchText.Length > 0)
                    {
                        aServingText.SetText(subCategory.EachServing.Carb + "g carbs, " + subCategory.EachServing.Protein + "g proteins, " + subCategory.EachServing.Fat + "g fats, " + subCategory.EachServing.KiloCal + " kcal");
                        foreach (var dish in subCategory.Dishes)
                        {
                            if (dish.Key.ToLower().Contains(mSearchText.ToLower()))
                            {
                                GameObject categoryItem = Instantiate(Resources.Load<GameObject>("Prefabs/addMeal/addMealCategory"));
                                categoryItem.name = "Category_" + index++;
                                categoryItem.transform.SetParent(aScrollViewContent.transform, false);
                                addMealCategoryController categoryController = categoryItem.GetComponent<addMealCategoryController>();
                                string imagePath = dish.Value.ItemSourceImage;
                                categoryController.initCategory(dish.Key, dish.Value, subCategory.EachServing, imagePath, mDayState, mDate);
                            }
                        }
                        aScrollViewContent.SetActive(false);
                        aScrollViewContent.SetActive(true);

                    }
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
        Dictionary<string, MealCategory> mCategories = DataManager.Instance.GetCategories();
        sDropdown.options.Add(new TMP_Dropdown.OptionData("Select Sub Catagory"));
        foreach (var category in mCategories)
        {
            if (sCategoryDropdown.options[sCategoryDropdown.value].text.Equals(category.Key))
            {
                foreach (var subCategory in category.Value.SubCategories)
                {
                    if (subCategory.Title.Equals("root"))
                    {
                        subCategory.Title = category.Key.ToString();
                    }
                    mSubCategory.Add(subCategory);
                    sDropdown.options.Add(new TMP_Dropdown.OptionData(subCategory.Title));
                }
            }
        }
        sDropdown.value = 0;
        sDropdown.RefreshShownValue();
        initFoodCategories();
    }

    void PopulateDropdownCategory()
    {
        sCategoryDropdown.options.Clear();
        Dictionary<string, MealCategory> mCategories = DataManager.Instance.GetCategories();
        sCategoryDropdown.options.Add(new TMP_Dropdown.OptionData("Select Catagory"));
        foreach (var category in mCategories)
        {
            sCategoryDropdown.options.Add(new TMP_Dropdown.OptionData(category.Key));
        }
        sDropdown.value = 0;
        sDropdown.RefreshShownValue();
        PopulateDropdown();
    }


    public void onGoBack()
    {
        StateManager.Instance.HandleBackAction(gameObject);
    }

    void UpdateCellSize()
    {
        gridLayoutFilterGroup.cellSize = new Vector2(gameObject.GetComponent<RectTransform>().rect.width / 2.22f, gridLayoutFilterGroup.cellSize.y);
        gridLayoutGroup.cellSize = new Vector2(gameObject.GetComponent<RectTransform>().rect.width / 2.2f, gridLayoutGroup.cellSize.y);
    }

    void Start()
    {
        
    }

    public void onOpenSideBar()
    {
        StateManager.Instance.openSidebar("sidebar", gameObject, "sidebarScreen");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject sidebarScreen = GameObject.Find("sidebarScreen(Clone)");
            if (sidebarScreen == null)
            {
                if (!userSessionManager.Instance.mSidebar)
                {
                    StateManager.Instance.HandleBackAction(gameObject);
                }
            }
        }
    }
}
