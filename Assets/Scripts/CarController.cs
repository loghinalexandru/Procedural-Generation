using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{

    public WheelCollider frontRight, frontLeft , backRight , backLeft;
    public Transform frontRightMesh, frontLeftMesh, backRightMesh, backLeftMesh;
    public Renderer tailLights;
    public Material brakeLightsOn;
    public Material brakeLightsOff;
    public Material backLightsOn;
    public Material backlightsOff;
    public float motorTorque = 30f;
    public float brakeTorque = 20f;
    public float maxAngle = 45f;

    private float currentAngle = 0.0f;
    private float currentTorque = 0.0f;
    private float m_horizontal = 0.0f;
    private float m_vertical = 0.0f;
    private Rigidbody body;
    private float direction;


    // Start is called before the first frame update
    void Start()
    {
        tailLights = tailLights.GetComponent<Renderer>();
        body = this.GetComponent<Rigidbody>();
    }

    private void GetInput()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");
        direction = transform.InverseTransformDirection(body.velocity).z;
    }

    private void Steer()
    {
        currentAngle = maxAngle * m_horizontal;
        frontLeft.steerAngle = currentAngle;
        frontRight.steerAngle = currentAngle;
    }

    private void Accelerate()
    {
        var lights = tailLights.materials;
        if(m_vertical > 0)
        {
            lights[0] = brakeLightsOff;
            lights[1] = backlightsOff;
            frontRight.brakeTorque = 0;
            frontLeft.brakeTorque = 0;
            backRight.brakeTorque = 0;
            backLeft.brakeTorque = 0;
            frontRight.motorTorque = this.motorTorque * m_vertical;
            frontLeft.motorTorque = this.motorTorque * m_vertical;
        }
        else if(m_vertical < 0 && direction > 0)
        {
            lights[0] = brakeLightsOn;
            lights[1] = backlightsOff;
            frontRight.motorTorque = 0;
            frontLeft.motorTorque = 0;
            frontRight.brakeTorque = this.brakeTorque;
            frontLeft.brakeTorque = this.brakeTorque;
            backRight.brakeTorque = this.brakeTorque;
            backLeft.brakeTorque = this.brakeTorque;
        }
        else if(m_vertical < 0 && direction < 0)
        {
            lights[0] = brakeLightsOff;
            lights[1] = backLightsOn;
            frontRight.brakeTorque = 0;
            frontLeft.brakeTorque = 0;
            backRight.brakeTorque = 0;
            backLeft.brakeTorque = 0;
            frontRight.motorTorque = (this.motorTorque / 2) * m_vertical;
            frontLeft.motorTorque = (this.motorTorque / 2)* m_vertical;
        }
        else
        {
            lights[0] = brakeLightsOff;
            lights[1] = backlightsOff;
            frontRight.brakeTorque = 0;
            frontLeft.brakeTorque = 0;
            backRight.brakeTorque = 0;
            backLeft.brakeTorque = 0;
        }
        tailLights.materials = lights;
    }

    private void UpdateWheel(WheelCollider colider , Transform mesh)
    {
        Vector3 position = mesh.position;
        Quaternion rotation = mesh.rotation;
        colider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }


    private void AnimateWheels()
    {
        UpdateWheel(frontRight, frontRightMesh);
        UpdateWheel(frontLeft, frontLeftMesh);
        UpdateWheel(backRight, backRightMesh);
        UpdateWheel(backLeft, backLeftMesh);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        AnimateWheels();
    }
}
