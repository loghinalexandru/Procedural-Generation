using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlatform : MonoBehaviour
{
    private Markov generator;
    private int rotation = 0;
    public int platformSize = 20;
    private int xOffset = 0;
    private int zOffset = 0;
    public GameObject currentPlatform;
    public float risingSpeed = 4f;
    public float fallingSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        this.generator = this.GetComponent<Markov>();
        this.zOffset = this.platformSize;

    }




    void Update()
    {
        this.RisePlatform();
    }

    private void SetOffset()
    {
        this.rotation = this.rotation % 360;
        int aux = Mathf.Abs(this.xOffset);
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

    public void InstantiatePlatform()
    {
        GameObject nextPlatform = Instantiate(this.generator.Next(), new Vector3(this.currentPlatform.transform.position.x + this.xOffset, -15, this.currentPlatform.transform.position.z + this.zOffset), Quaternion.Euler(0, rotation, 0));
        switch (nextPlatform.name)
        {
            case "Right(Clone)":
                this.rotation += 90;
                this.SetOffset();
                break;
            case "Left(Clone)":
                this.rotation -= 90;
                this.SetOffset();
                break;
            default:
                break;
        }
        DestroyCurrentPlatform();
        this.currentPlatform = nextPlatform;
    }
}
