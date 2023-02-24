using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Hyperfiction.Core.i18n;

namespace Hyperfiction.Editor.Core.i18n 
{
    [CustomPropertyDrawer(typeof(i18nSprite))]
    public class i18nSpriteDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var indent = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty key = property.FindPropertyRelative("key");

            // Draw label
            Rect rect = EditorGUI.IndentedRect(position);
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.width = 55;

            Rect rectField = EditorGUI.IndentedRect(position);
            rectField.height = EditorGUIUtility.singleLineHeight;
            rectField.x += rect.width;
            rectField.width -= rect.width + 15 + EditorGUIUtility.singleLineHeight;

        // reset indent before props
            EditorGUI.indentLevel = 0;

            EditorGUI.LabelField(rect, property.displayName);
            EditorGUI.PropertyField(rectField, key, GUIContent.none);

            EditorGUI.EndProperty();
            EditorGUI.indentLevel = indent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}