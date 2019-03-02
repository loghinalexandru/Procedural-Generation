using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNode : INode
{
    private List<INode> descendents { get; set; }
    private List<INode> neighbours { get; set; }
    private List<float> descendentsProbabilities { get; set; }
    private List<float> neighboursProbabilities { get; set; }
    public NodeType type { get; set; } = NodeType.stateNode;
    public INode parent { get; set; }
    public Direction direction { get; set; } = Direction.vertical;

    public StateNode(INode parentNode)
    {
        this.parent = parentNode;
        this.descendents = new List<INode>();
        this.neighbours = new List<INode>();
        this.descendentsProbabilities = new List<float>();
        this.neighboursProbabilities = new List<float>();
    }

    public void AddDescendant(INode child, float probability)
    {
        child.parent = this;
        this.descendents.Add(child);
        this.descendentsProbabilities.Add(probability);
    }

    public void AddNeighbour(INode neighbour, float probability)
    {
        this.neighbours.Add(neighbour);
        this.neighboursProbabilities.Add(probability);
    }


    private INode GetNextVerticalNode()
    {
        int index = 0;
        double start = 0.0f;
        double end = 0.0f;
        float randomValue = Random.value;
        for (int j = 0; j < this.descendentsProbabilities.Count; ++j)
        {
            start = end;
            end += this.descendentsProbabilities[j];
            if (start < randomValue && randomValue < end)
            {
                index = j;
                break;
            }
        }
        return descendents[index];
    }

    private INode GetNextHorizontalNode()
    {
        int index = 0;
        double start = 0.0f;
        double end = 0.0f;
        float randomValue = Random.value;
        for (int j = 0; j < this.neighboursProbabilities.Count; ++j)
        {
            start = end;
            end += this.neighboursProbabilities[j];
            if (start < randomValue && randomValue < end)
            {
                index = j;
                break;
            }
        }
        this.direction = Direction.vertical;
        return neighbours[index];
    }

    public INode Next()
    {
        if (this.direction == Direction.horizontal && this.neighbours.Count > 0)
            return GetNextHorizontalNode();
        else
            return GetNextVerticalNode();
    }
}
