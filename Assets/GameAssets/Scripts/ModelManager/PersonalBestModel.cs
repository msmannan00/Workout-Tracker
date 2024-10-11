using System;
using System.Collections.Generic;

[System.Serializable]
public class PersonalBestDataItem
{
    public string exerciseName;
    public string category;
    public int weight;
}

[System.Serializable]
public class PersonalBestData
{
    public List<PersonalBestDataItem> exercises = new List<PersonalBestDataItem>();
}