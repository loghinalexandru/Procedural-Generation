using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject camera;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject bustedScreen;
    public GameObject gameUI;
    public Animator[] wastedScreenControllers;
    public Animator[] pauseScreenControllers;
    public bool gamePaused = false;

    private Follow cameraScript;
    private EnemyAI enemyScript;
    private bool gameOver = false;
    private Vector3 initialPosition;
    private int currentIndex = 0;
    private bool interfaceOverlap = false;
    private bool pressed = false;
    private AudioSource[] gameSounds;

    void Start()
    {
        gameOverScreen.SetActive(false);
        gameUI.SetActive(true);
        cameraScript = camera.GetComponent<Follow>();
        enemyScript = enemy.GetComponent<EnemyAI>();
        gameSounds = GetComponents<AudioSource>();
        this.initialPosition = player.transform.position;
    }

    private void ToggleRender()
    {
        Time.timeScale = 1.0f - Time.timeScale;
    }

    private void ToggleSound()
    {
        if (this.gameSounds[0].isPlaying)
        {
            this.gameSounds[0].Pause();
        }
        else
        {
            this.gameSounds[0].UnPause();
        }
    }

    //Get traveled distance in km
    private double GetDistanceTraveled()
    {
        return Vector3.Distance(initialPosition, player.transform.position) / 1000;
    }

    private void UpdateGameUI()
    {
        if (!gameOver)
        {
            if (gamePaused)
            {
                gameUI.GetComponentsInChildren<UnityEngine.UI.Text>()[0].text = "\u25B6";
            }
            else
            {
                gameUI.GetComponentsInChildren<UnityEngine.UI.Text>()[0].text = "| |";
                gameUI.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text = GetDistanceTraveled().ToString("0.##") + " Km";
            }
        }
    }

    private void TogglePauseMenu()
    {

        if (!this.pauseScreen.activeSelf)
        {
            interfaceOverlap = this.bustedScreen.activeSelf;
            this.bustedScreen.SetActive(false);
            this.pauseScreen.SetActive(true);
        }
        else
        {
            this.bustedScreen.SetActive(interfaceOverlap);
            this.pauseScreen.SetActive(false);
            interfaceOverlap = false;
        }
    }

    public void SetPausedState()
    {
        gamePaused = gamePaused == true ? false : true;
    }

    public void SetGameOverState()
    {
        player.GetComponent<CarController>().enabled = false;
        cameraScript.enabled = false;
        enemyScript.enabled = false;
        gameOverScreen.SetActive(true);
        gameOver = true;
    }

    public void TogglePause()
    {
        ToggleSound();
        ToggleRender();
        TogglePauseMenu();
        SetPausedState();
    }

    public void CheckPauseButton()
    {
        if (Input.GetAxisRaw("Cancel") != 0 && !gameOver && !gamePaused)
        {
            if (!pressed)
            {
                TogglePause();
                pressed = true;
            }
        }
        else
        {
            pressed = false;
        }
    }

    private void CheckGameOverCondition()
    {
        if (player.transform.position.y < 0)
        {
            SetGameOverState();
        }
    }

    void Update()
    {
        CheckGameOverCondition();
        CheckWastedScreen();
        CheckPauseButton();
        CheckPauseScreen();
        UpdateGameUI();
        SoundEffects.FadeIn(gameSounds[0], 10.0f);
    }

    private void ResetIndex(Animator[] controllers , int index)
    {
        controllers[index].SetBool("clicked", false);
    }

    private void SetSelectedIndex(Animator[] controllers , int index)
    {
        if (!controllers[index].GetBool("selected"))
        {
            gameSounds[1].PlayDelayed(0.0f);
            for (int i = 0; i < controllers.Length; ++i)
            {
                if (i == index)
                    controllers[i].SetBool("selected", true);
                else
                    controllers[i].SetBool("selected", false);
            }
        }
    }

    private void SetClickedIndex(Animator[] controllers , int index)
    {
        controllers[index].SetBool("clicked", true);
    }

    private void CheckWastedScreen()
    {
        if (gameOver)
        {
            ResetIndex(this.wastedScreenControllers , currentIndex);
            if (Input.GetAxis("Horizontal") > 0)
            {
                currentIndex = currentIndex < 1 ? currentIndex + 1 : currentIndex;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                currentIndex = currentIndex > 0 ? currentIndex - 1 : currentIndex;
            }
            SetSelectedIndex(this.wastedScreenControllers , currentIndex);
            if (Input.GetAxis("Submit") > 0)
                SetClickedIndex(this.wastedScreenControllers , currentIndex);
        }
    }

    private void CheckPauseScreen()
    {
        if (gamePaused)
        {
            ResetIndex(this.pauseScreenControllers, this.currentIndex);
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    currentIndex = currentIndex < 1 ? currentIndex + 1 : currentIndex;
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    currentIndex = currentIndex > 0 ? currentIndex - 1 : currentIndex;
                }
                SetSelectedIndex(this.pauseScreenControllers, currentIndex);
            }
            if (Input.GetAxisRaw("Submit") > 0)
            {
                SetClickedIndex(this.pauseScreenControllers, currentIndex);
            }
        }
    }
}
