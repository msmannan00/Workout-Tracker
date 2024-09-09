using UnityEngine;
using System.IO;
using UnityEngine.Purchasing.MiniJSON;

public class DataManager : GenericSingletonClass<DataManager>
{
    public ExerciseData exerciseData = new ExerciseData();
    
    public void loadData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("data/exercise");
        this.exerciseData = JsonUtility.FromJson<ExerciseData>(jsonFile.text);
    }

    public ExerciseData getExerciseData()
    {
        return this.exerciseData;
    }

    public void SaveData(ExerciseDataItem exercise)
    {
        this.exerciseData.exercises.Add(exercise);
        string json = JsonUtility.ToJson(exerciseData, true);
        string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/exercise.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
        File.WriteAllText(filePath, json);
    }
}
