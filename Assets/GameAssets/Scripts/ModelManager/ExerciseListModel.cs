using System;

using System.Collections.Generic;
[System.Serializable]

public class ExerciseDataItem
{
    public string exerciseName;
    public string category;
    public List<string> secondaryCategoryies = new List<string>();
    public string icon;
    public int rank;
    public ExerciseType exerciseType;
}

[System.Serializable]
public class ExerciseData
{
    public List<ExerciseDataItem> exercises=new List<ExerciseDataItem>();
}
