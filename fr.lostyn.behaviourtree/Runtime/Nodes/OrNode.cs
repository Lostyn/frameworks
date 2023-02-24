using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrNode : LogicalNode
{
    Color _nodeColor = new Color(1, 0.06f, 0.33f);
    public override Color NodeColor => _nodeColor;
    
    public override List<Node> GetNexts() { return _firstCompleted.nexts; }
    Node _firstCompleted;

    protected override void OnStart() { }
    protected override void OnStop()
    {
        Debug.Log("OnStop Or");
        for(int i = 0; i < nexts.Count; i++) {
            if (nexts[i] != _firstCompleted)
                nexts[i].Cancel();
        }
    }

    protected override void OnCancel() {
        for(int i = 0; i < nexts.Count; i++) {
            nexts[i].Cancel();
        }
    }

    protected override State OnUpdate()
    {
        Node node;
        State state;

        for(int i = 0; i < nexts.Count; i++) {
            node = nexts[i];
            state = node.Update();

            if (state == State.Completed) {
                _firstCompleted = node;
                return State.Completed;
            }
        }

        return State.Running;
    }
}
