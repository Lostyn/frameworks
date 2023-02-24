using System.Collections;
using System.Collections.Generic;
using Hyperfiction.Core;
using UnityEditor;
using UnityEngine;

namespace  Hyperfiction.Editor.Core {
    
    [CustomEditor(typeof(MGButton), true)]
    public class MGButtonInspector : MGSelectableInspector 
    {
        SerializedProperty _OnClick;
        SerializedProperty _OnHover;
        SerializedProperty _OnOut;

        SerializedProperty m_PlayAudioClipOnSelectEnter;
        SerializedProperty m_AudioClipForOnSelectEnter;
        SerializedProperty m_PlayAudioClipOnSelectExit;
        SerializedProperty m_AudioClipForOnSelectExit;
        SerializedProperty m_PlayAudioClipOnHoverEnter;
        SerializedProperty m_AudioClipForOnHoverEnter;
        SerializedProperty m_PlayAudioClipOnHoverExit;
        SerializedProperty m_AudioClipForOnHoverExit;

        bool m_ShowSoundEvents = false;
        bool m_ShowExtendsEvents = false;

        protected override void OnEnable() {
            base.OnEnable();

            _OnClick = GetProperty("m_OnClick");
            _OnHover = GetProperty("m_OnHover");
            _OnOut = GetProperty("m_OnOut");

            m_PlayAudioClipOnSelectEnter = GetProperty("m_PlayAudioClipOnSelectEnter");
            m_AudioClipForOnSelectEnter = GetProperty("m_AudioClipForOnSelectEnter");
            m_PlayAudioClipOnSelectExit = GetProperty("m_PlayAudioClipOnSelectExit");
            m_AudioClipForOnSelectExit = GetProperty("m_AudioClipForOnSelectExit");
            m_PlayAudioClipOnHoverEnter = GetProperty("m_PlayAudioClipOnHoverEnter");
            m_AudioClipForOnHoverEnter = GetProperty("m_AudioClipForOnHoverEnter");
            m_PlayAudioClipOnHoverExit = GetProperty("m_PlayAudioClipOnHoverExit");
            m_AudioClipForOnHoverExit = GetProperty("m_AudioClipForOnHoverExit");
        }

        public override void OnInspector() {
            base.OnInspector();
            
            EditorGUILayout.Space();
            ExtendsInspector();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_OnClick);

            m_ShowSoundEvents = EditorGUILayout.Foldout(m_ShowSoundEvents, "Sound Events");
            if (m_ShowSoundEvents)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_PlayAudioClipOnSelectEnter, new GUIContent("Select Enter"));
                if (m_PlayAudioClipOnSelectEnter.boolValue)
                    EditorGUILayout.PropertyField(m_AudioClipForOnSelectEnter, GUIContent.none);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_PlayAudioClipOnSelectExit, new GUIContent("Select Exit"));
                if (m_PlayAudioClipOnSelectExit.boolValue)
                    EditorGUILayout.PropertyField(m_AudioClipForOnSelectExit, GUIContent.none);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_PlayAudioClipOnHoverEnter, new GUIContent("Hover"));
                if (m_PlayAudioClipOnHoverEnter.boolValue)
                    EditorGUILayout.PropertyField(m_AudioClipForOnHoverEnter, GUIContent.none);
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_PlayAudioClipOnHoverExit, new GUIContent("Out"));
                if (m_PlayAudioClipOnHoverExit.boolValue)
                    EditorGUILayout.PropertyField(m_AudioClipForOnHoverExit, GUIContent.none);
                GUILayout.EndHorizontal();
            }

            m_ShowExtendsEvents = EditorGUILayout.Foldout(m_ShowExtendsEvents, "Extends Events");
            if (m_ShowExtendsEvents) {
                EditorGUILayout.PropertyField(_OnHover);
                EditorGUILayout.PropertyField(_OnOut);
            }
        }

        protected virtual void ExtendsInspector() {

        }
    }
}