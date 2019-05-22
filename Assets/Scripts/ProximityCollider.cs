using UnityEngine;
using UnityEngine.UI;

public class ProximityTrigger : MonoBehaviour
{
    public float cooldownTime;
    public int bustTime = 3;
    public GameObject bustedUI;
    public Text timerUI;
    public GameController controller;

    private float currentCoolDown = 0;
    private int timer;
    private bool inRange = false;

    private void Start()
    {
        timer = bustTime;
        currentCoolDown = 0;
    }

    private void updateTimer()
    {
        if (timer - 1 >= 0)
        {
            Debug.Log(timer);
            timerUI.text = timer.ToString();
            timer -= 1;
            Invoke("updateTimer", 1.0f);
        }
        else if(inRange)
        {
            bustedUI.SetActive(false);
            controller.SetGameOverState();
            this.enabled = false;
        }
    }

    void Update()
    {
        if (currentCoolDown - Time.deltaTime > 0.0f)
        {
            currentCoolDown -= Time.deltaTime;
        }
        else
        {
            currentCoolDown = 0.0f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "AI" && currentCoolDown == 0.0f)
        {
            timer = bustTime;
            inRange = true;
            bustedUI.SetActive(true);
            updateTimer();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "AI")
        {
            bustedUI.SetActive(false);
            currentCoolDown = cooldownTime;
            inRange = false;
        }
    }
}
