using System;
using System.Collections.Generic;
using System.Linq;

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
    public float totalWeight;
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

// for exercise history show easyness
[Serializable]
public class ExerciseWithDate
{
    public List<HistoryExerciseModel> exercise =new List<HistoryExerciseModel>();
    public string dateTime;
    public ExerciseType exerciseType;
    public HistoryPerformance performance=HistoryPerformance.None;

    public HistoryExerciseModel GetHeaviestLiftedSet()
    {
        return exercise.OrderByDescending(e => e.weight).FirstOrDefault();
    }

    // Calculate the best set based on the formula: weight * (1 + 0.0333 * reps)
    public HistoryExerciseModel GetBestSet()
    {
        return exercise.OrderByDescending(e => e.weight * (1 + 0.0333f * e.reps)).FirstOrDefault();
    }
}
