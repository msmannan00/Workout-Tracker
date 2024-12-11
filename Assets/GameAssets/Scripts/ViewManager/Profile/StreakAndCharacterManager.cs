using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakAndCharacterManager : GenericSingletonClass<StreakAndCharacterManager>
{
    [Header("Settings")]
    public int weeklyGoal = 2; // Weekly workout goal (e.g., 2x per week)

    [Header("Inspector Variables")]
    public DateTime startOfCurrentWeek; // Start of the current week
    public List<string> gymVisits = new List<string>(); // Gym visit dates
    public int currentStreak = 0; // User's current streak
    public int characterLevel = 0;
    public string enfOfWeekDate;

    private void Start()
    {
        // Initialize start of the week based on today's date
        startOfCurrentWeek = ApiDataHandler.Instance.GetStartOfCurrentWeek(); //GetStartOfWeek(DateTime.Now);
    }

    /// <summary>
    /// Adds a gym visit for a given date.
    /// </summary>
    public void AddVisit(string date)
    {
        //// for testing
        //if (DateTime.TryParse(date, out DateTime parsedDate))
        //{
        //    string visitDate = parsedDate.ToString("yyyy-MM-dd");

        //    // Add visit if it's not already logged
        //    if (!gymVisits.Contains(visitDate))
        //    {
        //        gymVisits.Add(visitDate);
        //        Debug.Log($"Gym visit added: {visitDate}");
        //    }

        //    // Update streak after adding a visit
        //    UpdateStreak();
        //}
        //else
        {
            // Store gym visit with the current date
            string visitKey = "GymVisit_" + ApiDataHandler.Instance.GetCurrentWeekStartDate().ToString("yyyy-MM-dd");
            // Get the stored visit dates for the current week
            List<string> visits = PreferenceManager.Instance.GetStringList(visitKey) ?? new List<string>();
            // Get today's date in string format
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            // Add today's date if it's not already recorded
            if (!visits.Contains(today))
            {
                visits.Add(today);
                PreferenceManager.Instance.SetStringList(visitKey, visits);
            }
            UpdateStreak();
        }
    }

    /// <summary>
    /// Updates the streak based on gym visits within the 7-day period.
    /// </summary>
    public void UpdateStreak()
    {
        // Check if the current week has ended
        startOfCurrentWeek = ApiDataHandler.Instance.GetStartOfCurrentWeek();
        DateTime endOfCurrentWeek = startOfCurrentWeek.AddDays(7);
        print(startOfCurrentWeek.ToString("yyyy-MM-dd"));
        print(endOfCurrentWeek.ToString("yyyy-MM-dd"));
        if (DateTime.TryParse(enfOfWeekDate, out DateTime parsedDate))
        {
            DateTime _now = parsedDate;
        }
        else
            parsedDate = DateTime.Now;
        if (parsedDate >= endOfCurrentWeek)
        {
            // Check if the user met the goal in the last 7-day period
            if (HasMetWeeklyGoal())
            {
                // Increment streak and level up character
                currentStreak=userSessionManager.Instance.userStreak;
                currentStreak++;
                ApiDataHandler.Instance.SetUserStreakToFirebase(currentStreak);
                characterLevel=ApiDataHandler.Instance.GetCharacterLevel();
                characterLevel++;
                ApiDataHandler.Instance.SetCharacterLevel(characterLevel);
                Debug.Log($"Weekly goal met! Streak: {currentStreak}, Level: {characterLevel}");
            }
            else
            {
                // Reset streak and decrease level
                ApiDataHandler.Instance.SetUserStreakToFirebase(0);
                characterLevel = ApiDataHandler.Instance.GetCharacterLevel();
                characterLevel = Math.Max(0, characterLevel - 1); // Character level should not go below 1
                ApiDataHandler.Instance.SetCharacterLevel((int)characterLevel);
                Debug.Log($"Weekly goal not met. Streak reset. Level: {characterLevel}");
            }

            // Move start of the week to the next period
            ApiDataHandler.Instance.SetCurrentWeekStartDate(DateTime.Now);
            startOfCurrentWeek = ApiDataHandler.Instance.GetCurrentWeekStartDate();//GetStartOfWeek(DateTime.Now);
        }
    }

    /// <summary>
    /// Checks if the user has met the weekly goal based on visits in the 7-day period.
    /// </summary>
    public bool HasMetWeeklyGoal()
    {
        int visitCount = 0;

        foreach (string visit in gymVisits)
        {
            if (DateTime.TryParse(visit, out DateTime visitDate) &&
                visitDate >= startOfCurrentWeek &&
                visitDate < startOfCurrentWeek.AddDays(7))
            {
                visitCount++;
            }
        }
        weeklyGoal = userSessionManager.Instance.weeklyGoal;
        Debug.Log($"Visits in current 7-day period: {visitCount}/{weeklyGoal}");
        return visitCount >= weeklyGoal;
    }

    /// <summary>
    /// Gets the start date of the week for a given date (Monday as the start of the week).
    /// </summary>
    private DateTime GetStartOfWeek(DateTime date)
    {
        int daysToSubtract = (int)date.DayOfWeek - (int)DayOfWeek.Monday;
        if (daysToSubtract < 0) daysToSubtract += 7; // Adjust for Sunday
        return date.AddDays(-daysToSubtract).Date;
    }

    /// <summary>
    /// Sets the start of the week manually (for testing).
    /// </summary>
    public void SetStartOfWeek(string date)
    {
        if (DateTime.TryParse(date, out DateTime parsedDate))
        {
            startOfCurrentWeek = GetStartOfWeek(parsedDate);
            Debug.Log($"Week Start Date set to: {startOfCurrentWeek:yyyy-MM-dd}");
        }
        else
        {
            Debug.LogError("Invalid date format. Use yyyy-MM-dd.");
        }
    }
}
