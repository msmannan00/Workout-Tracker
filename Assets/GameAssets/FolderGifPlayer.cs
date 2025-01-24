using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FolderGifPlayer : MonoBehaviour
{
    public string mainFolderPath = "MainFolder"; // Path to the main folder in Resources
    public Image displayImage;                  // UI Image to display sprites
    public float frameDelay = 0.1f;             // Delay between frames

    public List<string> folderNames;            // List of folder names
    private int currentFolderIndex = 0;         // Current folder index
    private List<Sprite> currentSprites;        // Sprites from the current folder
    private int currentFrameIndex = 0;          // Current frame index
    private Coroutine playCoroutine;

    private Dictionary<string, List<Sprite>> folderSprites; // Cache for preloaded folder sprites
    private WaitForSeconds waitDelay;

    private void Start()
    {
        waitDelay = new WaitForSeconds(frameDelay); // Cache the wait delay
        //PreloadAllFolders(); // Preload sprites for all folders

        if (folderNames.Count > 0)
        {
            LoadFolderSprites(folderNames[currentFolderIndex]);
            StartGifPlayback();
        }
        else
        {
            Debug.LogError("No folders found in the main folder.");
        }
    }

    private void PreloadAllFolders()
    {
        folderSprites = new Dictionary<string, List<Sprite>>();
        foreach (var folderName in folderNames)
        {
            var sprites = Resources.LoadAll<Sprite>($"{mainFolderPath}/{folderName}");
            if (sprites.Length > 0)
            {
                folderSprites[folderName] = new List<Sprite>(sprites);
            }
            else
            {
                Debug.LogWarning($"No sprites found in folder: {folderName}");
            }
        }
    }

    private void LoadFolderSprites(string folderName)
    {
        if (playCoroutine != null) StopCoroutine(playCoroutine); // Stop ongoing playback

        if (folderSprites.TryGetValue(folderName, out currentSprites))
        {
            currentFrameIndex = 0;
        }
        else
        {
            Debug.LogError($"Folder not preloaded: {folderName}");
            currentSprites = new List<Sprite>();
        }
    }

    private void StartGifPlayback()
    {
        if (currentSprites == null || currentSprites.Count == 0) return;
        playCoroutine = StartCoroutine(PlayGif());
    }

    private IEnumerator PlayGif()
    {
        while (true)
        {
            if (currentSprites.Count > 0)
            {
                displayImage.sprite = currentSprites[currentFrameIndex];
                currentFrameIndex = (currentFrameIndex + 1) % currentSprites.Count;
                yield return null;
            }
        }
    }

    public void OnNextFolderButton()
    {
        if (folderNames.Count == 0) return;

        currentFolderIndex = (currentFolderIndex + 1) % folderNames.Count;
        LoadFolderSprites(folderNames[currentFolderIndex]);
        StartGifPlayback();
    }
}
