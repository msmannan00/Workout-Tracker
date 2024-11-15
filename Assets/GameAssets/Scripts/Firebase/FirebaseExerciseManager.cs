using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using Firebase.Auth;
public class FirebaseExerciseManager : GenericSingletonClass<FirebaseExerciseManager>
{
    public ExerciseData exerciseData = new ExerciseData();

    public DatabaseReference databaseReference;

    public void Load()
    {
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            print("FirebaseApp");
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
        exerciseData = JsonUtility.FromJson<ExerciseData>(jsonData);
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
                LoadExerciseData(jsonData);
                Debug.Log($"Retrieved data: {jsonData}");
                AddExerciseForUser(userSessionManager.Instance.mProfileUsername, exerciseData);
            }
            else
            {
                Debug.LogError("Failed to retrieve data.");
            }
        });
        
    }
    public void AddExerciseForUser(string userId, ExerciseData exercise)
    {
        print("AddExerciseForUser start");
        ExerciseData ex=new ExerciseData();
        ex.exercises.Add(exercise.exercises[1]);
        string json = JsonUtility.ToJson(ex);
        databaseReference.Child("users").Child(userId).Child("exercises").SetRawJsonValueAsync(json).ContinueWith(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Exercise added to user's exercises sub-child");
            }
            else
            {
                Debug.LogError("Failed to add exercise: " + task.Exception);
            }
        });
        AddOrUpdateExerciseForUser(userId, exercise.exercises[2]);
    }

    public void AddOrUpdateExerciseForUser(string userId, ExerciseDataItem exerciseItem)
    {
        // Convert the single exercise item to JSON
        string json = JsonUtility.ToJson(exerciseItem);

        // Reference the "exercises" node directly
        var exercisesRef = databaseReference.Child("users").Child(userId).Child("exercises").Child("exercises");

        // Fetch current count, then add a new item at the next index
        exercisesRef.GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int nextIndex = (int)snapshot.ChildrenCount;  // Get the current count to use as the next index

                // Add the new exercise at the next index
                exercisesRef.Child(nextIndex.ToString()).SetRawJsonValueAsync(json).ContinueWith(addTask => {
                    if (addTask.IsCompleted)
                    {
                        Debug.Log("Exercise added at index: " + nextIndex);
                    }
                    else
                    {
                        Debug.LogError("Failed to add exercise: " + addTask.Exception);
                    }
                });
            }
            else
            {
                Debug.LogError("Failed to get exercises: " + task.Exception);
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
