﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEstimator
{
    int maxIterations { get; set; }
    double epsilon { get; set; }
    void train(List<int> observations, double[,] transitionProbabilities, double[,] emissionProbabilities, List<double> pi);
    double[,] GetEmissionMatrix();
    double[,] GetTransitionMatrix();
}
