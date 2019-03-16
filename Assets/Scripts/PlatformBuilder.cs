using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBuilder : MonoBehaviour
{
    private PlatformGenerator platformGenerator;
    private CityObjectsGenerator cityObjectsGenerator;
    public GameObject currentPlatform;
    public float platformSize = 20;
    public int propsPerPlatform = 2;
    public float risingSpeed = 4f;
    public float fallingSpeed = 1f;
    private float xOffset = 0;
    private int rotation = 0;
    private float zOffset = 0;

    void Start()
    {
        this.platformGenerator = GameObject.FindGameObjectWithTag("Generator").GetComponent<PlatformGenerator>();
        this.cityObjectsGenerator = GameObject.FindGameObjectWithTag("Generator").GetComponent<CityObjectsGenerator>();
        this.zOffset = platformGenerator.emissions[0].transform.localScale.z;
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

    public GameObject BuildPlatform(GameObject state)
    {
        float nextObjectDistance = 0;
        GameObject nextPlatform = Instantiate(state, new Vector3(this.currentPlatform.transform.position.x + this.xOffset, -15, this.currentPlatform.transform.position.z + this.zOffset), Quaternion.Euler(0, 0, 0));
        Transform spawnPoints = nextPlatform.transform.Find("SpawnPoints");
        foreach (Transform child in spawnPoints)
        {
            GameObject prop = cityObjectsGenerator.NextEmission();
            float margin = prop.GetComponentInChildren<MeshRenderer>().bounds.extents.x;
            // Offsetting prop to be contained on the platform
            if (child.transform.eulerAngles.y == 90)
            {
                prop = Instantiate(prop, new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z - margin), child.transform.rotation);
            }
            if (child.transform.eulerAngles.y == 0)
            {

                prop = Instantiate(prop, new Vector3(child.transform.position.x + margin, child.transform.position.y, child.transform.position.z), child.transform.rotation);
            }
            if (child.transform.eulerAngles.y == 180)
            {
                prop = Instantiate(prop, new Vector3(child.transform.position.x - margin, child.transform.position.y, child.transform.position.z), child.transform.rotation);
            }
            prop.transform.SetParent(child);
        }
        //nextObjectDistance += props[i].transform.localScale.z + this.distanceBetweenProps;
        nextPlatform.transform.rotation = Quaternion.Euler(0, this.rotation, 0);
        return nextPlatform;
    }

    public void InstantiatePlatform()
    {
        GameObject nextPlatform = BuildPlatform(platformGenerator.NextEmission());
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
