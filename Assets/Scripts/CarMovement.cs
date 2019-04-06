using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    public WheelCollider frontRight, frontLeft , backRight , backLeft;
    public Transform frontRightMesh, frontLeftMesh, backRightMesh, backLeftMesh;
    public float torque = 30f;
    public float maxAngle = 45f;

    private float currentAngle = 0.0f;
    private float currentTorque = 0.0f;
    private float m_horizontal = 0.0f;
    private float m_vertical = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void GetInput()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        currentAngle = maxAngle * m_horizontal;
        frontLeft.steerAngle = currentAngle;
        frontRight.steerAngle = currentAngle;
    }

    private void Accelerate()
    {
        frontRight.motorTorque = torque * m_vertical;
        frontLeft.motorTorque = torque * m_vertical;
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
