using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HHMM))]
public class ProductionNode : MonoBehaviour, INode
{
    public List<GameObject> emissions;
    public List<float> emissionsProbabilities;
    public NodeType type { get; set; } = NodeType.productionNode;
    public INode parent { get; set; }
    public Direction direction { get; set; } = Direction.vertical;

    public ProductionNode(INode parent)
    {
        this.parent = parent;
        emissions = new List<GameObject>();
        emissionsProbabilities = new List<float>();
    }

    public GameObject GetNextEmission()
    {
        int index = 0;
        double start = 0.0f;
        double end = 0.0f;
        float randomValue = Random.value;
        for (int j = 0; j < this.emissionsProbabilities.Count; ++j)
        {
            start = end;
            end += this.emissionsProbabilities[j];
            if (start < randomValue && randomValue < end)
            {
                index = j;
                break;
            }
        }
        return emissions[index];
    }

    public INode Next()
    {
        this.parent.direction = Direction.horizontal;
        return this.parent;
    }

    void Start()
    {
        this.type = NodeType.productionNode;
        if (this.emissionsProbabilities.Count == 0)
        {
            throw new System.Exception("No emission probabilities!");
        }
    }
}
