using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSettings : MonoBehaviour
{
    public bool isStackable = false;
    public int maxObjectStack = 1;
    public float spawnOffset { get; set; } = 0.0f;
}
