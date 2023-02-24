using UnityEngine;

public static class AssetUtils {
    public static T GetScriptableObject<T>(string fileName) where T : ScriptableObject {
        if (string.IsNullOrEmpty(fileName)) return null;

        return (T)Resources.Load(fileName, typeof(T));
    }

    public static T GetScriptableObject<T>(string fileName, string resourcesPath) where T : ScriptableObject {
        if (string.IsNullOrEmpty(resourcesPath)) return null;
        if (string.IsNullOrEmpty(fileName)) return null;

        resourcesPath = CleanPath(resourcesPath);
        return (T)Resources.Load($"{resourcesPath}/{fileName}.asset", typeof(T));
    }

    public static string CleanPath(string path)
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (!path[path.Length - 1].Equals(@"\")) path += @"\";
        path = path.Replace(@"\\", @"\");
        path = path.Replace(@"\", "/");
        return path;
    }
}