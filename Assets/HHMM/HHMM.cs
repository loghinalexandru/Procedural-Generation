using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HHMM : MonoBehaviour
{
    [SerializeField]
    private StateNode root;

    [SerializeField]
    private Node currentState;
}
