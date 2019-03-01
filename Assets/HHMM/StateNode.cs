using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateNode : Node
{
    [SerializeField]
    private List<Node> descendents;

    [SerializeField]
    private List<Node> neighbours;

    private List<float> descendentsProbabilities { get; set; }
    private List<float> neighboursProbabilities { get; set; }

    public TextAsset neighboursTransition;
    public TextAsset descendentsTransition;

    public StateNode(Node parent) : base(NodeType.stateNode)
    {
        this.parent = parent;
    }

    public override Node Next()
    {
        return null;
    }
}
