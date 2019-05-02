using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;
    public GameObject gameOverScreen;

    private Follow cameraScript;
    private EnemyAI enemyScript;
    private bool gameOver = false;

    void Start()
    {
        gameOverScreen.SetActive(false);
        cameraScript = this.GetComponent<Follow>();
        enemyScript = enemy.GetComponent<EnemyAI>();
    }
    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < -2)
        {
            cameraScript.enabled = false;
            enemyScript.enabled = false;
            gameOverScreen.SetActive(true);
            gameOver = true;
        }
        if(gameOver && Input.anyKey)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
