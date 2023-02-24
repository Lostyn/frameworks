using UnityEditor;
using UnityEngine;

namespace Hyperfiction.Editor.Core {
    public static partial class EUI
    {
        public static class Title {
            public static void Draw(string text) => EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }
    }
}