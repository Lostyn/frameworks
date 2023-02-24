using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Hyperfiction.Editor.Core {
    public abstract class CustomInspector : UnityEditor.Editor
    {
        private const string PROP_SCRIPT = "m_Script";
        private const char PROPERTY_SEPARATOR = '/';

        protected readonly Dictionary<string, SerializedProperty> SerializedProperties = new Dictionary<string, SerializedProperty>();
        SerializedProperty script;

        protected EditorGUILayout.VerticalScope VerticalHelp => new EditorGUILayout.VerticalScope(EditorStyles.helpBox);
        protected EditorGUILayout.VerticalScope Vertical => new EditorGUILayout.VerticalScope();
        protected EditorGUILayout.VerticalScope VerticalLayout(params GUILayoutOption[] options) => new EditorGUILayout.VerticalScope(options);
        protected EditorGUILayout.HorizontalScope Horizontal => new EditorGUILayout.HorizontalScope();
        protected bool Button(string text, int width = 25) => GUILayout.Button(text,  GUILayout.Width(width), GUILayout.Height(25));

        protected List<SerializedProperty> events;
        bool _showInteractableEvents;

        protected virtual void OnEnable() {
            script = serializedObject.FindProperty(CustomInspector.PROP_SCRIPT);

            EditorApplication.update += Repaint;
        }

        protected virtual void OnDisable() => EditorApplication.update -= Repaint;

        public override void OnInspectorGUI() {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(script, true, new GUILayoutOption[0]);
            GUI.enabled = true;

            serializedObject.Update();
            OnInspector();
            OnLateInspector();
            
            if(events != null && events.Count > 0) {
                using( VerticalHelp ) {
                    _showInteractableEvents = EditorGUILayout.ToggleLeft("Show Events", _showInteractableEvents);
                    if (_showInteractableEvents) {
                        for(int i = 0; i < events.Count; i++) {
                            EditorGUILayout.PropertyField(events[i]);
                        }
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        public void DrawDefaultGUI() { base.OnInspectorGUI(); }
        public abstract void OnInspector();
        protected virtual void OnLateInspector() { }

        public SerializedProperty GetProperty(string propertyPath) {
            if (SerializedProperties.ContainsKey(propertyPath)) return SerializedProperties[propertyPath];

            //
            if (propertyPath.Contains(PROPERTY_SEPARATOR.ToString())) {
                List<string> split = propertyPath.Split(PROPERTY_SEPARATOR).ToList();
                string propertyName = split[0];
                split.RemoveAt(0);

                return GetRelativeProperty(serializedObject.FindProperty(propertyName), split);
            }


            SerializedProperty property = serializedObject.FindProperty(propertyPath);
            SerializedProperties.Add(propertyPath, property);
            return property;
        }

        public SerializedProperty GetRelativeProperty(SerializedProperty property, List<string> subPropertyNames) {
            string key = property.propertyPath + PROPERTY_SEPARATOR + string.Join(PROPERTY_SEPARATOR.ToString(), subPropertyNames);
            if (SerializedProperties.ContainsKey(key)) return SerializedProperties[key];
            
            SerializedProperty s = property;
            for(int i = 0; i < subPropertyNames.Count; i++) {
                s = s.FindPropertyRelative(subPropertyNames[i]);
            }
            SerializedProperties.Add(key, s);
            return s;
        }

        public SerializedProperty GetRelativeProperty(SerializedProperty parentProperty, string propertyPath) {
            List<string> split = propertyPath.Split(PROPERTY_SEPARATOR).ToList();
            return GetRelativeProperty(parentProperty, split);
        }
    }
}