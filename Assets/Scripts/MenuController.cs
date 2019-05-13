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
    public GameObject credits;

    private Follow cameraScript;
    private int currentIndex = 0;
    private bool selected = false;
    private bool cameraArrived = false;
    private bool gameStart = false;
    private bool creditsStart = false;
    private static bool retry = false;
    private AudioSource[] soundEffects;

    void Start()
    {
        soundEffects = this.GetComponents<AudioSource>();
        cameraScript = mainCamera.GetComponent<Follow>();
        if(retry == true)
        {
            retry = false;
            HideMainMenu();
            OnGameStart();
        }
    }

    private void SetSelectedIndex(int index)
    {
        soundEffects[1].PlayDelayed(0.0f);
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

    public static void OnGameReset()
    {
        retry = true;
    }

    public void OnCredits()
    {
        this.creditsStart = true;
        this.credits.SetActive(true);
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

    private void HideMainMenu()
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    private void SoundFadeIn(AudioSource audio , float time)
    {
        if(audio.enabled == true && audio.isPlaying == false)
        {
            audio.Play();
        }
        if(audio.volume < 1.0f)
        {
            audio.volume += Time.deltaTime / (time + 1);
        }
    }

    private void SoundFadeOut(AudioSource audio , float time)
    {
        if (audio.volume > 0)
        {
            audio.volume -= Time.deltaTime / (time + 1);
        }
        if(audio.volume == 0)
        {
            audio.enabled = false;
        }
    }

    public void OnGameStart()
    {
        gameStart = true;
        player.constraints = RigidbodyConstraints.None;
        AI.constraints = RigidbodyConstraints.None;
        player.velocity = Vector3.forward * 3;
        AI.velocity = Vector3.forward;
        bars.SetActive(true);
    }

    public void EnableScripts()
    {
        enemyScript.enabled = true;
        playerScript.enabled = true;
        cameraScript.enabled = true;
        soundEffects[1].enabled = false;
        HideMainMenu();
    }

    public void ResetCredits()
    {
        this.creditsStart = false;
        this.credits.SetActive(false);
        this.credits.transform.position = new Vector3(this.credits.transform.position.x, -2000, this.credits.transform.position.z);
    }
    private void CheckInput()
    {
        if (creditsStart)
        {
            if (Input.GetAxis("Cancel") != 0)
            {
                ResetCredits();
            }
        }
        if(gameStart || creditsStart)
        {
            return;
        }
        ResetIndex(currentIndex);
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!selected)
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
                selected = true;
            }
        }
        else if (Input.GetAxis("Vertical") == 0)
        {
            selected = false;
        }
        if (Input.GetAxis("Submit") != 0)
        {
            SetClickedIndex(currentIndex);
        }
    }
    void Update()
    {
        if (!gameStart)
        {
            SoundFadeIn(soundEffects[0], 10.0f);
        }
        else
        {
            SoundFadeOut(soundEffects[0], 3.0f);
            SoundFadeIn(soundEffects[2], 5.0f);
        }
        SnapCamera();
        CheckInput();
    }

}
