using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertObjectsGenerator : HMM
{
    void Start()
    {
        this.Init();
        this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        this.ParameterInference(new List<int> { 0, 1, 2, 4, 1, 2, 1, 4, 1, 4, 2, 1, 4, 4, 0, 1, 4, 3, 4, 4, 2, 1, 2, 3, 4, 0, 4, 0, 1, 2, 2, 4, 3, 4, 2, 3, 4, 2, 1, 0, 2, 0, 1, 2, 2, 3, 4, 2, 4 });
    }
}
