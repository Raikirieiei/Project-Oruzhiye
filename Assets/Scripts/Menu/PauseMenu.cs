using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;

    public GameObject pauseMenuUI;
    
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause) {
                Debug.Log("unpause");
                Resume();
                Debug.Log("Resume");
            }
            else{
                Debug.Log("pause");
                Pause();
                Debug.Log("Pause");
            }
        }
        
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isPause = true;
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
    }

    // need to fix player not include in scene *****
    public void MainMenu() {
        Time.timeScale = 1;
        player = GameObject.FindWithTag("Player");
        Destroy(player);
        SceneManager.LoadScene(0);
    }
}
