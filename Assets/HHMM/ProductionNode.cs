using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProductionNode : Node
{
    [SerializeField]
    private List<GameObject> observations;

    private List<float> emissionsProbabilities { get; set; }

    public TextAsset emissionsTransition;

    public ProductionNode(Node parent) : base(NodeType.productionNode)
    {
        this.parent = parent;
    }

    public override Node Next()
    {
        throw new System.NotImplementedException();
    }
}
