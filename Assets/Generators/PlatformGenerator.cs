﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : HMM
{
    void Start()
    {
        this.Init();
        this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        this.ParameterInference(new List<int> {
            0,1,2,1,0,2,1,0,2,2,1,2,1,2,4,5,4,3,4,5,5,4,3,4,5,3,5,3,5,7,6,7,8,7,8,6,7,8,6,8,6,7,8,7,8,6,8,7,6,8,6,8,7,8,0,1,2,1,2,2,0,1,0,2,1,2
            ,3,4,5,3,4,5,3,4,3,5,4,5,5,7,6,7,8,6,7,6,7,8,6,8,1,0,2,1,0,2,1,0,2,2,1,2,0,2,3,5,4,3,4,5,5,4,3,4,3,5,5,4,3,4,3,5,4,5,3,5,7,6,7,8,7,6,
            8,7,8,6,7,8,6,8,6,7,8,6,7,8,6,7,6,7,8,1,2,2,0,1,0,1,2,0,1,0,2,0,2,1,0,2,4,3,5,3,5,4,5,4,3,4,3,5,3,4,5,4,3,5,3,5,5,4,3,5,4,5,3,5,4,5,
            6,8,8,7,6,7,8,6,8,7,6,8,6,7,8,6,7,8,6,7,6,7,6,8,7,8,6,8,7,6,8,7,6,8,7,8,8,6,7,8,6,7,8,6,7,8,7,8,6,8,7,8,6,7,8,7,6,7,6,7,8,6,7,8,6,8,
            1,2,0,1,2,0,1,2,0,1,2,0,1,2,2,1,0,2,1,2,0,1,0,1,0,1,2,0,2,0,1,2,0,2,1,0,1,2,2,0,1,2,0,1,0,2,0,1,0,1,0,1,0,2,0,2,1,0,2,2,1,0,1,0,2,0,1
        }
        );
        foreach (var entry in this.NextEmission(3000)) {
            Debug.Log(entry);
        }
        //this.SaveEmissionProbabilities("/Probabilities/EM_emission.txt");
        //this.SaveTransitionProbabilities("/Probabilities/EM_transition.txt");
    }
}
