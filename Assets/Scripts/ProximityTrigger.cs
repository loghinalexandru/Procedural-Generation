using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameController bustedUI;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Proximity");
    }

    public void OnTriggerExit(Collider other)
    {
        
    }
}
