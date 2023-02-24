using System;
using UnityEditor;
using UnityEngine;

namespace Hyperfiction.Editor.Core {
    public static partial class EUI
    {
        public static class Foldout {
            static readonly Color _headerbackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.2f);

            public static void Draw(SerializedProperty property, string label, Action<GenericMenu> fillGenericMenu) {
                bool refIsExpanded = property.isExpanded;
                Draw(ref refIsExpanded, new GUIContent(label), fillGenericMenu);
                property.isExpanded = refIsExpanded;
            } 

            public static void Draw(ref bool expanded, string label) => Draw(ref expanded, new GUIContent(label), null);
            public static void Draw(ref bool expanded, GUIContent label, Action<GenericMenu> fillGenericMenu) {
                var e = Event.current;
                var rowHeight = EditorGUIUtility.singleLineHeight + 2;
                var offset = 4;
                
                EUI.Layout.Separator();
                var backgroundRect = GUILayoutUtility.GetRect(1f, rowHeight);
                EUI.Layout.Separator();

                var foldoutRect = backgroundRect;
                foldoutRect.y +=  expanded ? 1f : 2f;
                foldoutRect.xMin += offset;
                foldoutRect.width = rowHeight - offset;
                foldoutRect.height = rowHeight - offset;

                var labelRect = backgroundRect;
                labelRect.xMin += 18f + offset;
                labelRect.xMax -= 20f;

                // Background rect should be full-width
                backgroundRect.xMin = 0f;
                backgroundRect.width += 4f;

                // background
                EditorGUI.DrawRect(backgroundRect, _headerbackgroundColor);
                EditorGUILayout.Space(-rowHeight-2);

                using(EUI.Layout.Horizontal()) {
                    GUILayout.Toggle(expanded, GUIContent.none, EditorStyles.foldout, GUILayout.Height(rowHeight));
                    GUILayout.Label(label, EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();

                    if (fillGenericMenu != null) {
                        GUILayout.FlexibleSpace();
                        if (EUI.Button.DrawMini("\u22EE") ) {
                            var menu = new GenericMenu();
                            fillGenericMenu(menu);
                            menu.DropDown(new Rect(e.mousePosition, Vector2.zero));
                            e.Use();
                        }
                    }
                }
                
                if (e.type == EventType.MouseDown && backgroundRect.Contains(e.mousePosition) && e.button == 0) {
                    expanded = !expanded;
                    e.Use();
                }
            }
        }
    }
}