using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public float speed = 2f;
    public float rotationSpeed = 20f;
    public GameObject rightFrontWheel;
    public GameObject leftFrontWheel;
    public GameObject rightBackWheel;
    public GameObject leftBackWheel;
    public float maxSteeringAngle = 45.0f;
    public float maxVelocity = 20.0f;
    public float wheelsRPM = 100.0f;
    private float currentAngle = 0.0f;
    private Rigidbody body;
    private float steeringAngle = 0.0f;

    void Start()
    {
        body = player.GetComponent<Rigidbody>();
        body.maxAngularVelocity = 2.0f;
    }

    private void WheelSpinAnimation()
    {
        float velocity = Mathf.Abs(body.velocity.x + body.velocity.z);
        this.rightFrontWheel.transform.Rotate(velocity * wheelsRPM * Time.deltaTime, 0.0f, 0.0f);
        this.leftFrontWheel.transform.Rotate(velocity * wheelsRPM * Time.deltaTime, 0.0f, 0.0f);
        this.rightBackWheel.transform.Rotate(velocity * wheelsRPM * Time.deltaTime, 0.0f, 0.0f);
        this.leftBackWheel.transform.Rotate(velocity * wheelsRPM * Time.deltaTime, 0.0f, 0.0f);
    }

    private void WheelRotation()
    {
        Transform rightFrontParent = rightFrontWheel.transform.parent.transform;
        Transform leftFrontParent = leftFrontWheel.transform.parent.transform;

        if (Mathf.Abs(this.currentAngle + this.steeringAngle * Time.deltaTime * this.rotationSpeed) < this.maxSteeringAngle)
        {
            this.currentAngle += this.steeringAngle * Time.deltaTime * this.rotationSpeed;
            rightFrontParent.RotateAround(rightFrontWheel.transform.position, Vector3.up, this.steeringAngle * Time.deltaTime * this.rotationSpeed);
            leftFrontParent.RotateAround(leftFrontWheel.transform.position, Vector3.up, this.steeringAngle * Time.deltaTime * this.rotationSpeed);
        }
    }

    void Update()
    {
        this.WheelSpinAnimation();
        this.WheelRotation();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddRelativeTorque(Vector3.up * rotationSpeed);
            this.steeringAngle = this.maxSteeringAngle;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddRelativeTorque(Vector3.down * rotationSpeed);
            this.steeringAngle = -this.maxSteeringAngle;
        }
        if (player.transform.position.y > 0 && body.velocity.magnitude < maxVelocity)
            body.AddRelativeForce(Vector3.back * speed);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Box")
        {
            collision.gameObject.transform.parent = null;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 2);
            Destroy(collision.gameObject, 2);
        }
    }
}
