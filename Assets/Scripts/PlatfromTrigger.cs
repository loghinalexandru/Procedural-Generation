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

    void OnTriggerEnter(Collider collidingObject)
    {

        if (this.name == "NextPlatformTrigger")
        {
            if (!this.triggered)
            {
                this.callbackObject.GetComponent<PlatformBuilder>().InstantiatePlatform();
                this.triggered = true;
            }
        }
        else
        {
            collidingObject.GetComponent<SkidMarks>().StopEmission();
            collidingObject.GetComponent<SkidMarks>().enabled = false;
            Rigidbody rb = collidingObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass *= 10;
        }
    }
}
