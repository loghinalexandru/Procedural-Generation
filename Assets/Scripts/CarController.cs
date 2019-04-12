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
    public Material diskBrakes;
    public float motorTorque = 30f;
    public float brakeTorque = 20f;
    public float maxAngle = 45f;

    private Rigidbody body;
    private float currentAngle = 0.0f;
    private float currentTorque = 0.0f;
    private float m_horizontal = 0.0f;
    private float m_vertical = 0.0f;
    private float direction;
    private float discBrakesColor = 0.0f;
    private float discBrakesStep = 0.01f;

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

    private void setBrakesOff()
    {
        this.frontRight.brakeTorque = 0;
        this.frontLeft.brakeTorque = 0;
        this.backRight.brakeTorque = 0;
        this.backLeft.brakeTorque = 0;
    }

    private void setBrakesOn()
    {
        frontRight.motorTorque = 0;
        frontLeft.motorTorque = 0;
        this.frontRight.brakeTorque = this.brakeTorque;
        this.frontLeft.brakeTorque = this.brakeTorque;
        this.backRight.brakeTorque = this.brakeTorque;
        this.backLeft.brakeTorque = this.brakeTorque;
    }

    private void setTailLights(string mode)
    {
        var lights = tailLights.materials;
        switch (mode)
        {
            case "brakesOn":
                lights[0] = brakeLightsOn;
                lights[1] = backlightsOff;
                discBrakesColor = discBrakesColor < 1 ? discBrakesColor + this.discBrakesStep : discBrakesColor;
                this.diskBrakes.SetColor("_EmissionColor", new Color(this.discBrakesColor , 0, 0) * 3);
                break;
            case "allOff":
                lights[0] = brakeLightsOff;
                lights[1] = backlightsOff;
                discBrakesColor = discBrakesColor > 0 ? discBrakesColor - this.discBrakesStep : discBrakesColor;
                break;
            case "reverseOn":
                lights[0] = brakeLightsOff;
                lights[1] = backLightsOn;
                discBrakesColor = discBrakesColor > 0 ? discBrakesColor - this.discBrakesStep : discBrakesColor;
                break;
        }
        this.diskBrakes.SetColor("_EmissionColor", new Color(this.discBrakesColor, 0, 0) * 3);
        this.tailLights.materials = lights;
    }

    private void Accelerate()
    {
        if(m_vertical > 0)
        {
            setTailLights("allOff");
            setBrakesOff();
            frontRight.motorTorque = this.motorTorque * m_vertical;
            frontLeft.motorTorque = this.motorTorque * m_vertical;
        }
        else if(m_vertical < 0 && direction > 0)
        {
            setTailLights("brakesOn");
            setBrakesOn();
        }
        else if(m_vertical < 0 && direction < 0)
        {
            setTailLights("reverseOn");
            setBrakesOff();
            frontRight.motorTorque = (this.motorTorque * 0.75f) * m_vertical;
            frontLeft.motorTorque = (this.motorTorque *  0.75f)* m_vertical;
        }
        else
        {
            setTailLights("allOff");
            setBrakesOff();
        }
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

    void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        AnimateWheels();
    }
}
