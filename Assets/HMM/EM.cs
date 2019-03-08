using System.Collections;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public class EM
{
    private Matrix<float> transitionMatrix;
    private Matrix<float> emissionMatrix;
    private Vector<float> drawProbabilities;
    private int maxIterations = 2;
    private int stateCount = 0;
    private int emissionCount = 0;

    public EM(int stateCount, int emissionCount)
    {
        this.stateCount = stateCount;
        this.emissionCount = emissionCount;
        Init();
    }

    public float[,] GetTransitionMatrix()
    {
        return transitionMatrix.ToArray();
    }

    public float[,] EmissionMatrix()
    {
        return emissionMatrix.ToArray();
    }

    public void SetEmissionMatrix(float[,] matrix)
    {
        this.emissionMatrix = Matrix<float>.Build.DenseOfArray(matrix);
    }

    public void SetTransitionMatrix(float[,] matrix)
    {
        this.transitionMatrix = Matrix<float>.Build.DenseOfArray(matrix);
    }

    private void Init()
    {
        this.drawProbabilities = Vector<float>.Build.Dense(this.stateCount, 1.0f / this.stateCount);
        this.emissionMatrix = Matrix<float>.Build.Random(this.stateCount, this.emissionCount, new MathNet.Numerics.Distributions.ContinuousUniform(0, 10));
        this.transitionMatrix = Matrix<float>.Build.Random(this.stateCount, this.stateCount, new MathNet.Numerics.Distributions.ContinuousUniform(0, 10));
        this.emissionMatrix = Normalize(emissionMatrix, emissionMatrix.RowSums(), "row");
        this.transitionMatrix = Normalize(transitionMatrix, transitionMatrix.RowSums(), "row");
    }

    private Matrix<float> GetOccurenceMatrix(List<int> observations)
    {
        Matrix<float> occurences = Matrix<float>.Build.Dense(this.emissionCount, this.emissionCount);
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

    private Matrix<float> Normalize(Matrix<float> target, Vector<float> divisors, string mode)
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
        Matrix<float> intermediate;
        Matrix<float> emissionPrime;
        Matrix<float> transitionPrime;
        Matrix<float> occurenceMatrix = this.GetOccurenceMatrix(observations);
        this.SetMatrixBar();
        this.emissionMatrix = this.emissionMatrix.Transpose();
        for (int i = 0; i < this.maxIterations; ++i)
        {
            intermediate = occurenceMatrix.PointwiseDivide(this.emissionMatrix.Multiply(this.transitionMatrix).Multiply(this.emissionMatrix.Transpose()));
            transitionPrime = this.transitionMatrix.PointwiseMultiply(this.emissionMatrix.Transpose().Multiply(intermediate).Multiply(this.emissionMatrix));
            emissionPrime = this.emissionMatrix.PointwiseMultiply(intermediate.Multiply(this.emissionMatrix).Multiply(this.transitionMatrix.Transpose()).Add(intermediate.Transpose().Multiply(this.emissionMatrix).Multiply(this.transitionMatrix)));
            this.transitionMatrix = transitionPrime.Divide(transitionPrime.RowSums().Sum());
            this.emissionMatrix = Normalize(emissionPrime, emissionPrime.ColumnSums(), "column");
        }
        this.transitionMatrix = Normalize(this.transitionMatrix, this.transitionMatrix.RowSums(), "row");
        this.emissionMatrix = this.emissionMatrix.Transpose();
        Debug.Log(this.transitionMatrix);
    }
}
