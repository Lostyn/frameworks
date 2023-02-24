using UnityEditor;

public static class PropertyExtensions {
    public static SerializedProperty Find(this SerializedProperty prop, string name) {
        string[] names = name.Split("/");
        
        if (names.Length == 0) {
            return null;
        }

        SerializedProperty result = prop;
        for(int i = 0; i < names.Length; i++) {
            result = result.FindPropertyRelative(names[i]);
        }

        return result;
    }
}