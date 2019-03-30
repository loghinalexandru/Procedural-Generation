using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityObjectsGenerator : HMM
{
    //TODO: Add more training data
    void Start()
    {
        this.Init();
        this.estimator = new EM(this.stateStartProbabilities.Count, this.emissions.Count);
        this.ParameterInference(new List<int> {
            0, 7,9,7,1, 2, 4,8,0,2,3, 1, 2,7,9,7, 0, 1, 3, 2, 7,5,7, 1,2, 0, 2, 7, 5, 6, 7, 3,7,9,7, 2, 1,7, 4,8, 3, 7,5,6,7, 1,3,
            //2, 7, 6, 5, 7, 4, 3,0, 7,6,7, 1, 0, 2, 7,5,7, 4, 5, 7, 5, 7, 4, 3, 2, 1, 7,6,5,7,1, 2, 3, 4, 7, 5,
            //7 , 0,2,4,7,5,6,7,1,3,7,2,4,0,1,2,7,3,0,1,2,7,2,3,2,1,7,5,6,7,0,2,1,3,4,5,7,6,5,7,2,3,0,1,4,2,3,1,0,2,3,4,2,1,3,
            //7,5,7,4,2,1,3,4,7,0,1,2,3,4,7,5,7,3,4,2,1,0,2,3,4,0,1,1,4,0,7,5,6,7,4,3,2,4,2,1,0,2,4,5,7,1,0,2,3,4,7,5,7,2,3,1,
            //0,2,3,1,4,2,3,4,3,2,1,0,7,5,6,7,3,2,1,0,3,2,4,2,1,0,7,5,7,2,3,2,3,4,2,1,2,3,7,5,7,0,1,2,1,1,3,4,2,3,4,2,7,6,5,7,0,
            //1,2,3,4,0,1,2,3,4,7,5,7,4,3,2,4,2,3,1,0,7,5,7,0,1,2,3,4,4,3,2,1,0,1,7,5,6,7,3,2,4,1,0,2,3,1,0,2,3,7,5,7,0
        });
        //this.SaveEmissionProbabilities("/Probabilities/EM_cityPropsEmission.txt");
        //this.SaveTransitionProbabilities("/Probabilities/EM_cityPropsTransition.txt");
    }
}
