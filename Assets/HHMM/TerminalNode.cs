using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerminalNode : Node
{
    public TerminalNode(Node parent) : base(NodeType.terminalNode)
    {
        this.parent = parent;
    }

    public override Node Next()
    {
        return this.parent;
    }
}
