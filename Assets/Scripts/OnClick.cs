using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : StateMachineBehaviour
{

    public void OnRetry()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (animator.name)
        {
            case "Yes":
                OnRetry();
                break;
            case "No":
                OnExit();
                break;
            default:
                break;
        }
    }
}


