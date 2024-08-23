using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private static Timer instance;

    // Properties for managing the timer
    private float startTime;
    public float elapsedTime;
    private bool isRunning;

    // Text display variable (You can use Unity UI Text or TMPro)
    public UnityEngine.UI.Text timerText; // Ensure to assign this in the inspector

    // Get the singleton instance
    public static Timer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Timer>();
                if (instance == null)
                {
                    GameObject go = new GameObject("Timer");
                    instance = go.AddComponent<Timer>();
                }
            }
            return instance;
        }
    }

    void Start()
    {
        // Initialize
        elapsedTime = 0;
        isRunning = false;
    }
    public void ResetTimer()
    {
        elapsedTime = 0;
    }
    void Update()
    {
        // Update the timer if it is running
        if (isRunning)
        {
            elapsedTime = Time.time - startTime;
            UpdateTextVariable();
        }
    }

    public void StartTimer()
    {
        if (isRunning)
        {
            Debug.LogWarning("Timer is already running.");
            return;
        }

        startTime = Time.time;
        isRunning = true;
    }

    public void StopTimer()
    {
        if (!isRunning)
        {
            Debug.LogWarning("Timer is not running.");
            return;
        }

        isRunning = false;
    }

    private void UpdateTextVariable()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // Format the time as MM:SS
            string formattedTime = string.Format("{0:D2}:{1:D2}", minutes, seconds);

            timerText.text = $"{formattedTime}";
        }
    }
}
