using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class IfNode : Node
{
    Color _nodeColor = new Color(0.55f, 0.46f, 0.56f);
    public override Color NodeColor => _nodeColor;
}
