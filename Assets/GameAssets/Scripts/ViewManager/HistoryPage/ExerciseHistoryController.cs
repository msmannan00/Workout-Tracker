using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
public class ExerciseHistoryController : MonoBehaviour, PageController
{
    public TextMeshProUGUI exerciseNameText;
    public GameObject singleItem, mileAndTime, weightAndREPS;
    // for single item
    public TextMeshProUGUI singleItemTotal;
    public TextMeshProUGUI singleItemTotalLabel;
    public TextMeshProUGUI singleItemMax;
    public TextMeshProUGUI singleItemMaxLabel;
    // for mile and time
    public TextMeshProUGUI bestPaceText;
    public TextMeshProUGUI maxDistanceText;
    public TextMeshProUGUI maxDurationText;
    public TextMeshProUGUI totalDistanceText;
    public TextMeshProUGUI totalDurationText;
    // for weight and reps
    public TextMeshProUGUI maxVolumeText;
    public TextMeshProUGUI maxREPSText;
    public TextMeshProUGUI totalREPSText;
    public TextMeshProUGUI maxWeightText;
    public Transform content;

    public List<string> matchedDateTimes = new List<string>();

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        ExerciseDataItem exercise = (ExerciseDataItem)data["data"];
        exerciseNameText.text = exercise.exerciseName.ToUpper();
        List<HistoryExerciseModel> exerciseHistory = SearchExerciseByName(userSessionManager.Instance.historyData, exercise.exerciseName);

