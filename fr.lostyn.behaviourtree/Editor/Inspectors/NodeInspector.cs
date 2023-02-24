using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Node), true)]
public class NodeInspector : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        
        var container = new VisualElement();

        var guidField = new PropertyField( property.Find("guid"));
        guidField.SetEnabled(false);
        var stateField = new PropertyField( property.Find("state"));
        stateField.SetEnabled(false);
        
        container.Add(guidField);
        container.Add(stateField);

        var lastElement = container.ElementAt( container.childCount - 1);
        lastElement.style.marginBottom = new StyleLength(20);

        return container;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var labelString = property.managedReferenceValue.GetType().ToString();
        EditorGUI.LabelField(position, labelString);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (EditorGUIUtility.singleLineHeight);
    }
}