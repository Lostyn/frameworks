using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class CopyPasteSerializer {

    [Serializable]
    public struct NodeDescription {
        public string nodeType;
        public string nodeData;

        public NodeDescription(Node node) {
            nodeType = node.GetType().FullName;
            nodeData = EditorJsonUtility.ToJson(node);
        }
    }

    [Serializable]
    public class Wrapper {
        public NodeDescription[] Nodes;

        public Wrapper(string json) {
            EditorJsonUtility.FromJsonOverwrite(json, this);
        }

        public Wrapper(Node[] nodes) {
            Nodes = nodes.Select( n => new NodeDescription(n)).ToArray();
        }
    }

    static Node CreateNode(Type type) {
        Node node = Activator.CreateInstance(type) as Node;
        node.guid = GUID.Generate().ToString();
        return node;
    }

    public static Node Clone(Node node) {
        Type nodeType = node.GetType();
        Node cloneNode = CreateNode(nodeType);
        
        cloneNode.InjectForDuplicate(node);
        cloneNode.position = node.position;
        
        return cloneNode;
    }

    public static string Serialize(IEnumerable<GraphElement> elements)
    {
        var nodes = elements
                    .OnlyNodes()
                    .Where( e => !(e is RootNode))
                    .Select( e => Clone(e))
                    .ToArray();

        var wrapper = new Wrapper(nodes);
        return EditorJsonUtility.ToJson(wrapper);
    }

    public static List<Node> Deserialize(string data)
    {
        Assembly asm = typeof(Node).Assembly;
        var wrapper = new Wrapper(data);
        return wrapper.Nodes.Select( desc => {
            var nodeType = asm.GetType(desc.nodeType);
            var newNode = CreateNode(nodeType);
            EditorJsonUtility.FromJsonOverwrite(desc.nodeData, newNode);
            return newNode;
        }).ToList();
    }
}