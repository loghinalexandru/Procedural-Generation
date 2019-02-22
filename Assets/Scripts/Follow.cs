using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject following;
    public float xOffset = 35.3f;
    public float yOffset = 0f;
    public float zOffset = -35.3f;
    public float updateTime = 1.1f;

    // Update is called once per frame

    void Update()
    {
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, new Vector3(following.transform.position.x + this.xOffset, this.gameObject.transform.position.y, following.transform.position.z + this.zOffset), this.updateTime * Time.deltaTime);
    }
}
