using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;


public class InspectorView : VisualElement
{
    public new class UxmlFactory: UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }
    
    public InspectorView() { }

    internal void UpdateSelection(SerializedBehaviourTree serializer, NodeView nodeView)
    {
        Clear();
        if (nodeView == null) {
            return;
        }

        var nodeProperty = serializer.FindNode(serializer.Nodes, nodeView.node);
        if (nodeProperty == null) {
            return;
        } 

        // nodeProperty.isExpanded = true;

        PropertyField field = new PropertyField();
        field.BindProperty(nodeProperty);
        Add(field);
    }
}