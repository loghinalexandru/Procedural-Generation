using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidMarks : MonoBehaviour
{
    public GameObject player;
    public TrailRenderer skidMarkR;
    public TrailRenderer skidMarkL;
    public float minSkew = 30.0f;

    // Update is called once per frame
    void Update()
    {
        if (player.transform.eulerAngles.y % 90 >= minSkew && player.transform.eulerAngles.y % 90 <= 90 - minSkew)
        {
            skidMarkL.emitting = true;
            skidMarkR.emitting = true;
        }
        else
        {
            skidMarkL.emitting = false;
            skidMarkR.emitting = false;
        }
    }
}
