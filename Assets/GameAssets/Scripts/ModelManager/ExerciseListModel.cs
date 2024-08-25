[System.Serializable]
public class ExerciseDataItem
{
    public string exerciseName;
    public string category;
    public string icon;
}

[System.Serializable]
public class ExerciseData
{
    public ExerciseDataItem[] exercises;
}