using UnityEngine;
using UnityEditor;
using System;
using Hyperfiction.Core;

namespace  Hyperfiction.Editor.Core {
    [CustomEditor(typeof(BaseSelectable), true)]
    public class MGSelectableInspector : CustomInspector {

        SerializedProperty _primaryTarget;
        SerializedProperty _secondTargets;
        SerializedProperty _interactable;
        SerializedProperty _fadeDuration;

        BaseSelectable Target => (BaseSelectable) target;

        protected override void OnEnable() {
            base.OnEnable();

            _primaryTarget = GetProperty("m_primaryTarget");
            _secondTargets = GetProperty("m_secondTargets");

            _fadeDuration = GetProperty("m_fadeDuration");
            _interactable = GetProperty("m_interactable");
        }

        public override void OnInspector() {
            EditorGUILayout.PropertyField(_interactable, new GUIContent("Interactable"));
            DrawTarget(_primaryTarget);
            EditorGUILayout.PropertyField(_fadeDuration, new GUIContent("Fade Duration"));
            
            EditorGUILayout.Space(5);
            EUI.Title.Draw("Other targets");
            for(int i = 0; i < _secondTargets.arraySize; i++) {
                SerializedProperty property =  _secondTargets.GetArrayElementAtIndex(i);

                int index = i;
                string label = property.displayName;
                var ptarget = GetRelativeProperty(property, "target");
                if (ptarget.objectReferenceValue != null) label = ptarget.objectReferenceValue.name;

                EUI.Foldout.Draw(
                    property,
                    label,
                    menu => {
                        menu.AddItem(new GUIContent("Remove"), false, () => Target.RemoveSecondaryTarget(index) );
                    }
                );

                if (property.isExpanded) {
                    EditorGUILayout.Space();
                    DrawTarget(property);
                }
            }

            GUILayout.Space(2);
            
            using(EUI.Layout.Horizontal()) {
                GUILayout.FlexibleSpace();
                if (EUI.Button.Draw( "Add new target" )) {
                    Target.AddSecondaryTarget();
                }
            }
        }

        void DrawTarget(SerializedProperty property) {
            var target = GetRelativeProperty(property, "target");
            var normalColor = GetRelativeProperty(property, "colors/Normal");
            var highlightedColor = GetRelativeProperty(property, "colors/Light");
            var pressedColor = GetRelativeProperty(property, "colors/Dark");
            var disableColor = GetRelativeProperty(property, "disableColor");

            EditorGUILayout.PropertyField(target, new GUIContent("Target"));
            EditorGUILayout.PropertyField(normalColor, new GUIContent("Normal"));
            EditorGUILayout.PropertyField(highlightedColor, new GUIContent("Highlighted"));
            EditorGUILayout.PropertyField(pressedColor, new GUIContent("Pressed"));
            EditorGUILayout.PropertyField(disableColor, new GUIContent("Disable"));
            EditorGUILayout.Space();
        }
    }
}