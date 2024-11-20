using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ExerciseHistoryController : MonoBehaviour, PageController
{
    //public TextMeshProUGUI exerciseNameText;
    //public GameObject singleItem, mileAndTime, weightAndREPS;
    //// for single item
    //public TextMeshProUGUI singleItemTotal;
    //public TextMeshProUGUI singleItemTotalLabel;
    //public TextMeshProUGUI singleItemMax;
    //public TextMeshProUGUI singleItemMaxLabel;
    //// for mile and time
    //public TextMeshProUGUI bestPaceText;
    //public TextMeshProUGUI maxDistanceText;
    //public TextMeshProUGUI maxDurationText;
    //public TextMeshProUGUI totalDistanceText;
    //public TextMeshProUGUI totalDurationText;
    //// for weight and reps
    //public TextMeshProUGUI maxVolumeText;
    //public TextMeshProUGUI maxREPSText;
    //public TextMeshProUGUI totalREPSText;
    //public TextMeshProUGUI maxWeightText;

    public Button backButton;
    public Transform content;

    public List<string> matchedDateTimes = new List<string>();

    public void onInit(Dictionary<string, object> data, Action<object> callback)
    {
        ExerciseDataItem exercise = (ExerciseDataItem)data["data"];
        //exerciseNameText.text = exercise.exerciseName.ToUpper();
        //List<HistoryExerciseModel> exerciseHistory = SearchExerciseByName(ApiDataHandler.Instance.getHistoryData(), exercise.exerciseName);
        List<ExerciseWithDate> _exerciseHistory = _SearchExerciseByName(ApiDataHandler.Instance.getHistoryData(), exercise.exerciseName);
        if (exercise.exerciseType == ExerciseType.WeightAndReps)
        {
            ExerciseWithDate heaviestSet = GetTopPerformance(_exerciseHistory, HistoryPerformance.HeaviestLifted);
            foreach (var item in _exerciseHistory)
            {
                if (item == heaviestSet)
                {
                    item.performance = HistoryPerformance.HeaviestLifted;
                    break;
                }
            }
            ExerciseWithDate bestSet = GetTopPerformance(_exerciseHistory, HistoryPerformance.BestSet);
            foreach (var item in _exerciseHistory)
            {
                if (item == bestSet)
                {
                    if (item.performance == HistoryPerformance.None)
                        item.performance = HistoryPerformance.BestSet;
                    else
                        item.performance |= HistoryPerformance.BestSet;
                    break;
                }
            }
        }
        backButton.onClick.AddListener(Back);
        backButton.onClick.AddListener(AudioController.Instance.OnButtonClick);
        AddItems(_exerciseHistory);
        //switch (exercise.exerciseType)
        //{
        //    case ExerciseType.RepsOnly:
        //        singleItem.SetActive(true);
        //        singleItemMaxLabel.text = "Max reps";
        //        singleItemTotalLabel.text = "Total reps";
        //        ShowRepsOnly(exerciseHistory);
        //        break;
        //    case ExerciseType.TimeBased:
        //        singleItem.SetActive(true);
        //        singleItemMaxLabel.text = "Max time";
        //        singleItemTotalLabel.text = "Total time";
        //        ShowTimeOnly(exerciseHistory);
        //        break;
        //    case ExerciseType.TimeAndMiles:
        //        mileAndTime.SetActive(true);
        //        CalculateExerciseMileAndTime(exerciseHistory);
        //        break;
        //    case ExerciseType.WeightAndReps:
        //        weightAndREPS.SetActive(true);
        //        CalculateWeightRepsStats(exerciseHistory);
        //        break;
        //}
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public void AddItems(List<ExerciseWithDate> history)
    {
        foreach(var item in history)
        {
            Dictionary<string, object> mData = new Dictionary<string, object>
                {
                    { "history", item }
                };
            GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/history/exerciseHistoryDataModel");
            GameObject exerciseObject = Instantiate(exercisePrefab, content);
            exerciseObject.GetComponent<ItemController>().onInit(mData, null);
        }
    }
    //void ShowRepsOnly(List<HistoryExerciseModel> history)
    //{
    //    if (history == null || history.Count == 0)
    //    {
    //        Console.WriteLine("No exercises found.");
    //        return;
    //    }

    //    int totalReps = 0;
    //    int maxReps = int.MinValue;

    //    foreach (var exercise in history)
    //    {
    //        // Accumulate total reps
    //        totalReps += exercise.reps;

    //        // Find the max reps
    //        if (exercise.reps > maxReps)
    //        {
    //            maxReps = exercise.reps;
    //        }
    //    }
    //    singleItemTotal.text = totalReps.ToString();
    //    singleItemMax.text = maxReps.ToString();
    //}
    //void ShowTimeOnly(List<HistoryExerciseModel> history)
    //{
    //    if (history == null || history.Count == 0)
    //    {
    //        Console.WriteLine("No exercises found.");
    //        return;
    //    }

    //    int totalTime = 0;
    //    int bestTime = int.MaxValue;  // We use int.MaxValue to find the lowest time

    //    // Iterate through the list and calculate total time and best time
    //    foreach (var exercise in history)
    //    {
    //        totalTime += exercise.time;

    //        if (exercise.time < bestTime)
    //        {
    //            bestTime = exercise.time;
    //        }
    //    }

    //    singleItemTotal.text = FormatTime(totalTime);
    //    singleItemMax.text = FormatTime(totalTime);
    //}

    //public void CalculateExerciseMileAndTime(List<HistoryExerciseModel> exerciseList)
    //{
    //    if (exerciseList == null || exerciseList.Count == 0)
    //    {
    //        Console.WriteLine("No exercise data available.");
    //        return;
    //    }

    //    float totalDistance = 0f;  // Total miles
    //    int totalDuration = 0;     // Total time in seconds
    //    float maxDistance = float.MinValue;
    //    int maxDuration = int.MinValue;
    //    float bestPace = float.MaxValue; // Pace: time per distance (seconds per mile)

    //    // Loop through the exercise list to calculate the statistics
    //    foreach (var exercise in exerciseList)
    //    {
    //        totalDistance += exercise.mile;
    //        totalDuration += exercise.time;

    //        if (exercise.mile > maxDistance)
    //        {
    //            maxDistance = exercise.mile;
    //        }

    //        if (exercise.time > maxDuration)
    //        {
    //            maxDuration = exercise.time;
    //        }

    //        // Calculate pace and update best pace if current pace is better
    //        if (exercise.mile > 0)
    //        {
    //            float currentPace = (float)exercise.time / exercise.mile;  // Pace = time (seconds) per mile
    //            if (currentPace < bestPace)
    //            {
    //                bestPace = currentPace;
    //            }
    //        }
    //    }
    //    bestPaceText.text = FormatPace(bestPace) + " / mile";
    //    maxDistanceText.text = maxDistance.ToString() + " miles";
    //    maxDurationText.text = FormatTime(totalDuration);
    //    totalDistanceText.text = totalDistance.ToString() + " miles";
    //    totalDurationText.text = FormatTime(totalDuration);
    //}

    //public void CalculateWeightRepsStats(List<HistoryExerciseModel> exerciseList)
    //{
    //    if (exerciseList == null || exerciseList.Count == 0)
    //    {
    //        Console.WriteLine("No exercise data available.");
    //        return;
    //    }

    //    float maxWeight = float.MinValue;
    //    int maxReps = int.MinValue;
    //    int totalReps = 0;
    //    float maxVolume = float.MinValue;
    //    float totalVolume = 0f;
    //    List<TextMeshProUGUI> newTexts = new List<TextMeshProUGUI>();
    //    // Loop through the exercise list to calculate the statistics
    //    foreach (var exercise in exerciseList)
    //    {
    //        // Update total reps
    //        totalReps += exercise.reps;

    //        // Check for max weight
    //        if (exercise.weight > maxWeight)
    //        {
    //            maxWeight = exercise.weight;
    //        }

    //        // Check for max reps
    //        if (exercise.reps > maxReps)
    //        {
    //            maxReps = exercise.reps;
    //        }

    //        // Calculate volume for this set (weight * reps)
    //        float currentVolume = exercise.weight * exercise.reps;
    //        totalVolume += currentVolume;

    //        // Update max volume if the current volume is higher
    //        if (currentVolume > maxVolume)
    //        {
    //            maxVolume = currentVolume;
    //        }

    //        GameObject exercisePrefab = Resources.Load<GameObject>("Prefabs/history/exerciseHistoryItem");
    //        GameObject newExerciseObject = Instantiate(exercisePrefab, content);
    //        switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
    //        {
    //            case WeightUnit.kg:
    //                newExerciseObject.GetComponent<TextMeshProUGUI>().text = exercise.weight.ToString() + "kg x " + exercise.reps.ToString();
    //                break;
    //            case WeightUnit.lbs:
    //                newExerciseObject.GetComponent<TextMeshProUGUI>().text = (userSessionManager.Instance.ConvertKgToLbs(exercise.weight)).ToString("F2") + "lbs x " + exercise.reps.ToString();
    //                break;
    //        }
    //        //newExerciseObject.GetComponent<TextMeshProUGUI>().text = exercise.weight.ToString() + "kg x " + exercise.reps.ToString();
    //        newTexts.Add(newExerciseObject.GetComponent<TextMeshProUGUI>());
    //    }
    //    for (int i = 0; i < newTexts.Count; i++)
    //    {
    //        DateTime parsedDate = DateTime.ParseExact(matchedDateTimes[i], "MMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //        string formattedDate = parsedDate.ToString("dddd, dd MMMM yyyy");
    //        newTexts[i].text += "\t" + formattedDate;
    //    }
    //    switch ((WeightUnit)ApiDataHandler.Instance.GetWeightUnit())
    //    {
    //        case WeightUnit.kg:
    //            maxWeightText.text = maxWeight.ToString() + " kg";
    //            maxVolumeText.text = totalVolume.ToString() + " kg";
    //            break;
    //        case WeightUnit.lbs:
    //            maxWeightText.text = /*Mathf.RoundToInt*/(userSessionManager.Instance.ConvertKgToLbs(maxWeight)).ToString("F2") + " lbs";
    //            maxVolumeText.text = /*Mathf.RoundToInt*/(userSessionManager.Instance.ConvertKgToLbs(totalVolume)).ToString("F2") + " lbs";
    //            break;
    //    }
    //    maxREPSText.text = maxReps.ToString();
    //    totalREPSText.text = totalReps.ToString();
    //    //maxWeightText.text = maxWeight.ToString() + " kg";
    //    //maxVolumeText.text = totalVolume.ToString()+" kg";
    //}

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

    public List<ExerciseWithDate> _SearchExerciseByName(HistoryModel history, string exerciseName)
    {
        List<ExerciseWithDate> matchedExercises = new List<ExerciseWithDate>();

        foreach (var template in history.exerciseTempleteModel)
        {
            foreach (var exerciseType in template.exerciseTypeModel)
            {
                if (exerciseType.exerciseName.Equals(exerciseName, StringComparison.OrdinalIgnoreCase))
                {
                    var exerciseTemplate = new ExerciseWithDate
                    {
                        dateTime = template.dateTime,
                        exerciseType = exerciseType.exerciseType
                    };
                    foreach (var exercise in exerciseType.exerciseModel)
                    {
                        exerciseTemplate.exercise.Add(exercise);
                    }
                    matchedExercises.Add((exerciseTemplate));
                }
            }
        }

        return matchedExercises;
    }

    public ExerciseWithDate GetTopPerformance(List<ExerciseWithDate> exercises, HistoryPerformance performanceType)
    {
        ExerciseWithDate topPerformance = null;

        switch (performanceType)
        {
            case HistoryPerformance.HeaviestLifted:
                topPerformance = exercises
                    .OrderByDescending(e => e.GetHeaviestLiftedSet()?.weight ?? 0)
                    .FirstOrDefault();
                break;

            case HistoryPerformance.BestSet:
                topPerformance = exercises
                    .OrderByDescending(e =>
                    {
                        var bestSet = e.GetBestSet();
                        if (bestSet == null) return 0; // Handle null case for GetBestSet
                        return bestSet.reps == 1 ? bestSet.weight : bestSet.weight * (1 + 0.0333f * bestSet.reps);
                    })
                    .FirstOrDefault();
                break;

                // Add cases for other performance types if needed
        }

        return topPerformance;
    }

}
