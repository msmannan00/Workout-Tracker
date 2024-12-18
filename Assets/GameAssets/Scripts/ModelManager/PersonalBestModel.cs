using System;
using System.Collections.Generic;

[Serializable]
public class PersonalBestDataItem
{
    public string exerciseName;
    public int weight;
    public int rep;
}

[Serializable]
public class PersonalBestData
{
    public List<PersonalBestDataItem> exercises = new List<PersonalBestDataItem>();
}