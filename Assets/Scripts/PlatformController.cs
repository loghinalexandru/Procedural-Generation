using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public int lowerBound = 20;
    public int upperBound = 60;
    public List<GameObject> transitionPlatforms;
    public float platformSize { get; set; }

    private HMM[] generators;
    private int currentGeneratorIndex = 0;
    private int currentEmissionCount = 0;
    private int maxEmissionCount;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        generators = new HMM[3];
        GameObject generator = GameObject.FindGameObjectsWithTag("Generator")[0];
        generators[0] = generator.GetComponent<CityPlatformGenerator>();
        generators[1] = generator.GetComponent<CountryPlatformGenerator>();
        generators[2] = generator.GetComponent<DesertPlatformGenerator>();
        platformSize = generators[0].emissions[0].transform.localScale.z;
        maxEmissionCount = Random.Range(lowerBound, upperBound);
    }

    public GameObject GetNextPlatform()
    {
        GameObject output;
        currentEmissionCount++;
        if (currentEmissionCount > maxEmissionCount)
        {
            output = transitionPlatforms[currentGeneratorIndex];
            currentEmissionCount = 0;
            currentGeneratorIndex = (currentGeneratorIndex + 1) % generators.Length;
            maxEmissionCount = Random.Range(lowerBound, upperBound);
        }
        else
        {
            output = generators[currentGeneratorIndex].NextEmission();
        }
        return output;
    }
}
