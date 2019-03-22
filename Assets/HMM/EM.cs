using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

//TODO: Warning when iteration spits NaN after training because of division by zero

public class EM : IEstimator
{
    private Matrix<double> transitionMatrix;
    private Matrix<double> emissionMatrix;
    private Vector<double> drawProbabilities;
    private int stateCount = 0;
    private int emissionCount = 0;
    public double epsilon { get; set; } = 0.01f;
    public int maxIterations { get; set; } = 300;

    public EM(int stateCount, int emissionCount)
    {
        this.stateCount = stateCount;
        this.emissionCount = emissionCount;
        Init();
    }

    private void Init()
    {
        this.drawProbabilities = Vector<double>.Build.Dense(this.stateCount, 1.0d / this.stateCount);
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

    public void train(List<int> observations, double[,] transitionProbabilities, double[,] emissionProbabilities, List<double> pi)
    {
        this.SetEmissionMatrix(emissionProbabilities);
        this.SetTransitionMatrix(transitionProbabilities);
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
            if (jointDistribution.Subtract(previousJointDistribution).L1Norm() < this.epsilon)
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
}
