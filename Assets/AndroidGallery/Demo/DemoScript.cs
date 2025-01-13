using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//NOTE: we're using LukeWaffel.AndroidGallery, without this it won't work
using LukeWaffel.AndroidGallery;
using System.IO; // For file operations

public class DemoScript : MonoBehaviour {

    [Header("References")]
    public Image frame;

    private string imagePath;

    void Start()
    {
        // Define a path to save the last loaded image
        imagePath = Path.Combine(Application.persistentDataPath, "lastImage.png");

        // Load the last image if it exists
        LoadLastImage();
    }

    // This function is called by the Button
    public void OpenGalleryButton()
    {
        // Open the Android image picker
        AndroidGallery.Instance.OpenGallery(ImageLoaded);
    }

    // Callback function for when the image is loaded
    public void ImageLoaded()
    {
        Debug.Log("The image has successfully loaded!");

        // Get the loaded sprite
        Sprite loadedSprite = AndroidGallery.Instance.GetSprite();
        frame.sprite = loadedSprite;

        // Save the image data to persistent storage
        SaveImage(loadedSprite.texture);
    }

    // Save the texture to a file
    private void SaveImage(Texture2D texture)
    {
        // Convert the texture to PNG format
        byte[] imageBytes = texture.EncodeToPNG();

        // Save the image to the file system
        File.WriteAllBytes(imagePath, imageBytes);

        Debug.Log($"Image saved to: {imagePath}");
    }

    // Load the last image from storage
    private void LoadLastImage()
    {
        if (File.Exists(imagePath))
        {
            // Load the image bytes from file
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            // Create a Texture2D from the bytes
            Texture2D texture = new Texture2D(2, 2); // Placeholder size; will be replaced when loading
            if (texture.LoadImage(imageBytes))
            {
                // Create a sprite from the loaded texture
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                frame.sprite = sprite;

                Debug.Log("Last image loaded successfully!");
            }
            else
            {
                Debug.LogError("Failed to load image from saved data.");
            }
        }
        else
        {
            Debug.Log("No previously saved image found.");
        }
    }

    // This function exits the app
    public void Exit()
    {
        Application.Quit();
    }
}
