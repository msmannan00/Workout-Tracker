using UnityEngine;
using System.IO;
using UnityEngine.Purchasing.MiniJSON;

public class DataManager : GenericSingletonClass<DataManager>
{
    public ExerciseData exerciseData = new ExerciseData();
    public AchievementData achievementData = new AchievementData();
    public void loadData()
    {
        TextAsset exerciseJsonFile = Resources.Load<TextAsset>("data/exercise");
        this.exerciseData = JsonUtility.FromJson<ExerciseData>(exerciseJsonFile.text);

        TextAsset achievementJsonFile = Resources.Load<TextAsset>("data/achievement");
        this.achievementData = JsonUtility.FromJson<AchievementData>(achievementJsonFile.text);

    }

    public ExerciseData getExerciseData()
    {
        return this.exerciseData;
    }

    public void SaveExerciseData(ExerciseDataItem exercise)
    {
        this.exerciseData.exercises.Add(exercise);
        string json = JsonUtility.ToJson(exerciseData, true);
        string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/exercise.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
        File.WriteAllText(filePath, json);
    }
    public void SaveAchievementData()
    {
        string json = JsonUtility.ToJson(achievementData, true);
        string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/achievement.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
        File.WriteAllText(filePath, json);
    }
}
