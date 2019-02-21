using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public List<GameObject> straightRoad;
    public List<GameObject> rightTurnRoad;
    public List<GameObject> leftTurnRoad;
    private List<List<GameObject>> _prefabPlatforms = new List<List<GameObject>>();
    public List<GameObject> prefabObstacles;
    public float instantionSpeed = 2f;
    public GameObject player;
    public float risingSpeed = 4f;
    public float fallingSpeed = 1f;
    private GameObject _currentPlatform;
    private GameObject _previousPlatform;
    private float _directionX;
    private float _directionZ;
    public static float angle;
    public static bool generate = false;
    public float _time = 0;
    public static float platformSize;

    void Start()
    {
        _prefabPlatforms.Add(straightRoad);
        _prefabPlatforms.Add(leftTurnRoad);
        _prefabPlatforms.Add(rightTurnRoad);
        _currentPlatform = GameObject.Find("Terrain");
        _previousPlatform = _currentPlatform;
        platformSize = _currentPlatform.transform.localScale.z;
        _directionX = 0;
        _directionZ = platformSize;
        angle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            generate = false;
            GeneratePlatform();

            if (_previousPlatform.name != "Terrain")
            {
                Destroy(_previousPlatform, 20);
            }
            _previousPlatform = _currentPlatform;

        }

        if (_currentPlatform.transform.position.y < 0)
        {
            Vector3 lerpValue;
            lerpValue = Vector3.Lerp(_currentPlatform.transform.position, _currentPlatform.transform.position + Vector3.up * risingSpeed, 1f * Time.deltaTime);
            if (lerpValue.y > 0)
            {
                _currentPlatform.transform.position = new Vector3(_currentPlatform.transform.position.x, 0, _currentPlatform.transform.position.z);
            }
            else
            {
                _currentPlatform.transform.position = lerpValue;
            }
        }

    }

    private void GeneratePlatform()
    {
        int index = 0;

        if (angle == 0)
        {
            index = Random.Range(0, _prefabPlatforms.Count);
            switch (index)
            {
                case 0:
                    _currentPlatform = Instantiate(_prefabPlatforms[index][Random.Range(0, _prefabPlatforms[index].Count)], new Vector3(_currentPlatform.transform.position.x + _directionX, -15, _currentPlatform.transform.position.z + _directionZ), Quaternion.Euler(0, angle, 0));
                    return;
                case 1:
                    _currentPlatform = Instantiate(_prefabPlatforms[index][Random.Range(0, _prefabPlatforms[index].Count)], new Vector3(_currentPlatform.transform.position.x + _directionX, -15, _currentPlatform.transform.position.z + _directionZ), Quaternion.Euler(0, angle, 0));
                    angle = -90;
                    _directionX = -platformSize;
                    _directionZ = 0;
                    return;

                case 2:
                    _currentPlatform = Instantiate(_prefabPlatforms[index][Random.Range(0, _prefabPlatforms[index].Count)], new Vector3(_currentPlatform.transform.position.x + _directionX, -15, _currentPlatform.transform.position.z + _directionZ), Quaternion.Euler(0, angle, 0));
                    angle = 90;
                    _directionX = platformSize;
                    _directionZ = 0;
                    return;
            }
        }
        if (angle == 90)
        {
            index = Random.Range(0, _prefabPlatforms.Count - 1);
            switch (index)
            {
                case 0:
                    _currentPlatform = Instantiate(_prefabPlatforms[index][Random.Range(0, _prefabPlatforms[index].Count)], new Vector3(_currentPlatform.transform.position.x + _directionX, -15, _currentPlatform.transform.position.z + _directionZ), Quaternion.Euler(0, angle, 0));
                    return;

                case 1:
                    _currentPlatform = Instantiate(_prefabPlatforms[index][Random.Range(0, _prefabPlatforms[index].Count)], new Vector3(_currentPlatform.transform.position.x + _directionX, -15, _currentPlatform.transform.position.z + _directionZ), Quaternion.Euler(0, angle, 0));
                    angle = 0;
                    _directionX = 0;
                    _directionZ = platformSize;
                    return;
            }
        }
        if (angle == -90)
        {
            index = Random.Range(0, _prefabPlatforms.Count - 1);
            switch (index)
            {
                case 0:
                    _currentPlatform = Instantiate(_prefabPlatforms[index][Random.Range(0, _prefabPlatforms[index].Count)], new Vector3(_currentPlatform.transform.position.x + _directionX, -15, _currentPlatform.transform.position.z + _directionZ), Quaternion.Euler(0, angle, 0));
                    return;

                case 1:
                    index++;
                    _currentPlatform = Instantiate(_prefabPlatforms[index][Random.Range(0, _prefabPlatforms[index].Count)], new Vector3(_currentPlatform.transform.position.x + _directionX, -15, _currentPlatform.transform.position.z + _directionZ), Quaternion.Euler(0, angle, 0));
                    _directionX = 0;
                    _directionZ = platformSize;
                    angle = 0;
                    return;
            }
        }

    }
}


