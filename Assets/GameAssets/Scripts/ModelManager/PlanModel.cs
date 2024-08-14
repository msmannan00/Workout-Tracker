using System;
using System.Collections.Generic;

[Serializable]
public class PlanModel
{
    public Dictionary<DateTime, Dictionary<int, MealModel>> Meals { get; set; }
    public PlanModel()
    {
        Meals = new Dictionary<DateTime, Dictionary<int, MealModel>>();
    }

    public void initKey(DateTime mDate, int mDayState, string mTitle)
    {
        if (!Meals.ContainsKey(mDate))
        {
            Meals[mDate] = new Dictionary<int, MealModel>();
        }

        if (!Meals[mDate].ContainsKey(mDayState))
        {
            Meals[mDate][mDayState] = new MealModel();
        }

        if (!Meals[mDate][mDayState].Details.ContainsKey(mTitle))
        {
            Meals[mDate][mDayState].Details[mTitle] = null;
        }
    }
}

[Serializable]
public class MealModel
{
    public Dictionary<string, MealDetail> Details { get; set; }
    public MealModel()
    {
        Details = new Dictionary<string, MealDetail>();
    }
}

[Serializable]
public class MealDetail
{
    public double Fats { get; set; }
    public double Kcals { get; set; }
    public double Carbs { get; set; }
    public double Proteins { get; set; }
    public double ServingAmount { get; set; }
}
