using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GalleryImagePicker : MonoBehaviour
{
    public RawImage imageView;

    public void PickImageFromGallery()
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
                    imageView.texture = texture;
                    Debug.Log("Image successfully loaded from gallery.");
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
}
