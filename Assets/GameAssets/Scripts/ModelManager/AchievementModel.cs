using System;
using System.Collections.Generic;

[System.Serializable]
public class AchievementTemplateDataItem
{
    public string description;
    public bool isCompleted;
    public float value;
    public int coins;
    public string id;
}

[System.Serializable]
public class AchievementTemplate
{
    public string title;
    public AchievementType type;
    public List<string> category_exercise;
    public List<AchievementTemplateDataItem> achievementData = new List<AchievementTemplateDataItem>();
    public string id;
}

[System.Serializable]
public class AchievementData
{
    public List<AchievementTemplate> achievements = new List<AchievementTemplate>();
}