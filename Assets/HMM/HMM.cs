using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public abstract class HMM : MonoBehaviour
{
    private double[,] transitionProbabilities;
    private double[,] emissionProbabilities;
    private int currentStateIndex = 0;
    public int maxParalelModels = 10;
    public IEstimator estimator { get; set; }
    public List<double> stateStartProbabilities;
    public List<GameObject> emissions;
    public TextAsset transitionFile;
    public TextAsset emissionFile;

    //IMPORTANT : Call this from child class in void Start()
    protected void Init()
    {
        this.emissionProbabilities = new double[this.stateStartProbabilities.Count, this.emissions.Count];
        this.transitionProbabilities = new double[this.stateStartProbabilities.Count, this.stateStartProbabilities.Count];
        this.currentStateIndex = this.SetInitialState();
        this.SetProbabilities();
    }

    private void SetProbabilities()
    {
        if (this.emissionFile == null || this.transitionFile == null)
        {
            this.SetRandomProbabilities();
        }
        else
        {
            this.SetFileEmissionProbabilities();
            this.SetFileTransitionProbabilities();
        }
    }

    private void SetFileTransitionProbabilities()
    {
        string[] text = this.transitionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            string[] probabilities = text[i].Trim().Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                this.transitionProbabilities[i, j] = double.Parse(probabilities[j]);
            }
        }
    }

    private void SetFileEmissionProbabilities()
    {
        string[] text = this.emissionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            string[] probabilities = text[i].Trim().Split(' ');
            for (int j = 0; j < probabilities.Length; ++j)
            {
                this.emissionProbabilities[i, j] = double.Parse(probabilities[j]);
            }
        }
    }

    public void SaveEmissionProbabilities(string path)
    {
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(Application.dataPath + Path.GetDirectoryName(path), Path.GetFileName(path))))
        {
            for (int i = 0; i < this.emissionProbabilities.GetLength(0); ++i)
            {
                for (int j = 0; j < this.emissionProbabilities.GetLength(1); ++j)
                {
                    outputFile.Write(this.emissionProbabilities[i, j] + " ");
                }
                if (i != this.transitionProbabilities.GetLength(0) - 1)
                    outputFile.WriteLine();
            }
        }
    }

    public void SaveTransitionProbabilities(string path)
    {
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(Application.dataPath + Path.GetDirectoryName(path), Path.GetFileName(path))))
        {
            for (int i = 0; i < this.transitionProbabilities.GetLength(0); ++i)
            {
                for (int j = 0; j < this.transitionProbabilities.GetLength(1); ++j)
                {
                    outputFile.Write(this.transitionProbabilities[i, j] + " ");
                }
                if (i != this.transitionProbabilities.GetLength(0) - 1)
                    outputFile.WriteLine();
            }
        }
    }

    private void Normalize(double[,] target, List<double> divisors)
    {
        for (int i = 0; i < target.GetLength(0); ++i)
        {
            for (int j = 0; j < target.GetLength(1); ++j)
            {
                target[i, j] /= divisors[i];
            }
        }
    }
    //TODO: Refactor this function
    public void SetRandomProbabilities()
    {
        double rowMax = 0;
        List<double> divisors = new List<double>();
        for (int i = 0; i < this.stateStartProbabilities.Count; ++i)
        {
            rowMax = 0;
            for (int j = 0; j < this.stateStartProbabilities.Count; ++j)
            {
                this.transitionProbabilities[i, j] = Random.Range(0.0f, 10.0f);
                rowMax += this.transitionProbabilities[i, j];
            }
            divisors.Add(rowMax);
        }
        Normalize(this.transitionProbabilities, divisors);
        divisors.Clear();
        for (int i = 0; i < this.stateStartProbabilities.Count; ++i)
        {
            rowMax = 0;
            for (int j = 0; j < this.emissions.Count; ++j)
            {
                this.emissionProbabilities[i, j] = Random.Range(0.0f, 10.0f);
                rowMax += this.emissionProbabilities[i, j];
            }
            divisors.Add(rowMax);
        }
        Normalize(this.emissionProbabilities, divisors);
        rowMax = 0;
        for (int i = 0; i < this.stateStartProbabilities.Count; ++i)
        {
            this.stateStartProbabilities[i] = Random.Range(0.0f, 10.0f);
            rowMax += this.stateStartProbabilities[i];
        }
        stateStartProbabilities = stateStartProbabilities.Select(entry => entry = entry / rowMax).ToList();
    }

    private int SetInitialState()
    {
        int index = -1;
        double start = 0.0f;
        double end = 0.0f;
        double randomValue = Random.value;
        for (int i = 0; i < this.stateStartProbabilities.Count; ++i)
        {
            start = end;
            end += stateStartProbabilities[i];
            if (start <= randomValue && randomValue <= end)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    private int MakeTransition(int state)
    {
        int index = -1;
        double start = 0.0f;
        double end = 0.0f;
        double randomValue = Random.value;
        for (int j = 0; j < this.stateStartProbabilities.Count; ++j)
        {
            start = end;
            end += this.transitionProbabilities[state, j];
            if (start <= randomValue && randomValue <= end)
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
        double randomValue = Random.value;
        for (int j = 0; j < this.emissions.Count; ++j)
        {
            start = end;
            end += this.emissionProbabilities[state, j];
            if (start <= randomValue && randomValue <= end)
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
        this.currentStateIndex = MakeTransition(currentStateIndex);
        return this.emissions[emissionIndex];
    }

    public int[] NextEmission(int count)
    {
        int[] output = new int[count];
        for(int i = 0; i < count; ++i)
        {
            int emissionIndex = MakeEmission(this.currentStateIndex);
            this.currentStateIndex = MakeTransition(currentStateIndex);
            output[i] = emissionIndex;
        }
        return output;
    }

    public List<int> GameObjectToIndex(List<GameObject> observations)
    {
        List<int> output = new List<int>();
        for (int i = 0; i < observations.Count; ++i)
        {
            output.Add(this.emissions.IndexOf(observations[i]));
        }
        return output;
    }

    public void ParameterInference(List<GameObject> observations)
    {
        estimator.train(GameObjectToIndex(observations), this.transitionProbabilities, this.emissionProbabilities, this.stateStartProbabilities);
        this.emissionProbabilities = estimator.GetEmissionMatrix();
        this.transitionProbabilities = estimator.GetTransitionMatrix();
    }

    public void ParameterInference(List<int> observations)
    {
        double[,] bestEmission = this.emissionProbabilities;
        double[,] bestTransition = this.transitionProbabilities;
        double[] startProbabilites = this.stateStartProbabilities.ToArray();
        double maxLoglikelihood = double.MinValue;
        for (int i = 0; i < this.maxParalelModels; ++i)
        {
            double likelihood = estimator.train(observations, this.transitionProbabilities, this.emissionProbabilities, this.stateStartProbabilities);
            if (maxLoglikelihood < likelihood)
            {
                maxLoglikelihood = likelihood;
                bestEmission = estimator.GetEmissionMatrix();
                bestTransition = estimator.GetTransitionMatrix();
                startProbabilites = this.stateStartProbabilities.ToArray();
            }
            this.SetRandomProbabilities();
        }
        this.emissionProbabilities = bestEmission;
        this.transitionProbabilities = bestTransition;
        this.stateStartProbabilities = new List<double>(startProbabilites);
        this.currentStateIndex = this.SetInitialState();
        Debug.Log(maxLoglikelihood);
    }
}
