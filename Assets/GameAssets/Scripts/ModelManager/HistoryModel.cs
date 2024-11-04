using System;
using System.Collections.Generic;

[Serializable]
public class HistoryModel 
{
    public List<HistoryTempleteModel> exerciseTempleteModel = new List<HistoryTempleteModel>();
}
[Serializable]
public class HistoryTempleteModel
{
    public string templeteName;
    public string dateTime;
    public int completedTime;
    public int totalWeight;
    public int prs;
    public List<HistoryExerciseTypeModel> exerciseTypeModel = new List<HistoryExerciseTypeModel>();
}
[Serializable]
public class HistoryExerciseTypeModel
{
    public string exerciseName;
    public string categoryName;
    public string exerciseNotes;
    public int index;
    public ExerciseType exerciseType;
    public List<HistoryExerciseModel> exerciseModel = new List<HistoryExerciseModel>();
}
[Serializable]
public class HistoryExerciseModel
{
    public float weight;
    public int reps;
    public int time;
    public int rir;
    public float mile;
}
