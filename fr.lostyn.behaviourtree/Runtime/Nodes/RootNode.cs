using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RootNode : Node
{
    public override List<Node> GetNexts() { return nexts; }

    Color _nodeColor = new Color(0, 0, 0, 0);
    public override Color NodeColor => _nodeColor;

    protected override void OnStart() { }
    protected override void OnStop() { }
    protected override void OnCancel() { }

    protected override State OnUpdate()
    {
        return State.Completed;   
    }
}