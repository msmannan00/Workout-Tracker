using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Storage;
using Firebase.Extensions;
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
        if (NativeGallery.IsMediaPickerBusy())
        {
            Debug.LogWarning("Gallery is busy, please wait.");
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = LoadTextureFromPath(path);
                if (texture != null)
                {
                    Sprite loadedSprite = TextureToSprite(texture);
                    frame.sprite = loadedSprite;

                    // Save the image data to persistent storage
                    SaveImage(texture);

                    // Upload the image to Firebase
                    UploadImageToFirebase(texture);

                    frame.rectTransform.anchoredPosition = new Vector2(0, 0);
                    frame.rectTransform.sizeDelta = new Vector2(90, 90);

                    Debug.Log("Image successfully loaded from gallery and processed.");
                }
                else
                {
                    Debug.LogError("Failed to load image from path: " + path);
                }
            }
        }, "Select an image", "image/*");

        if (permission != NativeGallery.Permission.Granted)
        {
            Debug.LogWarning("Permission not granted to access the gallery.");
        }
    }

    // Save the texture to a file
    private void SaveImage(Texture2D texture)
    {
        byte[] imageBytes = texture.EncodeToPNG();
        File.WriteAllBytes(imagePath, imageBytes);
        Debug.Log($"Image saved to: {imagePath}");
    }

    // Load the last image from storage
    private void LoadLastImage()
    {
        if (File.Exists(imagePath))
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageBytes))
            {
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

    // Convert a texture to a sprite
    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    // Load a texture from a file path
    private Texture2D LoadTextureFromPath(string path)
    {
        byte[] imageBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageBytes))
        {
            return texture;
        }
        return null;
    }

    // Upload the image to Firebase
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
}
