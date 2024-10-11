using System;
using System.Collections.Generic;

[System.Serializable]
public class AchievementDataItem
{
    public string description;
    public bool isCompleted;
    public List<string> category_exercise;

    public int value;

}

[System.Serializable]
public class AchievementTemplate
{
    public string title;
    public AchievementType type;
    public List<AchievementDataItem> achievementData = new List<AchievementDataItem>();
}

[System.Serializable]
public class AchievementData
{
    public List<AchievementTemplate> achievements = new List<AchievementTemplate>();
}