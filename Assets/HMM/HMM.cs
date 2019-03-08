using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMM : MonoBehaviour
{
    private List<List<float>> transitionProbabilities;
    private List<List<float>> emissionProbabilities;
    private int currentStateIndex = 0;
    public List<float> startProbabilities;
    public List<GameObject> emissions;
    public TextAsset transitionFile;
    public TextAsset emissionFile;

    private void Init()
    {
        this.emissionProbabilities = new List<List<float>>();
        this.transitionProbabilities = new List<List<float>>();
        this.SetTransitionProbabilities();
        this.SetEmissionProbabilities();
        this.SetInitialState();
    }

    private void SetTransitionProbabilities()
    {
        string[] text = this.transitionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            List<float> line = new List<float>();
            string[] probabilities = text[i].Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                line.Add(float.Parse(probabilities[j]));
            }
            this.transitionProbabilities.Add(line);
        }
    }

    private void SetEmissionProbabilities()
    {
        string[] text = this.emissionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            List<float> line = new List<float>();
            string[] probabilities = text[i].Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                line.Add(float.Parse(probabilities[j]));
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
        int index = -1;
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

    private int MakeEmission(int state)
    {
        int index = -1;
        double start = 0.0f;
        double end = 0.0f;
        float randomValue = Random.value;
        for (int j = 0; j < this.emissionProbabilities[state].Count; ++j)
        {
            start = end;
            end += this.emissionProbabilities[state][j];
            if (start < randomValue && randomValue < end)
            {
                index = j;
                break;
            }
        }
        return index;
    }

    public GameObject NextEmission()
    {
        int emissionIndex = MakeEmission(this.currentStateIndex);
        while (emissionIndex == -1)
        {
            this.currentStateIndex = MakeTransition(currentStateIndex);
            emissionIndex = MakeEmission(this.currentStateIndex);
        }
        this.currentStateIndex = MakeTransition(currentStateIndex);
        return this.emissions[emissionIndex];
    }

    void Start()
    {
        Init();
    }
}
