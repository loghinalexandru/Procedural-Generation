using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject camera;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public Animator[] wastedScreenControllers;
    public Animator[] pauseScreenControllers;

    private Follow cameraScript;
    private EnemyAI enemyScript;
    private bool gameOver = false;
    private int currentIndex = 0;
    private bool pressed = false;
    private bool gamePaused = false;
    private AudioSource inGameMusic;

    void Start()
    {
        gameOverScreen.SetActive(false);
        cameraScript = camera.GetComponent<Follow>();
        enemyScript = enemy.GetComponent<EnemyAI>();
        inGameMusic = GetComponent<AudioSource>();
    }

    private void ToggleRender()
    {
        Time.timeScale = 1.0f - Time.timeScale;
    }

    private void ToggleSound()
    {
        if (this.inGameMusic.isPlaying)
        {
            this.inGameMusic.Pause();
        }
        else
        {
            this.inGameMusic.UnPause();
        }
    }

    private void TogglePauseMenu()
    {
        if (!this.pauseScreen.activeSelf)
        {
            this.pauseScreen.SetActive(true);
        }
        else
        {
            this.pauseScreen.SetActive(false);
        }
    }

    private void SetPausedState()
    {
        gamePaused = gamePaused == true ? false : true;
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
        if (Input.GetAxisRaw("Cancel") != 0)
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

    void Update()
    {
        if (player.transform.position.y < 0)
        {
            cameraScript.enabled = false;
            enemyScript.enabled = false;
            gameOverScreen.SetActive(true);
            gameOver = true;
        }
        CheckWastedScreen();
        CheckPauseButton();
        CheckPauseScreen();
        SoundEffects.FadeIn(inGameMusic, 10.0f);
    }

    private void ResetIndex(Animator[] controllers , int index)
    {
        controllers[index].SetBool("clicked", false);
    }

    private void SetSelectedIndex(Animator[] controllers , int index)
    {
        for (int i = 0; i < controllers.Length; ++i)
        {
            if (i == index)
                controllers[i].SetBool("selected", true);
            else
                controllers[i].SetBool("selected", false);
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
            ResetIndex(this.pauseScreenControllers , this.currentIndex);
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                currentIndex = currentIndex < 1 ? currentIndex + 1 : currentIndex;
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                currentIndex = currentIndex > 0 ? currentIndex - 1 : currentIndex;
            }
            SetSelectedIndex(this.pauseScreenControllers , currentIndex);
            if (Input.GetAxisRaw("Submit") > 0)
                SetClickedIndex(this.pauseScreenControllers , currentIndex);
        }
    }
}
