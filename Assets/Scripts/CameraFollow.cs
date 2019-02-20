using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject following;

    // Update is called once per frame

    void Update()
    {
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, new Vector3(following.transform.position.x + 35.3f, this.gameObject.transform.position.y, following.transform.position.z + -35.3f), 1.1f * Time.deltaTime);
    }
}
