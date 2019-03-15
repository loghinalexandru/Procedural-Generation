using System.Collections;
using System.Collections.Generic;

public class PlatformGenerator : HMM
{
    void Start()
    {
        this.Init();
        this.SetEstimator(new EM(this.stateStartProbabilities.Count, this.emissions.Count));
        this.ParameterInference(new List<int> {
                0,1,2,1,0,2,1,0,2,2,1,2,1,2,5,5,4,3,4,5,5,5,4,3,4,5,3,5,3,5,8,8,8,8,6,8,7,8,6,7,8,6,8,8,2,1,2,1,2,2,2,0,1,0,2,1,2
                ,5,4,5,3,4,5,4,5,5,5,8,6,7,8,8,8,6,7,6,7,8,8,6,8,2,1,2,1,0,2,1,0,2,2,1,2,0,2,3,5,4,3,4,5,3,5 }
        );
    }
}
