using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

    private GameObject pauseMenu;
    private GameObject gameManager;

    public void Retry(){
        pauseMenu = GameObject.Find("PauseMenu");
        gameManager = GameObject.Find("GameManager");
        Destroy(pauseMenu);
        Destroy(gameManager);
        SceneManager.LoadScene(1);
    }
 
    public void ReturnMainMenu(){
        pauseMenu = GameObject.Find("PauseMenu");
        gameManager = GameObject.Find("GameManager");
        Destroy(pauseMenu);
        Destroy(gameManager);
        SceneManager.LoadScene(0);
    }
}
