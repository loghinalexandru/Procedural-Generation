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
    public ParticleSystem backfireLeft;
    public ParticleSystem backfireRight;
    public float motorTorque = 30f;
    public float brakeTorque = 20f;
    public float maxAngle = 45f;

    private Rigidbody body;
    private float currentAngle = 0.0f;
    private float m_horizontal = 0.0f;
    private float m_vertical = 0.0f;
    private float direction;
    private float discBrakesColor = 0.0f;
    private float discBrakesStep = 0.01f;
    private bool isPlayable = false;
    private bool[] usedGears = { false, false, false, false , false};

    void Start()
    {
        tailLights = tailLights.GetComponent<Renderer>();
        body = this.GetComponent<Rigidbody>();
        BackfirePause();
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

    private void SetBrakesOff()
    {
        this.frontRight.brakeTorque = 0;
        this.frontLeft.brakeTorque = 0;
        this.backRight.brakeTorque = 0;
        this.backLeft.brakeTorque = 0;
    }

    private void BackfirePlay()
    {
        if (this.isPlayable)
        {
            backfireLeft.time = 0;
            backfireLeft.Play();
            backfireRight.time = 0;
            backfireRight.Play();
            this.isPlayable = false;
        }
    }

    private void BackfirePause()
    {
        backfireLeft.Pause();
        backfireRight.Pause();
    }

    private void SetBrakesOn()
    {
        frontRight.motorTorque = 0;
        frontLeft.motorTorque = 0;
        this.frontRight.brakeTorque = this.brakeTorque;
        this.frontLeft.brakeTorque = this.brakeTorque;
        this.backRight.brakeTorque = this.brakeTorque;
        this.backLeft.brakeTorque = this.brakeTorque;
    }

    private void SetTailLights(string mode)
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
            SetTailLights("allOff");
            SetBrakesOff();
            frontRight.motorTorque = this.motorTorque * m_vertical;
            frontLeft.motorTorque = this.motorTorque * m_vertical;
        }
        else if(m_vertical < 0 && direction > 0)
        {
            SetTailLights("brakesOn");
            SetBrakesOn();
            BackfirePlay();
        }
        else if(m_vertical < 0 && direction < 0)
        {
            SetTailLights("reverseOn");
            SetBrakesOff();
            frontRight.motorTorque = (this.motorTorque * 0.85f) * m_vertical;
            frontLeft.motorTorque = (this.motorTorque *  0.85f)* m_vertical;
        }
        else
        {
            SetTailLights("allOff");
            SetBrakesOff();
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

    private void SetGear()
    {
        float randomFactor = Random.Range(0, 1.0f);
        float threshold = 0.5f;
        if(this.direction > 3 && !usedGears[0])
        {
            if(randomFactor > threshold)
                this.isPlayable = true;
            usedGears[0] = true;
        }
        else if(this.direction < 3 && this.direction > 0)
        {
            if (usedGears[0] && randomFactor > threshold)
                this.isPlayable = true;
            usedGears[0] = false;
        }
        if (this.direction > 6 && !usedGears[1])
        {
            if (randomFactor > threshold)
                this.isPlayable = true;
            usedGears[1] = true;
        }
        else if (this.direction < 6)
        {
            if (usedGears[1] && randomFactor > threshold)
                this.isPlayable = true;
            usedGears[1] = false;
        }
        if (this.direction > 10  && !usedGears[2])
        {
            if (randomFactor > threshold)
                this.isPlayable = true;
            usedGears[2] = true;
        }
        else if (this.direction < 10)
        {
            if (usedGears[2] && randomFactor > threshold)
                this.isPlayable = true;
            usedGears[2] = false;
        }
        if (this.direction > 14 && !usedGears[3])
        {
            if (randomFactor > threshold)
                this.isPlayable = true;
            usedGears[3] = true;
        }
        else if (this.direction < 14)
        {
            if (usedGears[3] && randomFactor > threshold)
                this.isPlayable = true;
            usedGears[3] = false;
        }
        if(this.direction > 18 && !usedGears[4])
        {
            if (randomFactor > threshold)
                this.isPlayable = true;
            usedGears[4] = true;
        }
        else if(this.direction < 18)
        {
            if (usedGears[4] && randomFactor > threshold)
                this.isPlayable = true;
            usedGears[4] = false;
        }
        BackfirePlay();
    }

    void FixedUpdate()
    {
        GetInput();
        Steer();
        SetGear();
        Accelerate();
        AnimateWheels();
    }
}
