using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class HMM : MonoBehaviour
{
    private float[,] transitionProbabilities;
    private float[,] emissionProbabilities;
    private int currentStateIndex = 0;
    private int stateCount;
    private int emissionCount;
    private EM estimator;
    public List<float> startProbabilities;
    public List<GameObject> emissions;
    public TextAsset transitionFile;
    public TextAsset emissionFile;

    private void Init()
    {
        this.stateCount = this.startProbabilities.Count;
        this.emissionCount = this.emissions.Count;
        this.emissionProbabilities = new float[this.stateCount, this.emissionCount];
        this.transitionProbabilities = new float[this.stateCount, this.stateCount];
        this.SetTransitionProbabilities();
        this.SetEmissionProbabilities();
        this.SetInitialState();
        this.estimator = new EM(this.stateCount, this.emissionCount);
    }

    private void SetTransitionProbabilities()
    {
        string[] text = this.transitionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            string[] probabilities = text[i].Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                this.transitionProbabilities[i, j] = float.Parse(probabilities[j]);
            }
        }
    }

    private void SetEmissionProbabilities()
    {
        string[] text = this.emissionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            string[] probabilities = text[i].Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                this.emissionProbabilities[i, j] = float.Parse(probabilities[j]);
            }
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
        for (int j = 0; j < this.stateCount; ++j)
        {
            start = end;
            end += this.transitionProbabilities[state, j];
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
        for (int j = 0; j < this.emissionCount; ++j)
        {
            start = end;
            end += this.emissionProbabilities[state, j];
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
        Debug.Log(emissionIndex);
        return this.emissions[emissionIndex];
    }

    void Start()
    {
        Init();
        this.ParameterInference(null);
    }

    public void ParameterInference(List<GameObject> observations)
    {
        //estimator.SetNullNodes();
        estimator.train(new List<int> {
            0,1,2,1,0,2,1,0,2,2,1,2,1,0,3,5,4,3,4,5,5,5,4,3,4,5,3,5,3,5,8,8,8,8,6,8,7,8,6,7,8,6,8,8,0,1,2,1,2,2,2,0,1,0,2,1,0
            ,4,5,5,3,4,5,4,5,5,5,7,6,7,8,8,8,6,7,6,7,8,8,6,8,0,1,2,1,0,2,1,0,2,2,1,2,1,0,3,5,4,3,4,5,5,5
        });
        //estimator.SaveEmissionMatrirx("EM_emission.txt");
        //estimator.SaveTransitionMatrirx("EM_transition.txt");
        this.emissionProbabilities = estimator.GetEmissionMatrix();
        this.transitionProbabilities = estimator.GetTransitionMatrix();
    }
}
