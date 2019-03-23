using UnityEngine;

public class PlatformBuilder : MonoBehaviour
{
    public GameObject currentPlatform;
    public float platformSize = 20;
    public int propsPerPlatform = 2;
    public float risingSpeed = 4f;
    public float fallingSpeed = 1f;
    public float destroyDelay = 10.0f;
    private PlatformGenerator platformGenerator;
    private CityObjectsGenerator cityObjectsGenerator;
    private CountryObjectsGenerator countryObjectsGenerator;
    private DesertObjectsGenerator desertObjectsGenerator;
    private float xOffset = 0;
    private int rotation = 0;
    private float zOffset = 0;

    void Start()
    {
        this.platformGenerator = GameObject.FindGameObjectWithTag("Generator").GetComponent<PlatformGenerator>();
        this.cityObjectsGenerator = GameObject.FindGameObjectWithTag("Generator").GetComponent<CityObjectsGenerator>();
        this.countryObjectsGenerator = GameObject.FindGameObjectWithTag("Generator").GetComponent<CountryObjectsGenerator>();
        this.desertObjectsGenerator = GameObject.FindGameObjectWithTag("Generator").GetComponent<DesertObjectsGenerator>();
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
    //TODO: Call destroy after a buffer of platforms spawned instead of time passed
    private void DestroyCurrentPlatform()
    {
        Destroy(this.currentPlatform, this.destroyDelay);
    }

    private HMM ChoosePropGenerator(string sectionName)
    {
        if (sectionName.Contains("Desert"))
        {
            return this.desertObjectsGenerator;
        }
        if (sectionName.Contains("Country"))
        {
            return this.countryObjectsGenerator;
        }
        return cityObjectsGenerator;
    }

    private void InstantiateProp(HMM generator, Transform spawnPosition, SpawnSettings spawn, int objectCount)
    {
        GameObject prop = generator.NextEmission();
        MeshRenderer renderer = prop.GetComponentInChildren<MeshRenderer>();
        float margin = renderer.bounds.extents.x;
        // Offsetting prop to be contained on the platform and not spawn on top of eachother
        if (objectCount != 0)
            spawn.spawnOffset += renderer.bounds.extents.z;
        //Disable spawning but take space into account
        if (prop.name == "Epsilon")
        {
            spawn.spawnOffset += renderer.bounds.extents.z;
            return;
        }
        //Clockwise spawning from eulerAngle of 0 deg
        switch (spawnPosition.transform.eulerAngles.y)
        {
            case 90:
                prop = Instantiate(prop, new Vector3(spawnPosition.transform.position.x + spawn.spawnOffset, spawnPosition.transform.position.y, spawnPosition.transform.position.z - margin), spawnPosition.transform.rotation);
                break;
            case 0:
                prop = Instantiate(prop, new Vector3(spawnPosition.transform.position.x + margin, spawnPosition.transform.position.y, spawnPosition.transform.position.z + spawn.spawnOffset), spawnPosition.transform.rotation);
                break;
            case 180:
                prop = Instantiate(prop, new Vector3(spawnPosition.transform.position.x - margin, spawnPosition.transform.position.y, spawnPosition.transform.position.z - spawn.spawnOffset), spawnPosition.transform.rotation);
                break;
            default:
                return;
        }
        spawn.spawnOffset += renderer.bounds.extents.z;
        prop.transform.SetParent(spawnPosition);
    }

    //TODO: Refactor this function
    //TODO: Make a transition platform with road sign info panel
    public GameObject BuildPlatform(GameObject state)
    {
        GameObject nextPlatform = Instantiate(state, new Vector3(this.currentPlatform.transform.position.x + this.xOffset, -15, this.currentPlatform.transform.position.z + this.zOffset), Quaternion.Euler(0, 0, 0));
        HMM propGenerator = ChoosePropGenerator(nextPlatform.name);
        Transform spawnPoints = nextPlatform.transform.Find("SpawnPoints");
        foreach (Transform spawn in spawnPoints)
        {
            SpawnSettings spawnSettings = spawn.GetComponent<SpawnSettings>();
            if (spawnSettings.isStackable == false && spawnSettings.maxObjectStack > 1)
                throw new System.Exception("Spawn is not stackable!");
            for (int i = 0; i < spawnSettings.maxObjectStack; ++i)
            {
                InstantiateProp(propGenerator, spawn, spawnSettings, i);
            }
        }
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
