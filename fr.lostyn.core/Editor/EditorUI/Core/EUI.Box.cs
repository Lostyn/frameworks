using UnityEditor;
using UnityEngine;

namespace Hyperfiction.Editor.Core {
    public static partial class EUI
    {
        public static class Box {
            public static EditorGUILayout.VerticalScope Vertical => new EditorGUILayout.VerticalScope(EditorStyles.helpBox);
        }
    }
}