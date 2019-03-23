﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryObjectsGenerator : HMM
{
    void Start()
    {
        this.Init();
        this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        this.ParameterInference(new List<int> { 0, 1, 2, 3, 1, 2, 3, 6, 4, 0, 1, 1, 0, 5, 0, 1, 2, 3, 0, 1, 2, 3, 0, 3, 2, 0, 6, 4, 3, 0, 1, 1, 2, 2, 6, 4, 2, 3, 3, 1, 5, 1, 0, 0, 0, 1, 2, 4, 2, 6, 5, 2, 0, 1, 2, 3, 0, 3, 1, 3, 0, 1, 0, 1, 6, 4, 1, 2, 3, 2, 3, 0, 0, 1, 3 });
    }
}