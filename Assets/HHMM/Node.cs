using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType { stateNode, productionNode, terminalNode };


public abstract class Node : MonoBehaviour
{
    protected NodeType type { get; set; }
    protected Node parent { get; set; }

    public Node(NodeType type)
    {
        this.type = type;
    }

    public abstract Node Next();

}