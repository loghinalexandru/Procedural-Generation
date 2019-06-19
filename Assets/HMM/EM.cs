﻿using System.Collections.Generic;
using System.IO;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class EM : IEstimator
{
    private Matrix<double> transitionMatrix;
    private Matrix<double> emissionMatrix;
    private Vector<double> drawProbabilities;
    private int stateCount = 0;
    private int emissionCount = 0;
    public double epsilon { get; set; } = 1e-6;
    public int maxIterations { get; set; } = 200;

    public EM(int stateCount, int emissionCount)
    {
        this.stateCount = stateCount;
        this.emissionCount = emissionCount;
    }

    public double[,] GetTransitionMatrix()
    { 
        //for(int i = 0; i < transitionMatrix.RowCount; ++i)
        //    MathNet.Numerics.Sorting.Sort(transitionMatrix.Row(i));
        return transitionMatrix.ToArray();
    }

    public double[,] GetEmissionMatrix()
    {
        //for (int i = 0; i < emissionMatrix.RowCount; ++i)
        //    MathNet.Numerics.Sorting.Sort(emissionMatrix.Row(i));
        return emissionMatrix.ToArray();
    }

    public double[] GetStartProbabilities()
    {
        //MathNet.Numerics.Sorting.Sort(drawProbabilities);
        return drawProbabilities.ToArray();
    }

    public void SetEmissionMatrix(double[,] matrix)
    {
        this.emissionMatrix = Matrix<double>.Build.DenseOfArray(matrix);
    }

    public void SetTransitionMatrix(double[,] matrix)
    {
        this.transitionMatrix = Matrix<double>.Build.DenseOfArray(matrix);
    }

    public void SetDrawProbabilities(List<double> probabilities)
    {
        this.drawProbabilities = Vector<double>.Build.DenseOfArray(probabilities.ToArray());
    }

    private Matrix<double> GetOccurenceMatrix(List<int> observations)
    {
        Matrix<double> occurences = Matrix<double>.Build.Dense(this.emissionCount, this.emissionCount);
        for (int i = 0; i < observations.Count - 1; ++i)
        {
            occurences[observations[i], observations[i + 1]] += 1;
        }
        return occurences;
    }

    private void SetMatrixBar()
    {
        for (int i = 0; i < this.transitionMatrix.RowCount; ++i)
        {
            for (int j = 0; j < this.transitionMatrix.ColumnCount; ++j)
            {
                this.transitionMatrix[i, j] *= this.drawProbabilities[i];
            }
        }
    }

    private void saveLog(List<double> value , string path)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";

        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(Application.dataPath + Path.GetDirectoryName(path), Path.GetFileName(path)),true))
        {
            for(int i = 0; i < value.Count; ++i)
            {
                outputFile.Write(System.Convert.ToDecimal(value[i]) + ",");
            }
            outputFile.WriteLine();
            outputFile.WriteLine();
            outputFile.WriteLine();
        }
    }

    private Matrix<double> Normalize(Matrix<double> target, Vector<double> divisors, string mode)
    {
        Matrix<double> output = Matrix<double>.Build.DenseOfMatrix(target);
        for (int i = 0; i < target.RowCount; ++i)
        {
            for (int j = 0; j < target.ColumnCount; ++j)
            {
                if (mode == "row")
                    output[i, j] /= divisors[i];
                if (mode == "column")
                    output[i, j] /= divisors[j];
            }
        }
        return output;
    }

    //public double GetLikelihood(List<int> observations, Matrix<double> transitionMatrix, Matrix<double> emissionMatrix)
    //{
    //    if (observations.Count < 0)
    //        throw new System.Exception("Empty observations list!");
    //    Vector<double> previousValues = Vector<double>.Build.Dense(this.stateCount);
    //    Vector<double> currentValues = Vector<double>.Build.Dense(this.stateCount);
    //    currentValues = this.drawProbabilities.PointwiseMultiply(emissionMatrix.Column(observations[0]));
    //    for (int i = 1; i < observations.Count; ++i)
    //    {
    //        currentValues.CopyTo(previousValues);
    //        for (int j = 0; j < this.stateCount; ++j)
    //            currentValues[j] = previousValues.PointwiseMultiply(transitionMatrix.Column(j)).Sum() * emissionMatrix[j, observations[i]];
    //    }
    //    return System.Math.Log(currentValues.Sum());
    //}

    public double GetLikelihood(Matrix<double> occurence, Matrix<double> transition, Matrix<double> emission, Matrix<double> cBar)
    {
        double likelihood = 0;
        for (int i = 0; i < this.emissionCount; ++i)
        {
            for (int j = 0; j < this.emissionCount; ++j)
            {
                double innerSum = 0;
                for (int k = 0; k < this.stateCount; ++k)
                {
                    for (int l = 0; l < this.stateCount; ++l)
                    {
                        double result = ((emission[k, i] * transition[k, l] * emission[l, j]) * (System.Math.Log(emission[k, i]) + System.Math.Log(transition[k, l]) + System.Math.Log(emission[l, j]))) / cBar[i, j];
                        if (!double.IsNaN(result))
                            innerSum += result;
                    }
                }
                likelihood += occurence[i, j] * innerSum;
            }
        }
        return likelihood;
    }

    public double Train(List<int> observations, double[,] transitionProbabilities, double[,] emissionProbabilities, List<double> pi)
    {
        this.SetEmissionMatrix(emissionProbabilities);
        this.SetTransitionMatrix(transitionProbabilities);
        this.SetDrawProbabilities(pi);
        Matrix<double> previousJoinDistribution = Matrix<double>.Build.Dense(this.emissionCount, this.emissionCount);
        Matrix<double> previousEmission = Matrix<double>.Build.Dense(this.stateCount, this.emissionCount);
        Matrix<double> previousTransition = Matrix<double>.Build.Dense(this.stateCount, this.stateCount);
        Matrix<double> jointDistribution;
        Matrix<double> emissionDelta;
        Matrix<double> transitionDelta;
        Matrix<double> occurenceMatrix = this.GetOccurenceMatrix(observations);
        List<double> logs = new List<double>();
        this.SetMatrixBar();
        this.emissionMatrix = this.emissionMatrix.Transpose();
        previousEmission = previousEmission.Transpose();
        double likelihood = 0.0f;
        for (int i = 0; i < this.maxIterations; ++i)
        {
            jointDistribution = occurenceMatrix.PointwiseDivide(this.emissionMatrix.Multiply(this.transitionMatrix).Multiply(this.emissionMatrix.Transpose()));
            transitionDelta = this.transitionMatrix.PointwiseMultiply(this.emissionMatrix.Transpose().Multiply(jointDistribution).Multiply(this.emissionMatrix));
            emissionDelta = this.emissionMatrix.PointwiseMultiply(jointDistribution.Multiply(this.emissionMatrix).Multiply(this.transitionMatrix.Transpose()).Add(jointDistribution.Transpose().Multiply(this.emissionMatrix).Multiply(this.transitionMatrix)));
            this.transitionMatrix = transitionDelta.Divide(transitionDelta.RowSums().Sum());
            this.emissionMatrix = Normalize(emissionDelta, emissionDelta.ColumnSums(), "column");
            if (this.transitionMatrix.Subtract(previousTransition).L2Norm() < this.epsilon && this.emissionMatrix.Subtract(previousEmission).L2Norm() < this.epsilon && jointDistribution.Subtract(previousJoinDistribution).L2Norm() < this.epsilon)
                break;
            previousEmission = emissionMatrix;
            previousTransition = transitionMatrix;
            previousJoinDistribution = jointDistribution;
            //logs.Add(GetLikelihood(occurenceMatrix, this.transitionMatrix, this.emissionMatrix.Transpose(), this.emissionMatrix.Multiply(this.transitionMatrix).Multiply(this.emissionMatrix.Transpose())));
            //Debug.Log(GetLikelihood(occurenceMatrix, this.transitionMatrix, this.emissionMatrix.Transpose(), this.emissionMatrix.Multiply(this.transitionMatrix).Multiply(this.emissionMatrix.Transpose())));
        }
        likelihood = GetLikelihood(occurenceMatrix, this.transitionMatrix, this.emissionMatrix.Transpose(), this.emissionMatrix.Multiply(this.transitionMatrix).Multiply(this.emissionMatrix.Transpose()));
        this.transitionMatrix = Normalize(this.transitionMatrix, this.transitionMatrix.RowSums(), "row");
        this.emissionMatrix = this.emissionMatrix.Transpose();
        //saveLog(logs, "/Resources/logs.txt");
        return likelihood;
    }
}
