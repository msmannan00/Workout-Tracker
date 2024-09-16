using System;
using System.Collections.Generic;

[Serializable]
public class ExcerciseData
{
    public List<DefaultTempleteModel> exerciseTemplete = new List<DefaultTempleteModel>();
}

[Serializable]
public class DefaultTempleteModel
{
    public string templeteName = "Workout";
    public string templeteNotes;
    public List<ExerciseTypeModel> exerciseTemplete = new List<ExerciseTypeModel>();
}

[Serializable]
public class ExerciseTypeModel
{
    public int index = 0;
    public string name;
    public ExerciseType exerciseType;
    public List<ExerciseModel> exerciseModel = new List<ExerciseModel>();
}

[Serializable]
public class ExerciseModel
{
    public int setID = 1;
    public string previous = "-";
    public int weight = 0;
    public int lbs = 0;
    public int reps = 0;
    public bool toggle;
    public int time;
}
