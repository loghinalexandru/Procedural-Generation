using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Animator[] controllers;
    public EnemyAI enemyScript;
    public CarController playerScript;
    public Camera camera;
    public Rigidbody player;
    public Rigidbody AI;
    public Transform cameraPosition;

    private Follow cameraScript;
    private int currentIndex = 0;
    private bool pressed = false;

    void Start()
    {
        cameraScript = camera.GetComponent<Follow>();
    }
    private void SetSelectedIndex(int index)
    {
        for (int i = 0; i < controllers.Length; ++i)
        {
            if (i == index)
                controllers[i].SetBool("selected", true);
            else
                controllers[i].SetBool("selected", false);
        }
    }
    private void SetClickedIndex(int index)
    {
        controllers[index].SetBool("clicked", true);
    }

    public void OnGameExit()
    {
        Application.Quit();
    }

    public void SnapCamera()
    {
        camera.transform.position = cameraPosition.position;
        camera.transform.rotation = cameraPosition.rotation;
    }

    private void ResetIndex(int index)
    {
        controllers[index].SetBool("clicked", false);
    }

    public void OnGameStart()
    {
        SnapCamera();
        this.player.constraints = RigidbodyConstraints.None;
        this.AI.constraints = RigidbodyConstraints.None;
        this.enemyScript.enabled = true;
        this.playerScript.enabled = true;
        this.cameraScript.enabled = true;
        this.enabled = false;
    }
    private void CheckInput()
    {
        ResetIndex(this.currentIndex);
        if(Input.GetAxis("Vertical") != 0)
        {
            if (!pressed)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    currentIndex = (currentIndex + 1) % controllers.Length;
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    currentIndex = currentIndex > 0 ? currentIndex - 1 : controllers.Length - 1;
                }
                SetSelectedIndex(currentIndex);
                pressed = true;
            }
        }
        else if(Input.GetAxis("Vertical") == 0)
        {
            pressed = false;
        }
        if(Input.GetAxis("Submit") != 0)
        {
            SetClickedIndex(this.currentIndex);
        }
    }
    void Update()
    {
        CheckInput();
    }

}
