using UnityEditor;
using UnityEngine;

namespace Hyperfiction.Editor.Core {
    public static partial class EUI
    {
        public static class Layout {
            static readonly Color _splitterdark = new Color(0.12f, 0.12f, 0.12f, 1.333f);
            static readonly Color _splitterlight = new Color(0.6f, 0.6f, 0.6f, 1.333f);
            public static Color Splitter { get { return EditorGUIUtility.isProSkin ? _splitterdark : _splitterlight; } }

            public static EditorGUILayout.HorizontalScope Horizontal() => new EditorGUILayout.HorizontalScope();
            public static EditorGUILayout.HorizontalScope Horizontal(params GUILayoutOption[] options) => new EditorGUILayout.HorizontalScope(options);
            public static EditorGUILayout.HorizontalScope Horizontal(GUIStyle style) => new EditorGUILayout.HorizontalScope(style);
            
            public static EditorGUILayout.VerticalScope Vertical(params GUILayoutOption[] options) => new EditorGUILayout.VerticalScope(options);

            public static EditorGUILayout.ScrollViewScope Scroll(Vector2 scroll, params GUILayoutOption[] options) => new EditorGUILayout.ScrollViewScope(scroll, options);
            public static EditorGUILayout.ScrollViewScope VerticalScroll(Vector2 scroll, params GUILayoutOption[] options) => 
                new EditorGUILayout.ScrollViewScope(
                    scroll, 
                    false, 
                    true, 
                    GUIStyle.none, 
                    GUI.skin.verticalScrollbar, 
                    GUIStyle.none, 
                    options
                );
            public static EditorGUILayout.ScrollViewScope HiddenScroll(Vector2 scroll, params GUILayoutOption[] options) => 
                new EditorGUILayout.ScrollViewScope(
                    scroll, 
                    GUIStyle.none, 
                    GUIStyle.none, 
                    options
                );

            public static TemporaryLabelWidth TempLabelWidth( float width ) => new TemporaryLabelWidth( width );

            public static void Separator(float height = 1f) {
                var rect = GUILayoutUtility.GetRect(1f, height);
                rect.xMin = 0f;
                rect.width += 4f;

                if (Event.current.type != EventType.Repaint)
                    return;

                EditorGUI.DrawRect(rect, Splitter);
            }

        }
    }
}