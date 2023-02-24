using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SubtreeNode : ActionNode
{
    Color _nodeColor = new Color(0.2f, 0.2f, 0.2f);
    public override Color NodeColor => _nodeColor;

    [Space(20)]
    public BehaviourTree subtree;

    protected override void OnCancel()
    {
        subtree.Cancel();   
    }

    protected override void OnStart()
    {
        subtree = subtree.Clone();
        subtree.Bind(blackboard);
        subtree.Start();
    }

    protected override void OnStop() { }

    protected override State OnUpdate()
    {
        state = subtree.Update();
        return state;
    }


#if UNITY_EDITOR
    public override void InjectForDuplicate(Node node)
    {
        SubtreeNode n = node as SubtreeNode;
        if (n == null) return;

        subtree = n.subtree;
    }
#endif
}
