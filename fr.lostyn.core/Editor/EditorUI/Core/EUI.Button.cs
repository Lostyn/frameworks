using UnityEditor;
using UnityEngine;

namespace Hyperfiction.Editor.Core {
    public static partial class EUI
    {
        public static class Button {
            public static bool Draw(string text) => GUILayout.Button(text, GUILayout.Height(25));
            public static bool Draw(string text, int width = 25, int height = 25) => GUILayout.Button(text,  GUILayout.Width(width), GUILayout.Height(height));

            
            public static bool DrawBox(string text, string tootip) => GUILayout.Button(new GUIContent(text, tootip), GUILayout.Width(EditorGUIUtility.singleLineHeight ), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            
            public static bool DrawMini(string text) => GUILayout.Button(text, EditorStyles.miniButton, GUILayout.Width(EditorGUIUtility.singleLineHeight - 2 ), GUILayout.Height(EditorGUIUtility.singleLineHeight - 2));
        }
    }
}