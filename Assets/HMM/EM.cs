using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;
using System.IO;

public class EM
{
    private Matrix<double> transitionMatrix;
    private Matrix<double> emissionMatrix;
    private Vector<double> drawProbabilities;
    public double epsilon = 0.01f;
    private int maxIterations = 300;
    private int stateCount = 0;
    private int emissionCount = 0;

    public EM(int stateCount, int emissionCount)
    {
        this.stateCount = stateCount;
        this.emissionCount = emissionCount;
        Init();
    }

    public double[,] GetTransitionMatrix()
    {
        return transitionMatrix.ToArray();
    }

    public double[,] GetEmissionMatrix()
    {
        return emissionMatrix.ToArray();
    }

    public void SetEmissionMatrix(double[,] matrix)
    {
        this.emissionMatrix = Matrix<double>.Build.DenseOfArray(matrix);
    }

    public void SetTransitionMatrix(double[,] matrix)
    {
        this.transitionMatrix = Matrix<double>.Build.DenseOfArray(matrix);
    }

    private void Init()
    {
        this.drawProbabilities = Vector<double>.Build.Dense(this.stateCount, 1.0d / this.stateCount);
        this.emissionMatrix = Matrix<double>.Build.Random(this.stateCount, this.emissionCount, new MathNet.Numerics.Distributions.ContinuousUniform(0, 10));
        this.transitionMatrix = Matrix<double>.Build.Random(this.stateCount, this.stateCount, new MathNet.Numerics.Distributions.ContinuousUniform(0, 10));
        this.emissionMatrix = Normalize(emissionMatrix, emissionMatrix.RowSums(), "row");
        this.transitionMatrix = Normalize(transitionMatrix, transitionMatrix.RowSums(), "row");
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

    private Matrix<double> Normalize(Matrix<double> target, Vector<double> divisors, string mode)
    {
        for (int i = 0; i < target.RowCount; ++i)
        {
            for (int j = 0; j < target.ColumnCount; ++j)
            {
                if (mode == "row")
                    target[i, j] /= divisors[i];
                if (mode == "column")
                    target[i, j] /= divisors[j];
            }
        }
        return target;
    }

    public void train(List<int> observations)
    {
        Matrix<double> previousJointDistribution = Matrix<double>.Build.Dense(this.emissionCount, this.emissionCount);
        Matrix<double> jointDistribution;
        Matrix<double> emissionDelta;
        Matrix<double> transitionDelta;
        Matrix<double> occurenceMatrix = this.GetOccurenceMatrix(observations);
        this.SetMatrixBar();
        this.emissionMatrix = this.emissionMatrix.Transpose();
        for (int i = 0; i < this.maxIterations; ++i)
        {
            jointDistribution = occurenceMatrix.PointwiseDivide(this.emissionMatrix.Multiply(this.transitionMatrix).Multiply(this.emissionMatrix.Transpose()));
            Debug.Log(jointDistribution.Subtract(previousJointDistribution).L2Norm());
            if (jointDistribution.Subtract(previousJointDistribution).L2Norm() < this.epsilon)
                break;
            transitionDelta = this.transitionMatrix.PointwiseMultiply(this.emissionMatrix.Transpose().Multiply(jointDistribution).Multiply(this.emissionMatrix));
            emissionDelta = this.emissionMatrix.PointwiseMultiply(jointDistribution.Multiply(this.emissionMatrix).Multiply(this.transitionMatrix.Transpose()).Add(jointDistribution.Transpose().Multiply(this.emissionMatrix).Multiply(this.transitionMatrix)));
            this.transitionMatrix = transitionDelta.Divide(transitionDelta.RowSums().Sum());
            this.emissionMatrix = Normalize(emissionDelta, emissionDelta.ColumnSums(), "column");
            previousJointDistribution = jointDistribution;
        }
        this.transitionMatrix = Normalize(this.transitionMatrix, this.transitionMatrix.RowSums(), "row");
        this.emissionMatrix = this.emissionMatrix.Transpose();
    }
    public void SaveEmissionMatrirx(string filename)
    {
        StreamWriter outputFile = new StreamWriter(Path.Combine(Application.dataPath + "/Probabilities", filename));
        for (int i = 0; i < this.emissionMatrix.RowCount; ++i)
        {
            for (int j = 0; j < this.emissionMatrix.ColumnCount; ++j)
            {
                outputFile.Write(this.emissionMatrix[i, j] + " ");
            }
            outputFile.WriteLine();
        }
        outputFile.Close();
    }

    public void SaveTransitionMatrirx(string filename)
    {
        StreamWriter outputFile = new StreamWriter(Path.Combine(Application.dataPath + "/Probabilities", filename));
        for (int i = 0; i < this.transitionMatrix.RowCount; ++i)
        {
            for (int j = 0; j < this.transitionMatrix.ColumnCount; ++j)
            {
                outputFile.Write(this.transitionMatrix[i, j] + " ");
            }
            outputFile.WriteLine();
        }
        outputFile.Close();
    }
}
