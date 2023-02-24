using UnityEditor;
using UnityEngine;
using Hyperfiction.Core.i18n;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Hyperfiction.Editor.Core.i18n
{  
    [CustomPropertyDrawer(typeof(i18nString))]
    public class i18nStringDrawer : PropertyDrawer
    {

#region UIToolkit
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var lineContainer = new VisualElement();
            lineContainer.style.flexDirection = FlexDirection.Row;

            var keyProp = property.FindPropertyRelative("key");
            var keyField = new PropertyField(keyProp);
            keyField.style.flexGrow = 1;

            // keyField.ElementAt(0).ElementAt(0).style.minWidth = 30;
            var foundedField = new Label();
            foundedField.text = "\u2718";
            foundedField.style.color = Color.red;

            var btn = new Button( () => {
                 i18nWindowPicker.OpenWithKey((result) => {
                    keyProp.stringValue = result;
                    property.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty( property.serializedObject.targetObject );
                });
            });
            btn.text = "\u25BE";

            var resultField = new Label();
            resultField.SetEnabled(false);
            resultField.style.backgroundColor = new Color(0.16f, 0.16f, 0.16f);
            resultField.style.borderBottomColor = 
            resultField.style.borderLeftColor = 
            resultField.style.borderTopColor = 
            resultField.style.borderRightColor = Color.black;
            resultField.style.borderBottomLeftRadius = 
            resultField.style.borderBottomRightRadius = 
            resultField.style.borderTopRightRadius = 
            resultField.style.borderTopLeftRadius = 2;
            resultField.style.borderBottomWidth = 
            resultField.style.borderLeftWidth = 
            resultField.style.borderTopWidth = 
            resultField.style.borderRightWidth = 1;
            resultField.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            resultField.style.height = 60;
            resultField.style.paddingBottom = 
            resultField.style.paddingLeft = 
            resultField.style.paddingTop = 
            resultField.style.paddingRight = 4;
            resultField.style.whiteSpace = WhiteSpace.Normal;
            resultField.style.unityTextOverflowPosition = TextOverflowPosition.Start;

            lineContainer.Add(keyField);
            lineContainer.Add(foundedField);
            lineContainer.Add(btn);
            container.Add(lineContainer);
            container.Add(resultField);

            Refresh(keyProp, foundedField, resultField);
            container.TrackPropertyValue(keyProp, prop => {
                Refresh(prop, foundedField, resultField);
            });

            return container;
        }

        void Refresh(SerializedProperty keyProp, Label label, Label resultField) {
            string value = i18nEditorUtils.EditorGet(keyProp.stringValue);
            var founded = !string.IsNullOrEmpty(value);
            label.text = founded ? "\u2714" : "\u2718";
            label.style.color = founded ? Color.green : Color.red;

            resultField.text = value;
        }

#endregion


#region IMGUI
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var founded = false;
            var indent = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty key = property.FindPropertyRelative("key");

            // Draw label
            Rect rectField = EditorGUI.IndentedRect(position);
            rectField.height = EditorGUIUtility.singleLineHeight;
            rectField.width -= 15 + EditorGUIUtility.singleLineHeight;

            Rect rectMark = new Rect(rectField);
            rectMark.x += rectField.width;
            rectMark.width = 15;

            Rect rectBtn = new Rect(rectMark);
            rectBtn.x += rectMark.width;
            rectBtn.width = rectBtn.height = EditorGUIUtility.singleLineHeight;

            Rect rectPreview = EditorGUI.IndentedRect(position);
            rectPreview.y += EditorGUIUtility.singleLineHeight + 2;
            rectPreview.height = EditorGUIUtility.singleLineHeight * 3;

            // reset indent before props
            EditorGUI.indentLevel = 0;

            EditorGUI.PropertyField(rectField, key, label);

            if (EditorGUI.DropdownButton(rectBtn, new GUIContent(""), FocusType.Passive))
            {
                i18nWindowPicker.OpenWithKey((result) => {
                    property.FindPropertyRelative("key").stringValue = result;
                    property.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty( property.serializedObject.targetObject );
                });
            }

            founded = !string.IsNullOrEmpty(i18nEditorUtils.EditorGet(key.stringValue));
            GUI.color = founded ? Color.green : Color.red;
            EditorGUI.LabelField(rectMark, new GUIContent(founded ? "\u2714" : "\u2718"));
            GUI.color = Color.white;

            if (founded)
            {
                GUI.enabled = false;
                EditorStyles.textArea.wordWrap = true;
                EditorGUI.TextArea(rectPreview, i18nEditorUtils.EditorGet(key.stringValue), EditorStyles.textArea);
                GUI.enabled = true;
            }

            EditorGUI.EndProperty();
            EditorGUI.indentLevel = indent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var key = property.FindPropertyRelative("key");
            var founded = !string.IsNullOrEmpty(i18nEditorUtils.EditorGet(key.stringValue));
            return founded ? EditorGUIUtility.singleLineHeight * 4 + 2: EditorGUIUtility.singleLineHeight;
        }
#endregion
    }
}