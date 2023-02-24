using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AndNode : LogicalNode
{
    Color _nodeColor = new Color(1, 0.06f, 0.33f);
    public override Color NodeColor => _nodeColor;
    
    /// <summary>
    /// Store the guid to prevent cyclique references.
    /// </summary>
    /// <typeparam name="string"></typeparam>
    /// <returns></returns>
    [SerializeReference]
    public List<string> previous = new List<string>();

    public override List<Node> GetNexts() { return nexts; }

    List<Node> _previousNode;

    protected override void OnStart()
    {
        _previousNode = tree.nodes.Where( n => previous.Contains(n.guid)).ToList();
    }

    protected override void OnStop() { }
    protected override void OnCancel() { }

    protected override State OnUpdate()
    {
        for(int i = 0; i < _previousNode.Count; i++) {
            if (_previousNode[i].state != State.Completed)
                return State.Running;
        }

        return State.Completed;
    }
}
