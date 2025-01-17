using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Storage;
using Firebase.Extensions;
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
        UploadImageToFirebase(ConvertSpriteToTexture2D(loadedSprite));

        // Save the image data to persistent storage
        SaveImage(loadedSprite.texture);
        frame.rectTransform.anchoredPosition = new Vector2(0, 0);
        frame.rectTransform.sizeDelta = new Vector2(90, 90);
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

                frame.rectTransform.anchoredPosition = new Vector2(0, 0);
                frame.rectTransform.sizeDelta = new Vector2(90, 90);

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
    public void UploadImageToFirebase(Texture2D texture)
    {
        GlobalAnimator.Instance.FadeInLoader();
        byte[] imageBytes = texture.EncodeToPNG();
        string fileName = "profile_" + FirebaseManager.Instance.user.UserId + ".png";
        StorageReference profileImageRef = FirebaseManager.Instance.storageReference.Child("profile_images/" + fileName);

        profileImageRef.PutBytesAsync(imageBytes).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Image uploaded successfully");
                // Optionally, save the file URL in Firestore or Realtime Database
                GetDownloadUrl(profileImageRef);
            }
            else
            {
                Debug.LogError("Error uploading image: " + task.Exception);
            }
        });
    }
    private void GetDownloadUrl(StorageReference profileImageRef)
    {
        profileImageRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                string downloadUrl = task.Result.ToString();
                Debug.Log("Image URL: " + downloadUrl);
                // Save the URL in Firestore to associate it with the user account
                SaveImageUrlToFirestore(downloadUrl);
            }
        });
    }
    private void SaveImageUrlToFirestore(string url)
    {
        var userRef = FirebaseManager.Instance.databaseReference.Child("users").Child(FirebaseManager.Instance.user.UserId)
            .Child("profileImageUrl");
        userRef.SetValueAsync(url);

        GlobalAnimator.Instance.FadeOutLoader();
    }
    Texture2D ConvertSpriteToTexture2D(Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.LogError("Sprite is null.");
            return null;
        }

        // Get the texture from the sprite
        Texture2D originalTexture = sprite.texture;

        // Get the sprite's pixel data
        Rect spriteRect = sprite.rect;
        Color[] pixels = originalTexture.GetPixels(
            Mathf.FloorToInt(spriteRect.x),
            Mathf.FloorToInt(spriteRect.y),
            Mathf.FloorToInt(spriteRect.width),
            Mathf.FloorToInt(spriteRect.height)
        );

        // Create a new Texture2D and apply the pixels
        Texture2D newTexture = new Texture2D(
            Mathf.FloorToInt(spriteRect.width),
            Mathf.FloorToInt(spriteRect.height)
        );
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        return newTexture;
    }
}
