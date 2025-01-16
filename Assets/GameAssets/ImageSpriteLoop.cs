using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class ImageSpriteLoop : MonoBehaviour
{
    [Header("Sprite Settings")]
    public Sprite[] sprites; // Array of sprites
    public float frameRate = 0.1f; // Time per frame (in seconds)
    public bool loop = true; // Should the animation loop?

    private Image image; // Reference to the UI Image component
    private int currentFrame = 0; // Current frame index
    private float timer = 0f; // Timer to control frame rate

    void Start()
    {
        // Get the Image component attached to the GameObject
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("No Image component found! Please attach this script to a GameObject with an Image component.");
            //enabled = false;
            return;
        }

        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("Sprite array is empty or not assigned.");
            //enabled = false;
        }
    }

    void Update()
    {
        if (sprites == null || sprites.Length == 0 || (!loop && currentFrame >= sprites.Length))
            return;

        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to switch to the next frame
        if (timer >= frameRate)
        {
            timer -= frameRate; // Reset timer for the next frame
            currentFrame++; // Move to the next frame

            // Loop back to the first frame if necessary
            if (currentFrame >= sprites.Length)
            {
                if (loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame = sprites.Length - 1; // Stop at the last frame
                    return;
                }
            }

            // Set the sprite for the current frame
            image.sprite = sprites[currentFrame];
            image.SetNativeSize();
        }
    }

    public void ResetGif(Sprite[] sprites)
    {
        this.sprites = null;
        timer= 0f;
        currentFrame = 0;
        this.sprites = sprites;
    }
}
