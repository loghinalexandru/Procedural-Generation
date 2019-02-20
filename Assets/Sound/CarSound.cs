using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{

    public AudioSource startUp;
    public AudioSource idle;


    // Use this for initialization
    void Start()
    {
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        startUp.Play();
        yield return new WaitForSeconds(startUp.clip.length - 1);
        var player = GameObject.Find("Player");
        foreach (Transform child in player.transform)
        {
            if (child.gameObject.name == "LightsR" || child.gameObject.name == "LightsL")
            {
                child.gameObject.SetActive(true);
            }
        }

    }
}
