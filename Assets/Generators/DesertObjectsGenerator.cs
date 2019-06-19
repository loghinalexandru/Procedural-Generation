using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertObjectsGenerator : HMM
{
    void Start()
    {
        this.Init();
        //this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        //this.ParameterInference(new List<int> { 0, 1, 2, 4, 1, 2, 1, 4, 1, 4,5, 2, 1, 3, 0, 1, 4, 3, 4, 2, 1, 2, 3, 4, 5, 4, 0, 1, 0, 1, 2, 2, 0, 1, 4, 2, 3, 4, 2, 1, 0, 2, 0, 1, 2, 0, 2});
        //this.SaveEmissionProbabilities("/Resources/DesertPropsEmission.txt");
        //this.SaveTransitionProbabilities("/Resources/DesertPropsTransition.txt");
    }
}
