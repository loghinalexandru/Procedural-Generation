using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityObjectsGenerator : HMM
{
    void Start()
    {
        this.Init();
        this.SetEstimator(new EM(this.stateStartProbabilities.Count, this.emissions.Count));
        this.ParameterInference(new List<int> { 0, 1, 0, 1, 0, 1, 3, 2, 3, 0, 1, 0, 1, 0, 1, 0, 1, 3, 3, 3, 0, 1, 0, 1, 3, 1, 0, 1, 0, 3, 3, 0, 1, 0, 1, 0, 3 });
    }
}
