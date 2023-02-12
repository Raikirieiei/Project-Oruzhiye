using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;

    public GameObject pauseMenuUI;
    
    private GameObject playerSet;
    private GameObject gameManager;

    public static PauseMenu instance;

    void Awake(){
         if (instance == null) {
            instance = this;
        } 
        else if (instance != this){
            Destroy (gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    
    private void OnDestroy() {

    }

    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause) {
                Resume();
                Debug.Log("Resume");
            }
            else{
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
        playerSet = GameObject.FindWithTag("PlayerSet");
        gameManager = GameObject.Find("GameManager");
        Destroy(playerSet);
        Destroy(gameObject);
        Destroy(gameManager);
        SceneManager.LoadScene(0);
        
    }
}
