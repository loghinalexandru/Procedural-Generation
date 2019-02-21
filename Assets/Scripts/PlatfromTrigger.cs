using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromTrigger : MonoBehaviour
{

    private GameObject callbackObject;
    private bool triggered = false;

    void Start()
    {
        callbackObject = GameObject.Find("Camera");
    }

    void OnTriggerEnter(Collider col)
    {
        if (!triggered)
        {
            this.callbackObject.GetComponent<GeneratePlatform>().InstantiatePlatform();
            this.triggered = true;
        }

    }
}
