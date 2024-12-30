using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImagePicker : MonoBehaviour
{
    public string FinalPath;
    public Image IMAGE;
    public void LoadFile()
    {
        string FileType = NativeFilePicker.ConvertExtensionToFileType("*");
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation cancelled");
            else
            {
                FinalPath = path;
                Debug.Log("Picked file: " + FinalPath);
            }
        }, new string[] { FileType });
    }
    public void SaveFile()
    {
        // Create a dummy text file
        string filePath = Path.Combine(Application.temporaryCachePath, "test.txt");
        File.WriteAllText(filePath, "Hello world!");
        // Export the file
        NativeFilePicker.Permission permission = NativeFilePicker.ExportFile(filePath, (success) =>
        Debug.Log("File exported: " +
        success));
    }
    IEnumerator LoadTexture()
    {
        WWW www = new WWW(FinalPath);
        while(!www.isDone)
            yield return null;
        //IMAGE.aprite = www.texture;
        print("loded");
    }
}
