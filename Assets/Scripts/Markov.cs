using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Markov : MonoBehaviour
{
    public List<GameObject> states;
    public List<GameObject> emissions;
    public List<double> startProbabilities = new List<double>();
    public TextAsset transitionFile;
    public TextAsset emissionFile;
    private List<List<double>> transitionProbabilities;
    private List<List<double>> emissionProbabilities;
    private int currentStateIndex = 0;

    public Markov()
    {
        this.transitionProbabilities = new List<List<double>>();
        this.emissionProbabilities = new List<List<double>>();
    }


    public void Start()
    {
        this.SetTransitionProbabilities();
        this.SetEmissionProbabilities();
    }


    public void Update()
    {
        this.Next();
    }

    private void SetTransitionProbabilities()
    {
        string[] text = this.transitionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            List<double> line = new List<double>();
            string[] probabilities = text[i].Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                line.Add(double.Parse(probabilities[j]));
            }
            this.transitionProbabilities.Add(line);
        }
    }

    private void SetEmissionProbabilities()
    {
        string[] text = this.emissionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            List<double> line = new List<double>();
            string[] probabilities = text[i].Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                line.Add(double.Parse(probabilities[j]));
            }
            this.emissionProbabilities.Add(line);
        }
    }

    private void SetInitialState()
    {
        double start = 0.0f;
        double end = 0.0f;
        float randomValue = Random.value;
        for (int i = 0; i < this.startProbabilities.Count; ++i)
        {
            start = end;
            end += startProbabilities[i];
            if (start <= randomValue && randomValue <= end)
            {
                this.currentStateIndex = i;
                break;
            }
        }
    }

    private int MakeTransition(int state)
    {
        int index = 0;
        double start = 0.0f;
        double end = 0.0f;
        float randomValue = Random.value;
        for (int j = 0; j < this.transitionProbabilities[state].Count; ++j)
        {
            start = end;
            end += this.transitionProbabilities[state][j];
            if (start < randomValue && randomValue < end)
            {
                index = j;
                break;
            }
        }
        return index;
    }

    public List<GameObject> GenerateStates(int size)
    {
        List<GameObject> output = new List<GameObject>();
        int stateIndex = this.currentStateIndex;

        for (int i = 0; i < size; ++i)
        {
            Debug.Log(stateIndex);
            output.Add(this.states[stateIndex]);
            stateIndex = this.MakeTransition(stateIndex);
        }
        return output;
    }

    public GameObject Next()
    {
        Debug.Log(this.currentStateIndex);
        int oldState = this.currentStateIndex;
        this.currentStateIndex = MakeTransition(currentStateIndex);
        return this.states[oldState];
    }
}
