using System.Collections.Generic;
[System.Serializable]
public class ExerciseDataItem
{
    public string exerciseName;
    public string category;
    public string icon;
    public int rank;
    public bool isWeightExercise;
}

[System.Serializable]
public class ExerciseData
{
    public List<ExerciseDataItem> exercises=new List<ExerciseDataItem>();
}