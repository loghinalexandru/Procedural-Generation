﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryPlatformGenerator : HMM
{
    // Start is called before the first frame update
    void Start()
    {
        this.Init();
        //this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        //this.ParameterInference(new List<int> {
        //    0,1,0,1,0,1,0,1,0,1,0,1,0,1,2,1,0,1,0,2
        //});

        //this.SaveEmissionProbabilities("/Resources/CountryPlatformEmission.txt");
        //this.SaveTransitionProbabilities("/Resources/CountryPlatformTransition.txt");
    }
}
