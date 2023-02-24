using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public SerializedBehaviourTree serializer;

    public Node node;
    public Port input;
    public Port output;

    private GraphView m_GraphView;
    private GraphView graphView
    {
        get
        {
            if (m_GraphView == null)
            {
                m_GraphView = GetFirstAncestorOfType<GraphView>();
            }
            return m_GraphView;
        }
    }

    public NodeView(SerializedBehaviourTree tree, Node node) : base("Assets/fr.lostyn.behaviourtree/Editor/UIBuilder/NodeView.uxml") {
        this.serializer = tree;
        this.node = node;
        this.title = node.GetType().Name.Replace("Node", "");
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();

        VisualElement title = this.Q<VisualElement>("title");
        title.style.borderLeftColor = node.NodeColor;

        IMGUIContainer dataContainer = this.Q<IMGUIContainer>("data-container");
        dataContainer.onGUIHandler = node.OnDataGUI;
    }

    private void CreateInputPorts()
    {
        if (node is ActionNode) {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        } else if (node is OrNode) {
            input = new NodePort(Direction.Input, Port.Capacity.Single);
        } else if (node is AndNode) {
            input = new NodePort(Direction.Input, Port.Capacity.Multi);
        } else if (node is IfNode) {
            input = new NodePort(Direction.Input, Port.Capacity.Single);   
        }

        if (input != null) {
            input.portName = "";
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        output = new NodePort(Direction.Output, Port.Capacity.Multi);   
        
        if (output != null) {
            output.portName = "";
            outputContainer.Add(output);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);

        Vector2 position = new Vector2(newPos.xMin, newPos.yMin);
        serializer.SetNodePosition(node, position);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null) {
            OnNodeSelected.Invoke(this);
        }
    }

    public void UpdateState() {
        RemoveFromClassList("idle");
        RemoveFromClassList("running");
        RemoveFromClassList("completed");
        RemoveFromClassList("canceled");
        RemoveFromClassList("waiting");

        if (Application.isPlaying) {
            switch(node.state) {
                case Node.State.Idle:
                    AddToClassList("idle");
                    break;
                case Node.State.Waiting:
                    AddToClassList("waiting");
                    break;
                case Node.State.Running:
                    AddToClassList("running");
                    break;
                case Node.State.Completed:
                    AddToClassList("completed");
                    break;
                case Node.State.Canceled:
                    AddToClassList("canceled");
                    break;
            }
        }
    }

    public void DisconnectAll() {
        HashSet<GraphElement> toDelete = new HashSet<GraphElement>();
        AddConnectionsToDeleteSet(inputContainer, ref toDelete);
        AddConnectionsToDeleteSet(outputContainer, ref toDelete);
        toDelete.Remove(null);

        if (graphView != null) {
            graphView.DeleteElements(toDelete);
        } else {
            Debug.Log("Disconnecting nodes that are not in a GraphView will not work.");
        }
    }

    void AddConnectionsToDeleteSet(VisualElement container, ref HashSet<GraphElement> toDelete)
    {
        List<GraphElement> toDeleteList = new List<GraphElement>();
        container.Query<Port>().ForEach(elem =>
        {
            if (elem.connected)
            {
                foreach (Edge c in elem.connections)
                {
                    if ((c.capabilities & Capabilities.Deletable) == 0)
                        continue;

                    toDeleteList.Add(c);
                }
            }
        });

        toDelete.UnionWith(toDeleteList);
    }
}