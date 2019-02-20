using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{

    public GameObject player;
    public float speed = 2f;
    public float rotationSpeed = 20f;
    public float directionSpeed = 2f;
    public GameObject _rightWheel;
    public GameObject _leftWheel;
    private Quaternion rotationMin = Quaternion.Euler(new Vector3(0f, -45f, 0));
    private Quaternion rotationMax = Quaternion.Euler(new Vector3(0f, 45f, 0));
    private Rigidbody _body;

    // Use this for initialization
    void Start()
    {
        _body = player.GetComponent<Rigidbody>();
        // _body.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {

            _body.AddRelativeTorque(Vector3.up * rotationSpeed);
            if (_rightWheel.transform.localEulerAngles.y < 45 || _rightWheel.transform.localEulerAngles.y > 180)
            {

                _rightWheel.transform.localEulerAngles = new Vector3(_rightWheel.transform.localEulerAngles.x, _rightWheel.transform.localEulerAngles.y + rotationSpeed / 2, _rightWheel.transform.localEulerAngles.z);
                _leftWheel.transform.localEulerAngles = new Vector3(_leftWheel.transform.localEulerAngles.x, _leftWheel.transform.localEulerAngles.y + rotationSpeed / 2, _leftWheel.transform.localEulerAngles.z);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {

            _body.AddRelativeTorque(Vector3.down * rotationSpeed);
            if (_rightWheel.transform.localEulerAngles.y > 315 || _rightWheel.transform.localEulerAngles.y < 180)
            {
                _rightWheel.transform.localEulerAngles = new Vector3(_rightWheel.transform.localEulerAngles.x, _rightWheel.transform.localEulerAngles.y - rotationSpeed / 2, _rightWheel.transform.localEulerAngles.z);
                _leftWheel.transform.localEulerAngles = new Vector3(_leftWheel.transform.localEulerAngles.x, _leftWheel.transform.localEulerAngles.y - rotationSpeed / 2, _leftWheel.transform.localEulerAngles.z);

            }

        }
        _body.AddRelativeForce(Vector3.back * speed);
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
