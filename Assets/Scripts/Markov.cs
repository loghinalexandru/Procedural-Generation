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

    public Markov()
    {
        this.transitionProbabilities = new List<List<double>>();
        this.emissionProbabilities = new List<List<double>>();
    }


    public void Start()
    {
        this.SetTransitionProbabilities();
        this.SetEmissionProbabilities();
        this.GenerateStates(10);
    }


    public void Update()
    {

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

    public List<GameObject> GenerateStates(int size)
    {
        List<GameObject> output = new List<GameObject>();
        int currentStateIndex = 0;
        double start = 0.0f;
        double end = 0.0f;
        float randomValue = Random.value;
        for (int i = 0; i < this.startProbabilities.Count; ++i)
        {
            start = end;
            end += startProbabilities[i];
            if (start <= randomValue && randomValue <= end)
            {
                currentStateIndex = i;
                output.Add(this.states[i]);
                break;
            }
        }
        Debug.Log(currentStateIndex);

        for (int i = 0; i < size - 1; ++i)
        {
            Debug.Log(currentStateIndex);
            start = 0.0f;
            end = 0.0f;
            randomValue = Random.value;
            for (int j = 0; j < this.transitionProbabilities[currentStateIndex].Count; ++j)
            {
                start = end;
                end += this.transitionProbabilities[currentStateIndex][j];
                if (start < randomValue && randomValue < end)
                {
                    output.Add(this.states[j]);
                    currentStateIndex = j;
                    break;
                }
            }

        }
        return output;
    }
}
