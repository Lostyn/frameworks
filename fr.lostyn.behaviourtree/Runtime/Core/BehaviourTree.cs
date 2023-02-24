using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    [SerializeReference]
    public Node rootNode;

    [SerializeReference]
    public List<Node> nodes = new List<Node>();

    public Blackboard blackboard = new Blackboard();

    [SerializeField] List<Node> _runningNodes = new List<Node>();
    public List<Node> runningNodes => _runningNodes;

    public BehaviourTree() {
        rootNode = new RootNode();
        nodes.Add(rootNode);
    }

    public void Start() {
        _runningNodes.AddRange( rootNode.nexts );
        nodes.ForEach( node => node.tree = this );
    }

    public void Cancel() {
        _runningNodes.ForEach( n => n.Cancel() );
    }

    public Node.State Update() {
        Node node;
        Node.State state = Node.State.Running;
        if (_runningNodes.Count > 0) {
            for(int i = 0; i < _runningNodes.Count; i++) {
                node = _runningNodes[i];

                state = node.Update();

                if (state == Node.State.Completed) {
                    _runningNodes.Remove(node);
                    _runningNodes.AddRange(node.GetNexts().Where( n => !_runningNodes.Contains(n)));
                } else if (state == Node.State.Canceled) {
                    _runningNodes.Remove(node);
                }
            }
        } else {
            return Node.State.Completed;
        }

        return Node.State.Running;
    }

    public Node GetNodeByGuid(string guid) {
        return nodes.FirstOrDefault( n => n.guid == guid );
    }

    public BehaviourTree Clone() {
        BehaviourTree tree = Instantiate(this);
        return tree;
    }

    public void Bind(Blackboard blackboard) {
        this.blackboard = blackboard;
        rootNode.blackboard = blackboard;
        nodes.ForEach( n => n.blackboard = blackboard);
    }

    public List<Node> GetChildren(Node parent) {
        return parent.nexts;
    }

    public IEnumerable<T> GetNodes<T>() where T: Node 
    {
        return nodes
                .Where( node => node is T)
                .Select( node => node as T);
    }
}
