using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public abstract class HMM : MonoBehaviour
{
    private double[,] transitionProbabilities;
    private double[,] emissionProbabilities;
    private double[,] transitionCumulativeProbabilities;
    private double[,] emissionCumulativeProbabilities;
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
        this.SetCumulativeDistributions();
    }

    private void SetCumulativeDistributions()
    {
        this.emissionCumulativeProbabilities = GetCumulativeDistribution(this.emissionProbabilities);
        this.transitionCumulativeProbabilities = GetCumulativeDistribution(this.transitionProbabilities);
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

    private double[,] GetCumulativeDistribution(double[,] matrix)
    {
        double[,] output = new double[matrix.GetLength(0) , matrix.GetLength(1)];
        for(int i = 0; i < matrix.GetLength(0); ++i)
        {
            double sum = 0.0f;
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                sum += matrix[i, j];
                output[i, j] = sum;
            }
        }
        return output;
    }

    private void SetFileTransitionProbabilities()
    {
        string[] text = this.transitionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            double[] probabilities = System.Array.ConvertAll(text[i].Trim().Split(' '), double.Parse);
            System.Array.Sort(probabilities);
            for (int j = 0; j < probabilities.Length; ++j)
            {
                this.transitionProbabilities[i, j] = probabilities[j];
            }
        }
    }

    private void SetFileEmissionProbabilities()
    {
        string[] text = this.emissionFile.text.Split('\n');
        for (int i = 0; i < text.Length; ++i)
        {
            double[] probabilities = System.Array.ConvertAll(text[i].Trim().Split(' '), double.Parse);
            System.Array.Sort(probabilities);
            for (int j = 0; j < probabilities.Length; ++j)
            {
                this.emissionProbabilities[i, j] = probabilities[j];
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

    private void Normalize(double[,] target, double[] divisors)
    {
        for (int i = 0; i < target.GetLength(0); ++i)
        {
            for (int j = 0; j < target.GetLength(1); ++j)
            {
                target[i, j] /= divisors[i];
            }
        }
    }

    private void SetRandomMatrix(double[,] matrix)
    {
        double[] divisors = new double[matrix.GetLength(0)];
        double rowMax = 0.0f;
        for (int i = 0; i < matrix.GetLength(0); ++i)
        {
            rowMax = 0;
            for (int j = 0; j < matrix.GetLength(1); ++j)
            {
                matrix[i, j] = Random.Range(0.0f, 100.0f);
                rowMax += matrix[i, j];
            }
            divisors[i] = rowMax;
        }
        Normalize(matrix, divisors);
    }

    private void SetRandomStart()
    {
        double rowMax = 0;
        for (int i = 0; i < this.stateStartProbabilities.Count; ++i)
        {
            this.stateStartProbabilities[i] = Random.Range(0.0f, 100.0f);
            rowMax += this.stateStartProbabilities[i];
        }
        stateStartProbabilities = stateStartProbabilities.Select(entry => entry = entry / rowMax).ToList();
    }
    public void SetRandomProbabilities()
    {
        SetRandomStart();
        SetRandomMatrix(this.transitionProbabilities);
        SetRandomMatrix(this.emissionProbabilities);
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

    private int BinarySearch(int state , double target , double[,] matrix)
    {
        int left = 0;
        int right = matrix.GetLength(1) - 1;
        while (left <= right)
        {
            int midpoint = (left + right) / 2;
            if(target <= matrix[state , midpoint])
            {
                if (midpoint == 0 || (midpoint > 0 && target > matrix[state, midpoint - 1]))
                    return midpoint;
                right = midpoint - 1;
            }
            else
            {
                left = midpoint + 1;
            }
        }
        return -1;
    }

    private int MakeTransition(int state)
    {
        return BinarySearch(state, Random.value, transitionCumulativeProbabilities);
    }

    private int MakeEmission(int state)
    {
        return BinarySearch(state, Random.value, emissionCumulativeProbabilities);
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
                startProbabilites = estimator.GetStartProbabilities();
            }
            this.SetRandomProbabilities();
        }
        this.emissionProbabilities = bestEmission;
        this.transitionProbabilities = bestTransition;
        this.stateStartProbabilities = new List<double>(startProbabilites);
        this.currentStateIndex = this.SetInitialState();
        this.SetCumulativeDistributions();
        Debug.Log(maxLoglikelihood);
    }
}
