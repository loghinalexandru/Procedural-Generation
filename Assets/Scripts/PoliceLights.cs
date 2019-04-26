using System.Collections;
using UnityEngine;

public class PoliceLights : MonoBehaviour
{

    public Material red;
    public Material blue;
    public float intensity = 10.0f;

    void Start()
    {
        StartCoroutine(SwitchColors());
    }

    private IEnumerator SwitchColors()
    {
        for (; ; )
        {
            SetRed();
            yield return new WaitForSeconds(1.0f);
            SetBlue();
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void SetRed()
    {
        blue.SetColor("_EmissionColor", Color.blue);
        red.SetColor("_EmissionColor", Color.red * intensity);

    }

    private void SetBlue()
    {
        red.SetColor("_EmissionColor", Color.red);
        blue.SetColor("_EmissionColor", Color.blue * intensity);
    }

}
