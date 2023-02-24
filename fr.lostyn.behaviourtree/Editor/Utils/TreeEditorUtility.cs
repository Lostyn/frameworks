using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class TreeEditorUtility {
    public static List<string> GetAssetPaths<T>() where T : UnityEngine.Object {
        string[] assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        return assetIds.Select( id => AssetDatabase.GUIDToAssetPath(id)).ToList();
    }
}