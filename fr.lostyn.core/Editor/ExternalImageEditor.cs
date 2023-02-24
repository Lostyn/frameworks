using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using Hyperfiction.Core;

namespace Hyperfiction.Editor.Core {
    [CustomEditor(typeof(ExternalUIImage), true)]
    public class ExternalImageEditor : GraphicEditor {
        SerializedProperty m_path;
        SerializedProperty m_defaultSprite;
        SerializedProperty m_UVRect;
        SerializedProperty m_OnLoaded;

        GUIContent m_UVRectContent;

        protected override void OnEnable() {
            base.OnEnable();

            m_UVRectContent     = EditorGUIUtility.TrTextContent("UV Rect");

            m_path = serializedObject.FindProperty("m_path");
            m_defaultSprite = serializedObject.FindProperty("m_defaultSprite");
            m_UVRect = serializedObject.FindProperty("m_UVRect");
            m_OnLoaded = serializedObject.FindProperty("m_OnLoaded");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_path);
            EditorGUILayout.PropertyField(m_defaultSprite);

            AppearanceControlsGUI();
            RaycastControlsGUI();
            EditorGUILayout.PropertyField(m_UVRect, m_UVRectContent);
            EditorGUILayout.PropertyField(m_OnLoaded);

            serializedObject.ApplyModifiedProperties();
        }

        public override bool HasPreviewGUI() => false;
    }
}