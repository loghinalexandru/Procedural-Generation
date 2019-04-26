using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public GameObject follow;
    public float maxSpeed = 18.0f;
    public float minSpeed = 5.0f;
    public float accelerationRate = 0.1f;
    public float brakingRate = 0.3f;

    private float baseSpeed = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, follow.transform.position) > 8.0f)
        {
            if (baseSpeed + accelerationRate < maxSpeed)
                baseSpeed +=  this.accelerationRate;
        }
        if (Vector3.Distance(transform.position, follow.transform.position) < 8.0f)
        {
            if (baseSpeed - brakingRate > minSpeed)
                baseSpeed -= brakingRate;
        }
        Vector3 dir = follow.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.position, dir, 10f, 0.0f));
        transform.position = Vector3.MoveTowards(transform.position, follow.transform.position, baseSpeed * Time.deltaTime);
    }
}
