using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;
    public GameObject gameOverScreen;
    public Animator[] controllers;

    private Follow cameraScript;
    private EnemyAI enemyScript;
    private bool gameOver = false;
    private bool pressed = false;
    private int currentIndex = 0;

    void Start()
    {
        gameOverScreen.SetActive(false);
        cameraScript = GetComponent<Follow>();
        enemyScript = enemy.GetComponent<EnemyAI>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < -2)
        {
            cameraScript.enabled = false;
            enemyScript.enabled = false;
            gameOverScreen.SetActive(true);
            gameOver = true;
        }
        CheckWastedScreen();
    }

    private void ResetIndex(int index)
    {
        controllers[index].SetBool("clicked", false);
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

    private void CheckWastedScreen()
    {
        if (gameOver)
        {
            ResetIndex(currentIndex);
            if (Input.GetAxis("Horizontal") > 0)
            {
                currentIndex = currentIndex < 1 ? currentIndex + 1 : currentIndex;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                currentIndex = currentIndex > 0 ? currentIndex - 1 : currentIndex;
            }
            SetSelectedIndex(currentIndex);
            if (Input.GetAxis("Submit") > 0)
                SetClickedIndex(currentIndex);
        }
    }


}