        switch (exercise.exerciseType)
        {
            case ExerciseType.RepsOnly:
                singleItem.SetActive(true);
                singleItemMaxLabel.text = "Max reps";
                singleItemTotalLabel.text = "Total reps";
                ShowRepsOnly(exerciseHistory);
                break;
            case ExerciseType.TimeBased:
                singleItem.SetActive(true);
                singleItemMaxLabel.text = "Max time";
                singleItemTotalLabel.text = "Total time";
                ShowTimeOnly(exerciseHistory);
                break;
            case ExerciseType.TimeAndMiles:
                mileAndTime.SetActive(true);
                CalculateExerciseMileAndTime(exerciseHistory);
                break;
            case ExerciseType.WeightAndReps:
                weightAndREPS.SetActive(true);
                CalculateWeightRepsStats(exerciseHistory);
                break;
        }
    }
    void ShowRepsOnly(List<HistoryExerciseModel> history)
    {
        if (history == null || history.Count == 0)
        {
            Console.WriteLine("No exercises found.");
            return;
        }

        int totalReps = 0;
        int maxReps = int.MinValue;

        foreach (var exercise in history)
        {
            // Accumulate total reps
            totalReps += exercise.reps;

            // Find the max reps
            if (exercise.reps > maxReps)
            {
                maxReps = exercise.reps;
            }
        }
        singleItemTotal.text= totalReps.ToString();
        singleItemMax.text= maxReps.ToString();
    }
    void ShowTimeOnly(List<HistoryExerciseModel> history)
    {
        if (history == null || history.Count == 0)
        {
            Console.WriteLine("No exercises found.");
            return;
        }

        int totalTime = 0;
        int bestTime = int.MaxValue;  // We use int.MaxValue to find the lowest time

        // Iterate through the list and calculate total time and best time
        foreach (var exercise in history)
        {
            totalTime += exercise.time;

            if (exercise.time < bestTime)
            {
                bestTime = exercise.time;
            }
        }

        singleItemTotal.text = FormatTime(totalTime);
        singleItemMax.text = FormatTime(totalTime);
    }

    public void CalculateExerciseMileAndTime(List<HistoryExerciseModel> exerciseList)
    {
        if (exerciseList == null || exerciseList.Count == 0)
        {
            Console.WriteLine("No exercise data available.");
            return;
        }

        float totalDistance = 0f;  // Total miles
        int totalDuration = 0;     // Total time in seconds
        float maxDistance = float.MinValue;
        int maxDuration = int.MinValue;
        float bestPace = float.MaxValue; // Pace: time per distance (seconds per mile)

        // Loop through the exercise list to calculate the statistics
        foreach (var exercise in exerciseList)
        {
            totalDistance += exercise.mile;
            totalDuration += exercise.time;

            if (exercise.mile > maxDistance)
            {
                maxDistance = exercise.mile;
            }

            if (exercise.time > maxDuration)
            {
                maxDuration = exercise.time;
            }

            // Calculate pace and update best pace if current pace is better
            if (exercise.mile > 0)
            {
                float currentPace = (float)exercise.time / exercise.mile;  // Pace = time (seconds) per mile
                if (currentPace < bestPace)
                {
                    bestPace = currentPace;
                }
            }
        }
        bestPaceText.text = FormatPace(bestPace) + " / mile";
        maxDistanceText.text = maxDistance.ToString() + " miles";
        maxDurationText.text = FormatTime(totalDuration);
        totalDistanceText.text = totalDistance.ToString() + " miles";
        totalDurationText.text = FormatTime(totalDuration);
    }

    public void CalculateWeightRepsStats(List<HistoryExerciseModel> exerciseList)
    {
        if (exerciseList == null || exerciseList.Count == 0)
        {
            Console.WriteLine("No exercise data available.");
            return;
        }

        float maxWeight = float.MinValue;
        int maxReps = int.MinValue;
        int totalReps = 0;
        float maxVolume = float.MinValue;
        float totalVolume = 0f;
        List<TextMeshProUGUI> newTexts = new List<TextMeshProUGUI>();
        // Loop through the exercise list to calculate the statistics
        foreach (var exercise in exerciseList)
        {
            // Update total reps
            totalReps += exercise.reps;

            // Check for max weight
            if (exercise.weight > maxWeight)
            {
                maxWeight = exercise.weight;
            }

            // Check for max reps
            if (exercise.reps > maxReps)
            {
                maxReps = exercise.reps;
            }

            // Calculate volume for this set (weight * reps)
            float currentVolume = exercise.weight * exercise.reps;
            totalVolume += currentVolume;

            // Update max volume if the current volume is higher
            if (currentVolume > maxVolume)
            {
                maxVolume = currentVolume;
            }

            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/history/exerciseHistoryItem");
            GameObject newExerciseObject = Instantiate(exercisePrefab, content);
            newExerciseObject.GetComponent<TextMeshProUGUI>().text = exercise.weight.ToString() + "kg x " + exercise.reps.ToString();
            newTexts.Add(newExerciseObject.GetComponent<TextMeshProUGUI>());
        }
        for (int i = 0; i < newTexts.Count; i++)
        {
            DateTime parsedDate = DateTime.ParseExact(matchedDateTimes[i], "MMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedDate = parsedDate.ToString("dddd, dd MMMM yyyy");
            newTexts[i].text += "\t" + formattedDate;
        }
        maxWeightText.text = maxWeight.ToString() + " kg";
        maxREPSText.text = maxReps.ToString();
        totalREPSText.text = totalReps.ToString();
        maxVolumeText.text = totalVolume.ToString()+" kg";
    }

    // Helper function to format time
    private string FormatTime(int timeInSeconds)
    {
        if (timeInSeconds < 60)
        {
            return $"{timeInSeconds} seconds";
        }
        else
        {
            int minutes = timeInSeconds / 60;
            int seconds = timeInSeconds % 60;
            return $"{minutes} min {seconds} sec";
        }
    }

    // Helper function to format pace (time per mile)
    private string FormatPace(float pace)
    {
        if (pace < 60)
        {
            return $"{pace:F2} seconds";
        }
        else
        {
            int minutes = (int)pace / 60;
            int seconds = (int)pace % 60;
            return $"{minutes} min {seconds} sec";
        }
    }
    public void Back()
    {
        StateManager.Instance.HandleBackAction(gameObject);
        StateManager.Instance.OpenFooter(null, null, false);
    }
    public List<HistoryExerciseModel> _SearchExerciseByName(HistoryModel history, string exerciseName)
    {
        List<HistoryExerciseModel> result = new List<HistoryExerciseModel>();
        foreach (var templete in history.exerciseTempleteModel)
        {
            foreach (var exerciseType in templete.exerciseTypeModel)
            {
                if (exerciseType.exerciseName == exerciseName)
                {
                    result.AddRange(exerciseType.exerciseModel);
                }
            }
        }
        return result;
    }
    public List<HistoryExerciseModel> SearchExerciseByName(HistoryModel history, string exerciseName)
    {
        List<HistoryExerciseModel> matchedExercises = new List<HistoryExerciseModel>();
        matchedDateTimes.Clear();
        foreach (var template in history.exerciseTempleteModel)
        {
            foreach (var exerciseType in template.exerciseTypeModel)
            {
                if (exerciseType.exerciseName.ToUpper() == exerciseName.ToUpper())
                {
                    foreach (var exercise in exerciseType.exerciseModel)
                    {
                        matchedExercises.Add(exercise);
                        matchedDateTimes.Add(template.dateTime);
                    }
                }
            }
        }
        return matchedExercises;
    }
}
