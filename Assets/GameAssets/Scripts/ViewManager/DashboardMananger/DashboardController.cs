using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardController : MonoBehaviour, PageController 
{
    public TMP_Text aUserWelcome;
    public GameObject aScrollViewContent;
    public GridLayoutGroup gridLayoutGroup;

    public void onInit(Dictionary<string, object> data)
    {
        aUserWelcome.text = "Welcome! " + userSessionManager.Instance.mProfileUsername;
        initFoodCategories();
        UpdateCellSize();
    }

    public void updatePlanDetail()
    {
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        StateManager.Instance.OpenStaticScreen("planCreator", gameObject, "planCreatorScreen", mData, true);
        
    }

    public void viewPlanDetail()
    {
        StateManager.Instance.onRemoveBackHistory();
        Dictionary<string, object> mData = new Dictionary<string, object> { };
        //StateManager.Instance.OpenStaticScreen("dailyMeal", gameObject, "dailyMealScreen", mData, true);
        StateManager.Instance.OpenStaticScreen("exercise", gameObject, "exerciseScreen", mData, true);
    }

    public void initFoodCategories()
    {
        Dictionary<string, MealCategory> mCategories = DataManager.Instance.GetCategories();
        int index = 0;
        foreach (var category in mCategories)
        {
            GameObject categoryItem = Instantiate(Resources.Load<GameObject>("Prefabs/dashboard/dashboardMealCategory"));
            categoryItem.name = "Category_" + index++;
            categoryItem.transform.SetParent(aScrollViewContent.transform, false);
            DashboardMealCategoryController categoryController = categoryItem.GetComponent<DashboardMealCategoryController>();
            string imagePath = category.Value.ItemSourceImage;
            categoryController.initCategory(category.Value.Title, category.Value.SubCategories, imagePath, gameObject);
        }
        for (int i = 0; i < 2; i++)
        {
            GameObject bottomSpace = new GameObject("BottomSpace_" + i);
            LayoutElement layoutElement = bottomSpace.AddComponent<LayoutElement>();
            layoutElement.minHeight = 50f;
            bottomSpace.transform.SetParent(aScrollViewContent.transform, false);
        }
    }

    public void onOpenSideBar()
    {
        gameObject.transform.parent.SetSiblingIndex(1);
        StateManager.Instance.openSidebar("sidebar", gameObject, "sidebarScreen");
    }

    void UpdateCellSize()
    {
        gridLayoutGroup.cellSize = new Vector2(gameObject.GetComponent<RectTransform>().rect.width / 2.2f, gridLayoutGroup.cellSize.y);
    }


    void Update()
    {
    }
}
