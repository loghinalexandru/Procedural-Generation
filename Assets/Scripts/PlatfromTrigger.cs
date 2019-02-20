using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromTrigger : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        PlatformGeneration.generate = true;
    }
}
