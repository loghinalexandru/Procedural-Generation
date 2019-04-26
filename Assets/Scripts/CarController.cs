using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public WheelCollider frontRight, frontLeft, backRight, backLeft;
    public Transform frontRightMesh, frontLeftMesh, backRightMesh, backLeftMesh;
    public Renderer tailLights;
    public Material brakeLightsOn, brakeLightsOff, backLightsOn, backlightsOff, diskBrakes , carMaterial;
    public ReflectionProbe probe;
    public ParticleSystem backfireLeft, backfireRight;
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
    private int currentGear = -1;

    void Start()
    {
        tailLights = tailLights.GetComponent<Renderer>();
        body = GetComponent<Rigidbody>();
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
        frontRight.brakeTorque = 0f;
        frontLeft.brakeTorque = 0f;
        backRight.brakeTorque = 0f;
        backLeft.brakeTorque = 0f;
    }

    private void BackfirePlay()
    {
        backfireLeft.time = 0;
        backfireLeft.Play();
        backfireRight.time = 0;
        backfireRight.Play();
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
        frontRight.brakeTorque = brakeTorque;
        frontLeft.brakeTorque = brakeTorque;
        backRight.brakeTorque = brakeTorque;
        backLeft.brakeTorque = brakeTorque;
    }

    private void SetTailLights(string mode)
    {
        var lights = tailLights.materials;
        switch (mode)
        {
            case "brakesOn":
                lights[0] = brakeLightsOn;
                lights[1] = backlightsOff;
                discBrakesColor = discBrakesColor < 1 ? discBrakesColor + discBrakesStep : discBrakesColor;
                diskBrakes.SetColor("_EmissionColor", new Color(discBrakesColor, 0, 0) * 3);
                break;
            case "allOff":
                lights[0] = brakeLightsOff;
                lights[1] = backlightsOff;
                discBrakesColor = discBrakesColor > 0 ? discBrakesColor - discBrakesStep : discBrakesColor;
                break;
            case "reverseOn":
                lights[0] = brakeLightsOff;
                lights[1] = backLightsOn;
                discBrakesColor = discBrakesColor > 0 ? discBrakesColor - discBrakesStep : discBrakesColor;
                break;
        }
        diskBrakes.SetColor("_EmissionColor", new Color(discBrakesColor, 0, 0) * 3);
        tailLights.materials = lights;
    }

    private void Accelerate()
    {
        if (m_vertical > 0)
        {
            SetTailLights("allOff");
            SetBrakesOff();
            frontRight.motorTorque = motorTorque * m_vertical;
            frontLeft.motorTorque = motorTorque * m_vertical;
        }
        else if (m_vertical < 0 && direction > 0)
        {
            SetTailLights("brakesOn");
            SetBrakesOn();
        }
        else if (m_vertical < 0 && direction < 0)
        {
            SetTailLights("reverseOn");
            SetBrakesOff();
            frontRight.motorTorque = (motorTorque * 0.85f) * m_vertical;
            frontLeft.motorTorque = (motorTorque * 0.85f) * m_vertical;
        }
        else
        {
            SetTailLights("allOff");
            SetBrakesOff();
        }
    }

    private void UpdateWheel(WheelCollider colider, Transform mesh)
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

    public int GetGear()
    {
        int currentGear = -1;
        if (direction > 3)
            currentGear = 0;
        if (direction > 6)
            currentGear = 1;
        if (direction > 10)
            currentGear = 2;
        if (direction > 14)
            currentGear = 3;
        if (direction > 18)
            currentGear = 4;
        return currentGear;
    }

    private void SetGear()
    {
        float randomFactor = Random.Range(0, 1.0f);
        float threshold = 0.5f;
        int gear = GetGear();
        if (currentGear != gear)
        {
            currentGear = gear;
            if (randomFactor > threshold)
                BackfirePlay();
        }
    }

    private void SetReflection()
    {
        carMaterial.SetTexture("_Cube", this.probe.texture);
    }

    void FixedUpdate()
    {
        SetReflection();
        GetInput();
        Steer();
        SetGear();
        Accelerate();
        AnimateWheels();
    }
}
