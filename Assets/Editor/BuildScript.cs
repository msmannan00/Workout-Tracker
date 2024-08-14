using UnityEditor;
using System.IO;

public class BuildScript
{
    public static void BuildAndroid()
    {
        string[] scenes = { "Assets/GameAssets/Scenes/root.unity" };
        string buildPath = "Builds/Android";
        string apkName = "health.apk";

        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        BuildPipeline.BuildPlayer(scenes, Path.Combine(buildPath, apkName), BuildTarget.Android, BuildOptions.None);
    }
}
