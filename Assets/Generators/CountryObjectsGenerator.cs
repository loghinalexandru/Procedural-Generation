using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryObjectsGenerator : HMM
{
    void Start()
    {
        this.Init();
        //this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        //this.ParameterInference(new List<int> { 0, 1, 2, 3, 1, 2, 3, 6, 1, 0, 1, 7, 0, 5, 0, 1, 2, 3, 0, 1, 2, 3, 0, 3, 2, 0, 6, 4, 3, 0, 1, 3, 2, 2, 6, 0, 2, 7, 3, 1, 5, 1, 0, 0, 0, 1, 2, 4, 7, 6, 5, 2, 0, 1, 2, 3, 0, 3, 1, 3, 0, 1, 0, 1, 6, 2, 1, 2, 3, 2, 3, 1, 0, 1, 3 });
        //this.SaveEmissionProbabilities("/Resources/CountryPropsEmission.txt");
        //this.SaveTransitionProbabilities("/Resources/CountryPropsTransition.txt");
    }

}
