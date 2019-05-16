using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickPause : StateMachineBehaviour
{
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        switch (animator.name)
        {
            case "Resume":
                gameController.TogglePause();
                break;
            case "Exit To Menu":
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("MainScene");
                break;
            default:
                break;
        }
    }
}
