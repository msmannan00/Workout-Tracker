using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
public class FirebaseExerciseManager : GenericSingletonClass<FirebaseExerciseManager>
{
    public ExerciseData exerciseData = new ExerciseData();

    public DatabaseReference databaseReference;

    public void Load()
    {
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result == Firebase.DependencyStatus.Available)
            {
                // Firebase is ready
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                RetrieveExerciseData();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }

    // Deserialize JSON to ExerciseData object
    public void LoadExerciseData(string jsonData)
    {
        print("load data");
        exerciseData = JsonUtility.FromJson<ExerciseData>(jsonData);
        print("Load data2");
    }

    // Example function to retrieve data from Firebase
    public void RetrieveExerciseData()
    {
        databaseReference.Child("exerciseData").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string jsonData = snapshot.GetRawJsonValue();
                print(jsonData);
                LoadExerciseData(jsonData);
                Debug.Log($"Retrieved data: {jsonData}");
            }
            else
            {
                Debug.LogError("Failed to retrieve data.");
            }
        });
    }

    // Example function to add a new exercise and save it to Firebase
    public void AddNewExercise(ExerciseDataItem newExercise)
    {
        exerciseData.exercises.Add(newExercise);
        SaveExerciseData();
    }

    // Save data to Firebase
    public void SaveExerciseData()
    {
        string jsonData = JsonUtility.ToJson(exerciseData);
        databaseReference.Child("exerciseData").SetRawJsonValueAsync(jsonData);
    }
}
