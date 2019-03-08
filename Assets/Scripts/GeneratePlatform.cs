using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GeneratePlatform : MonoBehaviour
{
    private HMM generator;
    public GameObject currentPlatform;
    public float platformSize = 20;
    public int propsPerPlatform = 2;
    public float risingSpeed = 4f;
    public float fallingSpeed = 1f;
    public float centerDistance = 10f;
    public float distanceBetweenProps = 2f;
    private float xOffset = 0;
    private int rotation = 0;
    private float zOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<HMM>();
        this.zOffset = generator.emissions[0].transform.localScale.z;
        this.platformSize = this.zOffset;
    }

    void Update()
    {
        this.RisePlatform();
    }

    private void SetOffset()
    {
        this.rotation = this.rotation % 360;
        float aux = Mathf.Abs(this.xOffset);
        this.xOffset = Mathf.Abs(this.zOffset);
        this.zOffset = aux;
        if (this.rotation == -90 || this.rotation == 180 || this.rotation == -180 || this.rotation == 270)
        {
            this.xOffset = -this.xOffset;
            this.zOffset = -this.zOffset;
        }
    }

    private void RisePlatform()
    {
        if (this.currentPlatform.transform.position.y < 0)
        {
            Vector3 lerpValue;
            lerpValue = Vector3.Lerp(this.currentPlatform.transform.position, this.currentPlatform.transform.position + Vector3.up * risingSpeed, 1f * Time.deltaTime);
            if (lerpValue.y > 0)
            {
                this.currentPlatform.transform.position = new Vector3(this.currentPlatform.transform.position.x, 0, this.currentPlatform.transform.position.z);
            }
            else
            {
                this.currentPlatform.transform.position = lerpValue;
            }
        }
    }

    private void DestroyCurrentPlatform()
    {
        Destroy(this.currentPlatform, 10);
    }

    private float SideGeneration(string orientation)
    {
        switch (orientation)
        {
            case "Right(Clone)":
                return -this.centerDistance;
            case "Left(Clone)":
                return this.centerDistance;
            default:
                if (Random.Range(-1, 1) < 0)
                {
                    return -this.centerDistance;
                }
                else
                {
                    return this.centerDistance;
                }
        }
    }

    public GameObject BuildPlatform(GameObject state)
    {
        float nextObjectDistance = 0;
        //List<GameObject> props = generator.GenerateEmissions(state, this.propsPerPlatform);
        GameObject nextPlatform = Instantiate(state, new Vector3(this.currentPlatform.transform.position.x + this.xOffset, -15, this.currentPlatform.transform.position.z + this.zOffset), Quaternion.Euler(0, 0, 0));
        //for (int i = 0; i < props.Count; ++i)
        //{
        //    float side = this.SideGeneration(nextPlatform.name);
        //    int propRotation = 0;
        //    if (side < 0)
        //        propRotation = 180;
        //    props[i] = Instantiate(props[i], new Vector3(nextPlatform.transform.position.x + side, nextPlatform.transform.position.y, nextPlatform.transform.position.z - this.platformSize / 2 + 2 * nextObjectDistance), Quaternion.Euler(0, propRotation, 0));
        //    nextObjectDistance += props[i].transform.localScale.z + this.distanceBetweenProps;
        //    props[i].transform.SetParent(nextPlatform.transform);
        //}
        nextPlatform.transform.rotation = Quaternion.Euler(0, this.rotation, 0);
        return nextPlatform;
    }

    public void InstantiatePlatform()
    {
        GameObject nextPlatform = BuildPlatform(generator.NextEmission());
        if (nextPlatform.name.Contains("Left"))
        {
            this.rotation -= 90;
            this.SetOffset();
        }
        if (nextPlatform.name.Contains("Right"))
        {
            this.rotation += 90;
            this.SetOffset();
        }
        DestroyCurrentPlatform();
        this.currentPlatform = nextPlatform;
    }
}
