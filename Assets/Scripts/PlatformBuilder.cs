using UnityEngine;
//TODO: REFACTOR THIS ASAP
public class PlatformBuilder : MonoBehaviour
{
    public GameObject currentPlatform;
    public float risingSpeed = 4f;
    public float destroyDelay = 10.0f;
    private PlatformController controller;
    private HMM[] propGenerators;
    private float xOffset = 0;
    private int rotation = 0;
    private float zOffset = 0;

    void Start()
    {
        propGenerators = new HMM[3];
        controller = GetComponent<PlatformController>();
        propGenerators[0] = GameObject.FindGameObjectWithTag("Generator").GetComponent<CityObjectsGenerator>();
        propGenerators[1] = GameObject.FindGameObjectWithTag("Generator").GetComponent<CountryObjectsGenerator>();
        propGenerators[2] = GameObject.FindGameObjectWithTag("Generator").GetComponent<DesertObjectsGenerator>();
        zOffset = controller.platformSize;
    }

    void Update()
    {
        RisePlatform();
    }

    private void SetOffset()
    {
        rotation = rotation % 360;
        float aux = Mathf.Abs(xOffset);
        xOffset = Mathf.Abs(zOffset);
        zOffset = aux;
        if (rotation == -90 || rotation == 180 || rotation == -180 || rotation == 270)
        {
            xOffset = -xOffset;
            zOffset = -zOffset;
        }
    }

    private void RisePlatform()
    {
        if (currentPlatform.transform.position.y < 0)
        {
            Vector3 lerpValue;
            lerpValue = Vector3.Lerp(currentPlatform.transform.position, currentPlatform.transform.position + Vector3.up * risingSpeed, 1f * Time.deltaTime);
            if (lerpValue.y > 0)
            {
                currentPlatform.transform.position = new Vector3(currentPlatform.transform.position.x, 0, currentPlatform.transform.position.z);
            }
            else
            {
                currentPlatform.transform.position = lerpValue;
            }
        }
    }
    //TODO: Call destroy after a buffer of platforms spawned instead of time passed
    private void DestroyCurrentPlatform()
    {
        Destroy(currentPlatform, destroyDelay);
    }

    private HMM ChoosePropGenerator(string sectionName)
    {
        if (sectionName.Contains("Desert"))
            return propGenerators[2];
        if (sectionName.Contains("Country"))
            return propGenerators[1];
        return propGenerators[0];
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
        GameObject nextPlatform = Instantiate(state, new Vector3(currentPlatform.transform.position.x + xOffset, -15, currentPlatform.transform.position.z + zOffset), Quaternion.Euler(0, 0, 0));
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
        nextPlatform.transform.rotation = Quaternion.Euler(0, rotation, 0);
        return nextPlatform;
    }

    public void InstantiatePlatform()
    {
        GameObject nextPlatform = BuildPlatform(controller.GetNextPlatform());
        if (nextPlatform.name.Contains("Left"))
        {
            rotation -= 90;
            SetOffset();
        }
        if (nextPlatform.name.Contains("Right"))
        {
            rotation += 90;
            SetOffset();
        }
        DestroyCurrentPlatform();
        currentPlatform = nextPlatform;
    }
}
