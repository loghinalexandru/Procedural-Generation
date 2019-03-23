using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityObjectsGenerator : HMM
{
    void Start()
    {
        this.Init();
        this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        this.ParameterInference(new List<int> { 0, 1, 4, 1, 0, 4, 1, 0, 1, 0, 1, 0, 0, 1, 1, 0, 4, 2, 3, 4, 1,1 , 0, 1, 1, 0, 1, 0, 1, 4, 2, 3, 4, 0, 1, 0, 1, 1, 0, 1, 1, 0, 4, 0, 1, 0, 4, 2, 4, 0, 1,
            4, 1, 0, 4, 1,0,1,0, 4, 2, 4, 1, 0, 1,1,0,1,1,0,0, 4, 2, 4 , 0,1,0,1,1,0,1,4,2,3,4,1,1,0,1,0,4,2,4,2,4,0,1,0,1,0,1,0,4,0,0,1,0,1,0,1,0,4,2,4,0,1,0,0,1,0,4,3,2,4,1,0,1,0,1,1,0,1,4,1,0,1,4,2,4,0,1,0,4,3,2,4,0,1,0,0,1,0,4,2,3,4,0,1,0,1,0,0,4,2,3,4,0,1,4,2,4,0,1,1,0,1,0,4,2,4,0,1,4,2,3,4,1,0,1,1,0,0,1,0,4,3,2,4,0,1});
        //this.SaveEmissionProbabilities("/Probabilities/EM_cityPropsEmission.txt");
        //this.SaveTransitionProbabilities("/Probabilities/EM_cityPropsTransition.txt");
    }
}
