using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{
    public Sprite[] sprites; // Array to hold your sprites
    public float frameRate = 0.1f; // Time between frames
    private Image image;
    private int currentFrame;
    private float timer;

    void Start()
    {
        image = GetComponent<Image>();
        if (sprites.Length == 0)
        {
            Debug.LogWarning("No sprites assigned to the animation!");
            return;
        }
    }

    void Update()
    {
        if (sprites.Length == 0) return;

        // Update the timer
        timer += Time.deltaTime;

        // Change frame based on frame rate
        if (timer >= frameRate)
        {
            timer = 0f; // Reset timer
            currentFrame++; // Move to the next frame
            if (currentFrame >= sprites.Length)
            {
                currentFrame = 0; // Loop back to the first frame
            }

            // Assign the current sprite
            image.sprite = sprites[currentFrame];
        }
    }
}
