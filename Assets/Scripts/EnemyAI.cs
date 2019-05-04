using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public GameObject follow;
    public float maxSpeed = 18.0f;
    public float minSpeed = 5.0f;
    public float accelerationRate = 0.1f;
    public float brakingRate = 0.3f;
    public Material tailLights;

    private float baseSpeed = 3.0f;

    private void SetBrakesOn()
    {
        tailLights.SetColor("_EmissionColor", Color.red * 12f);
    }

    private void SetBrakesOff()
    {
        tailLights.SetColor("_EmissionColor", Color.red * 6.5f);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, follow.transform.position) > 8.0f)
        {
            if (baseSpeed + accelerationRate < maxSpeed)
            {
                SetBrakesOff();
                baseSpeed += accelerationRate;
            }
        }
        if (Vector3.Distance(transform.position, follow.transform.position) < 8.0f)
        {
            if (baseSpeed - brakingRate > minSpeed)
            {
                SetBrakesOn();
                baseSpeed -= brakingRate;
            }
        }
        Vector3 dir = follow.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.position, dir, 10f, 0.0f));
        transform.position = Vector3.MoveTowards(transform.position, follow.transform.position, baseSpeed * Time.deltaTime);
    }
}
