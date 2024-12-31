using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase.Storage;
using Firebase.Extensions;
using System.IO;

public class ProfileImagePicker : MonoBehaviour
{
    //public string FinalPath;
    public Image profileImage;
    private void OnEnable()
    {
        if(userSessionManager.Instance.profileSprite != null)
        {
            profileImage.sprite = userSessionManager.Instance.profileSprite;
        }
    }
    public void PickImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                StartCoroutine(LoadImage(path));
            }
        });
    }
    private IEnumerator LoadImage(string path)
    {
        GlobalAnimator.Instance.FadeInLoader();
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            userSessionManager.Instance.profileSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            profileImage.sprite= userSessionManager.Instance.profileSprite;
            UploadImageToFirebase(texture);
        }
        else
        {
            Debug.LogError("Failed to load image from gallery");
        }
    }
    public void UploadImageToFirebase(Texture2D texture)
    {
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

}
