using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

    private GameObject pauseMenu;
    private GameObject gameManager;
    private GameObject rewardManager;

    private void DestroyOnLoadObject(){
        pauseMenu = GameObject.Find("PauseMenu");
        gameManager = GameObject.Find("GameManager");
        rewardManager = GameObject.Find("RewardSelector");
        Destroy(pauseMenu);
        Destroy(gameManager);
        Destroy(rewardManager);
    }

    public void Retry(){
        DestroyOnLoadObject();
        SceneManager.LoadScene(1);
    }
 
    public void ReturnMainMenu(){
        DestroyOnLoadObject();
        SceneManager.LoadScene(0);
    }
}
