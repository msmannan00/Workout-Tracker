using System;
using System.Collections.Generic;

[System.Serializable]
public class PersonalBestDataItem
{
    public string exerciseName;
    public int weight;
    public int rep;
}

[System.Serializable]
public class PersonalBestData
{
    public List<PersonalBestDataItem> exercises = new List<PersonalBestDataItem>();
}