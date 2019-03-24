using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityObjectsGenerator : HMM
{
    void Start()
    {
        this.Init();
        this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        this.ParameterInference(new List<int> { 0, 1, 2, 2, 3, 2, 1, 0, 2, 3, 2, 1, 0, 1, 2, 0, 3, 1, 0, 2, 7, 5, 7, 7, 1, 2, 0, 3, 2, 2, 1, 0, 2, 7, 5, 6, 7, 3, 2, 1, 3, 2, 0, 1, 2, 3,4, 0,4, 1, 1, 3,
            2, 7, 6, 5, 7, 4, 3, 2, 1, 1, 0, 2, 4, 3, 5, 7, 5, 7, 4, 3, 2, 1, 0, 0, 1, 2, 3, 4, 7, 5, 7 , 0,2,4 , 1,4, 2,3});
        //this.SaveEmissionProbabilities("/Probabilities/EM_cityPropsEmission.txt");
        //this.SaveTransitionProbabilities("/Probabilities/EM_cityPropsTransition.txt");
    }
}
