using Newtonsoft.Json;
using System.Collections.Generic;

public class MealCategory
{
    public List<SubCategory> SubCategories { get; set; }
    public string Title { get; set; }
    public string ItemSourceImage { get; set; }
}

public class SubCategory
{
    public string Title { get; set; }
    public ServingInfo EachServing { get; set; }
    public Dictionary<string, MealItem> Dishes { get; set; }
}

public class ServingInfo
{
    [JsonConverter(typeof(StringToNumberConverter))]
    public double Carb { get; set; }

    [JsonConverter(typeof(StringToNumberConverter))]
    public double Protein { get; set; }

    [JsonConverter(typeof(StringToNumberConverter))]
    public double Fat { get; set; }

    [JsonConverter(typeof(StringToNumberConverter))]
    public double KiloCal { get; set; }
}

public class MealItem
{
    public string Measure { get; set; }

    [JsonConverter(typeof(StringToNumberConverter))]
    public double Amount { get; set; }

    public string ItemSourceImage { get; set; } = "";
    public string RecipeURL { get; set; } = "";
}