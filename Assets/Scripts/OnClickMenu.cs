﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickMenu : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MenuController controller = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuController>();
        switch (animator.name)
        {
            case "Play":
                controller.OnGameStart();
                break;
            case "Credits":
                controller.OnCredits();
                break;
            case "Exit":
                controller.OnGameExit();
                break;
            default:
                break;
        }
    }

}
