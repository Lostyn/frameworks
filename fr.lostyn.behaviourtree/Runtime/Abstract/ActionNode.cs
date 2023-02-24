using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ActionNode : Node
{
    public override List<Node> GetNexts() { return nexts; }
}
