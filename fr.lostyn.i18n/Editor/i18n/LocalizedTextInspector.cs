using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using fr.lostyn.i18n;

namespace fr.lostyneditor.i18n
{  
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LocalizedText), true)]
    public class LocalizedTextInspector : UnityEditor.Editor {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            GUILayout.Space(6f);
            SerializedProperty prop = serializedObject.FindProperty("key");
            SerializedProperty key = prop.FindPropertyRelative( "key" );
            EditorGUILayout.BeginHorizontal();
            {
                string cachedValue = key.stringValue;
                EditorGUILayout.PropertyField( prop );
                string value = key.stringValue;

                if (value != cachedValue ) {
                    SetText( i18nEditorUtils.EditorGet( value ) );
                }
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Update text component")) {
                SetText(i18nEditorUtils.EditorGet(key.stringValue));
            }

            serializedObject.ApplyModifiedProperties();
        }

        void SetText(string text)
        {
            Text t = (target as LocalizedText).GetComponent<Text>();
            if (t != null) t.text = text;

            TextMeshProUGUI tm = (target as LocalizedText).GetComponent<TextMeshProUGUI>();
            if (tm != null) tm.text = text;
        }
    }
}