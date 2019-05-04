using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Animator[] controllers;
    public EnemyAI enemyScript;
    public CarController playerScript;
    public Camera mainCamera;
    public Rigidbody player;
    public Rigidbody AI;
    public Transform cameraPosition;
    public GameObject bars;

    private Follow cameraScript;
    private int currentIndex = 0;
    private bool pressed = false;
    private bool cameraArrived = false;
    private bool gameStart = false;

    void Start()
    {
        cameraScript = mainCamera.GetComponent<Follow>();
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
        if (mainCamera.transform.position == cameraPosition.position)
        {
            cameraArrived = true;
            EnableScripts();
            return;
        }
        if (cameraArrived == false && gameStart == true)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, cameraPosition.position, 0.2f);
            mainCamera.transform.rotation = Quaternion.RotateTowards(mainCamera.transform.rotation, cameraPosition.rotation, 0.4f);
        }
    }

    private void ResetIndex(int index)
    {
        controllers[index].SetBool("clicked", false);
    }

    public void OnGameStart()
    {
        gameStart = true;
        player.constraints = RigidbodyConstraints.None;
        AI.constraints = RigidbodyConstraints.None;
        player.velocity = Vector3.forward;
        AI.velocity = Vector3.forward;
        bars.SetActive(true);
    }

    public void EnableScripts()
    {
        enemyScript.enabled = true;
        playerScript.enabled = true;
        cameraScript.enabled = true;
        gameObject.SetActive(false);
    }
    private void CheckInput()
    {
        ResetIndex(currentIndex);
        if (Input.GetAxis("Vertical") != 0)
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
        else if (Input.GetAxis("Vertical") == 0)
        {
            pressed = false;
        }
        if (Input.GetAxis("Submit") != 0)
        {
            SetClickedIndex(currentIndex);
        }
    }
    void Update()
    {
        CheckInput();
        SnapCamera();
    }

}
