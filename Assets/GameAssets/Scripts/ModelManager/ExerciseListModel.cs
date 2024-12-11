using System;
using System.Collections.Generic;
[Serializable]

public class ExerciseDataItem
{
    public string exerciseName;
    public string category;
    public List<string> secondaryCategoryies = new List<string>();
    public string icon;
    public int rank;
    public ExerciseType exerciseType;
}

[Serializable]
public class ExerciseData
{
    public List<ExerciseDataItem> exercises=new List<ExerciseDataItem>();
}
