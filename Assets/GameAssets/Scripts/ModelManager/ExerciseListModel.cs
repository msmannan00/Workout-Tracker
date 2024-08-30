[System.Serializable]
public class ExerciseDataItem
{
    public string exerciseName;
    public string category;
    public string icon;
    public int rank;
}

[System.Serializable]
public class ExerciseData
{
    public ExerciseDataItem[] exercises;
}