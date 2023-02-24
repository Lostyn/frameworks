using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;

public class SerializedBehaviourTree {
    readonly public SerializedObject serializedObject;
    readonly public BehaviourTree tree;

    const string sPropNodes = "nodes";
    const string sPropBlackboard = "blackboard";
    const string sPropGuid = "guid";
    const string sPropNext = "nexts";
    const string sPropPrevious = "previous";
    const string sPropPosition = "position";


    public SerializedProperty Nodes => serializedObject.FindProperty(sPropNodes);
    public SerializedProperty Blackboard => serializedObject.FindProperty(sPropBlackboard);

    public SerializedBehaviourTree(BehaviourTree tree)
    {
        serializedObject = new SerializedObject(tree);
        this.tree = tree;
    }

    public void Save() {
        serializedObject.ApplyModifiedProperties();
    }

    public SerializedProperty FindNode(SerializedProperty array, Node node) {
        for(int i = 0; i < array.arraySize; ++i) {
            var current = array.GetArrayElementAtIndex(i);
            if (current.FindPropertyRelative(sPropGuid).stringValue == node.guid) {
                return current;
            }
        }
        return null;
    }

    public void DeleteNode(SerializedProperty array, Node node) {
       for (int i = 0; i < array.arraySize; ++i) {
            var current = array.GetArrayElementAtIndex(i);
            if (current.FindPropertyRelative(sPropGuid).stringValue == node.guid) {
                array.DeleteArrayElementAtIndex(i);
                return;
            }
        }
    }

    public void DeletePreviousNode(SerializedProperty array, Node node) {
       for (int i = 0; i < array.arraySize; ++i) {
            var current = array.GetArrayElementAtIndex(i);
            if (current.stringValue == node.guid) {
                array.DeleteArrayElementAtIndex(i);
                return;
            }
        }
    }



    public Node CreateNodeInstance(System.Type type) {
        Node node = System.Activator.CreateInstance(type) as Node;
        node.guid = GUID.Generate().ToString();
        return node;
    }

    SerializedProperty AppendArrayElement(SerializedProperty arrayProperty) {
        arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
        return arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
    }

    public Node CreateNode(System.Type type, Vector2 position) {
        Node node = CreateNodeInstance(type);
        node.position = position;
        
        InjectNode(node);
        Save();
        return node;
    }

    public void InjectNode(Node node) {
        SerializedProperty newNode = AppendArrayElement(Nodes);
        newNode.managedReferenceValue = node;

        Save();
    }

    public void SetNodePosition(Node node, Vector2 position) {
        var nodeProp = FindNode(Nodes, node);
        nodeProp.Find(sPropPosition).vector2Value = position;
        Save();
    }

    public void DeleteNode(Node node) {
        SerializedProperty nodesProperty = Nodes;

        DeleteNode(Nodes, node);
        Save();
    }

    public void AddChild(Node parent, Node child) {
        // Nexts
        var parentProperty = FindNode(Nodes, parent);
        var nextProperty = parentProperty.FindPropertyRelative(sPropNext);
        if (nextProperty != null) {
            SerializedProperty newNext = AppendArrayElement(nextProperty);
            newNext.managedReferenceValue = child;
        }

        // Previous
        var childProperty = FindNode(Nodes, child);
        var previousProperty = childProperty.FindPropertyRelative(sPropPrevious);
        if (previousProperty != null) {
            SerializedProperty newPrevious = AppendArrayElement(previousProperty);
            newPrevious.stringValue = parent.guid;
        }

        Save();
    }

    public void RemoveChild(Node parent, Node child) {
        // Nexts
        var parentProperty = FindNode(Nodes, parent);
        var nextProperty = parentProperty.FindPropertyRelative(sPropNext);
        if (nextProperty != null) {
            DeleteNode(nextProperty, child);
        }

        // Previous
        var childProperty = FindNode(Nodes, child);
        var previousProperty = childProperty.FindPropertyRelative(sPropPrevious);
        if (previousProperty != null) {
            DeletePreviousNode(previousProperty, parent);
        }

        Save();
    }

    public void MoveIntoSubtree(BehaviourTree subtree, List<Node> nodes) {
        var subSerializer = new SerializedBehaviourTree(subtree);
        
        Node node = null;
        SerializedProperty nodeProperty = null;
        
        Node newNode = null;
        SerializedProperty newNodeProperty = null;

        for(int i = 0; i < nodes.Count; i++) {
            node = nodes[i];
            nodeProperty = FindNode(Nodes, node);
            
            newNode = subSerializer.CreateNode(node.GetType(), node.position);
            newNodeProperty = subSerializer.FindNode(subSerializer.Nodes, newNode);
            
            CloneInto(nodeProperty, newNodeProperty, node.GetType());
            subSerializer.Save();
        }

        for(int i = 0; i < nodes.Count; i++) {
            nodeProperty = FindNode(Nodes, nodes[i]);
            newNodeProperty = subSerializer.FindNode(subSerializer.Nodes, nodes[i]);
            
            var nodeNexts = nodeProperty.Find("nexts");
            var newNexts = newNodeProperty.Find("nexts");
            for(int j = 0; j < nodeNexts.arraySize; j++) {
                var p = AppendArrayElement(newNexts);
                var next = subSerializer.tree.GetNodeByGuid(nodes[i].nexts[j].guid);
                if (next != null)
                    p.managedReferenceValue = next;
            }
            subSerializer.Save();
        }
    }

    public void CloneInto(SerializedProperty node, SerializedProperty clone, Type nodeType) {
        clone.Find("guid").stringValue = node.Find("guid").stringValue;

        if (nodeType == typeof(SubtreeNode)) {
            clone.Find("subtree").objectReferenceValue = node.Find("subtree").objectReferenceValue;
        }
    }
}