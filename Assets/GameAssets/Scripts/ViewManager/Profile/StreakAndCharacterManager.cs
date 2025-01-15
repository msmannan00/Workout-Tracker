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
    public List<Message> completedMessages = new List<Message>();
    public List<Message> notCompletedMessages = new List<Message>();
    private void Start()
    {
        // Add completed messages with improved titles
        completedMessages.Add(new Message("Victory Unlocked!", "Congratulations! You've completed your weekly goal! Your streak has increased, and you've leveled up. Keep up the great work!"));
        completedMessages.Add(new Message("Champion's Progress!", "Well done! You've crushed your weekly goal! Your streak and level have both increased. Keep the momentum going!"));
        completedMessages.Add(new Message("Level Up Achieved!", "Amazing! You've accomplished your weekly goal! Your streak is stronger, and you've climbed to the next level."));

        // Add not completed messages with improved titles
        notCompletedMessages.Add(new Message("Bounce Back Stronger!", "You missed your weekly goal. Your streak has been reset to 0, and your level has decreased. Don't give up—start fresh and aim for success next week!"));
        notCompletedMessages.Add(new Message("A New Chance Awaits!", "This week didn't go as planned. Your streak is reset, and your level is reduced, but it's a chance to start stronger. You've got this!"));
        notCompletedMessages.Add(new Message("Every Failure is a Step!", "You missed your weekly goal. Your streak has been reset, and your level has dropped slightly. But remember, every setback is a setup for a comeback!"));

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
        //{
        //    // Store gym visit with the current date
        //    string visitKey = "GymVisit_" + startOfCurrentWeek.ToString("yyyy-MM-dd");
        //    // Get the stored visit dates for the current week
        //    List<string> visits = PreferenceManager.Instance.GetStringList(visitKey) ?? new List<string>();
        //    // Get today's date in string format
        //    string today = DateTime.Now.ToString("yyyy-MM-dd");

        //    // Add today's date if it's not already recorded
        //    if (!visits.Contains(today))
        //    {
        //        visits.Add(today);
        //        PreferenceManager.Instance.SetStringList(visitKey, visits);
        //    }
        //    UpdateStreak();
        //}
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        if (!gymVisits.Contains(today))
        {
            gymVisits.Add(today);
            ApiDataHandler.Instance.SetGymVisitsToFirebase(gymVisits);
        }
        UpdateStreak();
    }

    /// <summary>
    /// Updates the streak based on gym visits within the 7-day period.
    /// </summary>
    public void UpdateStreak()
    {
        // Check if the current week has ended
        //startOfCurrentWeek = ApiDataHandler.Instance.GetStartOfCurrentWeek();
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
                userSessionManager.Instance.userStreak= currentStreak;
                ApiDataHandler.Instance.SetUserStreakToFirebase(currentStreak);
                characterLevel=userSessionManager.Instance.characterLevel;
                characterLevel = Mathf.Clamp(characterLevel + 1, 0, 7);
                userSessionManager.Instance.characterLevel= characterLevel;
                ApiDataHandler.Instance.SetCharacterLevelToFirebase(characterLevel);
                ShowRandomMessageOnPopup(true);
                Debug.Log($"Weekly goal met! Streak: {currentStreak}, Level: {characterLevel}");
            }
            else
            {
                // Reset streak and decrease level
                currentStreak = 0;
                userSessionManager.Instance.userStreak = currentStreak;
                ApiDataHandler.Instance.SetUserStreakToFirebase(currentStreak);
                characterLevel = userSessionManager.Instance.characterLevel;
                characterLevel = Mathf.Clamp(characterLevel - 1, 0, 7);
                userSessionManager.Instance.characterLevel = characterLevel;
                ApiDataHandler.Instance.SetCharacterLevelToFirebase((int)characterLevel);
                ShowRandomMessageOnPopup(false);
                Debug.Log($"Weekly goal not met. Streak reset. Level: {characterLevel}");
            }

            // Move start of the week to the next period
            ApiDataHandler.Instance.SetCurrentWeekStartDate(DateTime.Now);
            //startOfCurrentWeek = ApiDataHandler.Instance.GetCurrentWeekStartDate();//GetStartOfWeek(DateTime.Now);
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
    public void ShowRandomMessageOnPopup(bool isGoalCompleted)
    {
        List<Message> messages = isGoalCompleted ? completedMessages : notCompletedMessages;

        if (messages.Count > 0)
        {
            // Randomly select a title and a description
            string randomTitle = messages[UnityEngine.Random.Range(0, messages.Count)].title;
            string randomDescription = messages[UnityEngine.Random.Range(0, messages.Count)].description;

            List<object> initialData = new List<object> { isGoalCompleted, randomTitle, randomDescription};
            PopupController.Instance.OpenPopup("shared", "LevelAndStreakPopup", null, initialData);
        }
    }
}
[System.Serializable]
public class Message
{
    public string title;
    public string description;

    public Message(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
}
