using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlatform : MonoBehaviour
{

    private Markov generator;

    // Start is called before the first frame update
    void Start()
    {
        this.generator = new Markov();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Use the generator for the platforms like before
        //TODO: Use emissions to populate platforms with props
        Debug.Log(generator.Next());
    }
}
