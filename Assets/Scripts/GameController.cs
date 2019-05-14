using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;
    public GameObject camera;
    public GameObject gameOverScreen;
    public Animator[] controllers;

    private Follow cameraScript;
    private EnemyAI enemyScript;
    private bool gameOver = false;
    private int currentIndex = 0;
    private bool pressed = false;
    private AudioSource inGameMusic;

    void Start()
    {
        gameOverScreen.SetActive(false);
        cameraScript = camera.GetComponent<Follow>();
        enemyScript = enemy.GetComponent<EnemyAI>();
        inGameMusic = GetComponent<AudioSource>();
    }
    private void FreezeRender()
    {
        Time.timeScale = 1.0f - Time.timeScale;
    }

    private void FreezeSound()
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

    void Update()
    {
        if (player.transform.position.y < 0)
        {
            cameraScript.enabled = false;
            enemyScript.enabled = false;
            gameOverScreen.SetActive(true);
            gameOver = true;
        }
        if (Input.GetAxisRaw("Cancel") != 0)
        {
            if (!pressed)
            {
                FreezeSound();
                FreezeRender();
                pressed = true;
            }
        }
        else
        {
            pressed = false;
        }
        CheckWastedScreen();
        SoundEffects.FadeIn(inGameMusic, 10.0f);
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
