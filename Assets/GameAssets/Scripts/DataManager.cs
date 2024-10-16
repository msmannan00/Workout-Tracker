using UnityEngine;
using System.IO;
using UnityEngine.Purchasing.MiniJSON;
using System.Linq;

public class DataManager : GenericSingletonClass<DataManager>
{
    public ExerciseData exerciseData = new ExerciseData();
    public AchievementData achievementData = new AchievementData();
    public PersonalBestData personalBestData = new PersonalBestData();
    //public void loadData()
    //{
    //    TextAsset exerciseJsonFile = Resources.Load<TextAsset>("data/exercise");
    //    this.exerciseData = JsonUtility.FromJson<ExerciseData>(exerciseJsonFile.text);

    //    TextAsset achievementJsonFile = Resources.Load<TextAsset>("data/achievement");
    //    this.achievementData = JsonUtility.FromJson<AchievementData>(achievementJsonFile.text);

    //    TextAsset personBestJsonFile = Resources.Load<TextAsset>("data/personalBest");
    //    this.personalBestData = JsonUtility.FromJson<PersonalBestData>(personBestJsonFile.text);

    //}

    //public ExerciseData getExerciseData()
    //{
    //    return this.exerciseData;
    //}
    //public AchievementData getAchievementData()
    //{
    //    return this.achievementData;
    //}
    //public PersonalBestData getPersonalBestData()
    //{
    //    return this.personalBestData;
    //}
    //public void SaveExerciseData(ExerciseDataItem exercise)
    //{
    //    this.exerciseData.exercises.Add(exercise);
    //    string json = JsonUtility.ToJson(exerciseData, true);
    //    string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/exercise.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
    //    File.WriteAllText(filePath, json);
    //}
    //public void SaveAchievementData()
    //{
    //    string json = JsonUtility.ToJson(achievementData, true);
    //    string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/achievement.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
    //    File.WriteAllText(filePath, json);
    //}
    //public void SavePersonalBestData()
    //{
    //    string json = JsonUtility.ToJson(personalBestData, true);
    //    string filePath = "E:/Git Hub/Workout-Tracker/Assets/Resources/Data/personalBest.json";//Path.Combine(Application.persistentDataPath, "data/exercise");
    //    File.WriteAllText(filePath, json);
    //}
    //public int GetCompletedAchievements()
    //{
    //    int completedCount = 0;
    //    foreach (AchievementTemplate achievement in achievementData.achievements)
    //    {
    //        bool isAchievementCompleted = achievement.achievementData.All(item => item.isCompleted);
    //        if (isAchievementCompleted)
    //        {
    //            completedCount++;
    //        }
    //    }

    //    return completedCount;
    //}
    //public int GetTotalAchievements()
    //{
    //    return achievementData.achievements.Count;
    //}
    //public int GetTotalTrophys()
    //{
    //    int count = 0;
    //    foreach (AchievementTemplate achievement in achievementData.achievements)
    //    {
    //        count = count + achievement.achievementData.Count;
    //    }

    //    return count;
    //}
    //public int GetCompletedTrophys()
    //{
    //    int count = 0;
    //    foreach (AchievementTemplate achievement in achievementData.achievements)
    //    {
    //        foreach (AchievementTemplateDataItem item in achievement.achievementData)
    //        {
    //            if (item.isCompleted)
    //            {
    //                count++;
    //            }
    //        }
    //    }

    //    return count;
    //}
}
