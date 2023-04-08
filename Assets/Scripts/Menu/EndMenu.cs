using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

    private GameObject pauseMenu;
    private GameObject gameManager;
    private GameObject rewardManager;
    private GameObject skillSelector;

    private void DestroyOnLoadObject(){
        pauseMenu = GameObject.Find("PauseMenu");
        gameManager = GameObject.Find("GameManager");
        rewardManager = GameObject.Find("RewardSelector");
        skillSelector = GameObject.Find("SkillSelector");
        Destroy(pauseMenu);
        Destroy(gameManager);
        Destroy(rewardManager);
        Destroy(skillSelector);
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
