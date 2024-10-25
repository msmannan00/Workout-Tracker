using System;
using System.Collections.Generic;

[System.Serializable]
public class AchievementTemplateDataItem
{
    public string description;
    public bool isCompleted;

    public float value;

}

[System.Serializable]
public class AchievementTemplate
{
    public string title;
    public AchievementType type;
    public List<string> category_exercise;
    public List<AchievementTemplateDataItem> achievementData = new List<AchievementTemplateDataItem>();
}

[System.Serializable]
public class AchievementData
{
    public List<AchievementTemplate> achievements = new List<AchievementTemplate>();
}