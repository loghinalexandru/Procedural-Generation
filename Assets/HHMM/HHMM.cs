using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HHMM : MonoBehaviour
{
    private StateNode root;
    private List<List<float>> probabilities;
    public TextAsset probabilitiesFile;
    private INode currentState;

    public void Start()
    {
        this.Init();

    }

    public void Init()
    {
        root = new StateNode(null);
        currentState = root;
        StateNode left = new StateNode(this.root);
        StateNode right = new StateNode(this.root);
        StateNode straight = new StateNode(this.root);
        TerminalNode terminal = new TerminalNode(this.root);

        root.AddDescendant(left, 0.33f);
        root.AddDescendant(right, 0.33f);
        root.AddDescendant(straight, 0.33f);

        left.AddNeighbour(left, 0.0f);
        left.AddNeighbour(right, 0.5f);
        left.AddNeighbour(straight, 0.5f);

        right.AddNeighbour(left, 0.5f);
        right.AddNeighbour(right, 0.0f);
        right.AddNeighbour(straight, 0.5f);

        straight.AddNeighbour(left, 0.3f);
        straight.AddNeighbour(right, 0.3f);
        straight.AddNeighbour(straight, 0.3f);
        straight.AddNeighbour(terminal, 0.1f); //Return to Root Node only if straight

        ProductionNode[] prod = GetComponents<ProductionNode>();

        left.AddDescendant(prod[0], 1.0f);
        right.AddDescendant(prod[1], 1.0f);
        straight.AddDescendant(prod[2], 1.0f);
    }

    public void LoadProbabilities()
    {
        string[] text = this.probabilitiesFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            List<float> line = new List<float>();
            string[] probabilities = text[i].Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                line.Add(float.Parse(probabilities[j]));
            }
            this.probabilities.Add(line);
        }
    }

    public GameObject NextEmission()
    {
        GameObject output;
        while (this.currentState.type != NodeType.productionNode)
        {
            this.currentState = this.currentState.Next();
        }
        output = (this.currentState as ProductionNode).GetNextEmission();
        this.currentState = this.currentState.Next();
        return output;
    }
}
