using UnityEngine;
using UnityEngine.UI;

public class ProximityTrigger : MonoBehaviour
{
    public int bustTime = 5;
    public GameObject bustedUI;
    public Text timerUI;
    public GameController controller;

    private int timer;
    private bool inRange = false;

    private void updateTimer()
    {
        if (timer - 1 >= 0 && inRange)
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "AI")
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
            CancelInvoke();
            bustedUI.SetActive(false);
            inRange = false;
        }
    }
}
