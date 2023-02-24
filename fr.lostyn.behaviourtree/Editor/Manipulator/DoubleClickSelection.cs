using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DoubleClickSelection : MouseManipulator
{
    double time;
    double doubleClickDuration = 0.3;

    public DoubleClickSelection() {
        time = EditorApplication.timeSinceStartup;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        var graphView = target as BehaviourTreeView;
        if (graphView == null)
            return;
        
        double duration = EditorApplication.timeSinceStartup - time;
        time = EditorApplication.timeSinceStartup;
        if (duration < doubleClickDuration) {
            if (CanStopManipulation(evt)) {
                NodeView clickedElement = evt.target as NodeView;
                if (clickedElement == null) {
                    var ve = evt.target as VisualElement;
                    clickedElement = ve.GetFirstAncestorOfType<NodeView>();
                    if (clickedElement == null)
                        return;
                }
                
                graphView.DoubleClickNodeView(clickedElement);
            }
        }

        
    }
}