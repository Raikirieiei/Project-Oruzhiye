using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;
    private bool canPause = true;

    public GameObject pauseMenuUI;
    
    private GameObject playerSet;
    private GameObject gameManager;
    private GameObject rewardManager;
    private GameObject skillSelector;

    public static PauseMenu instance;

    void Awake(){
         if (instance == null) {
            instance = this;
        } 
        else if (instance != this){
            Destroy (gameObject);
        }
        GameManager.OnGameStateChanged += GameManagerOnGameStageChanged;
        DontDestroyOnLoad(gameObject);
    }

    
    private void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnGameStageChanged;
    }

    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
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


    private void GameManagerOnGameStageChanged(GameState state) {
        if(state == GameState.RewardSelect || state == GameState.StatMenu || state == GameState.SkillSelect ){
            canPause = false;
        }else{
            canPause = true;
        }
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isPause = true;
        GameManager.instance.UpdateGameState(GameState.Pause);
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
        GameManager.instance.UpdateGameState(GameState.Normal);
    }

    // need to fix player not include in scene *****
    public void MainMenu() {
        Time.timeScale = 1;
        playerSet = GameObject.FindWithTag("PlayerSet");
        gameManager = GameObject.Find("GameManager");
        rewardManager = GameObject.Find("RewardSelector");
        skillSelector = GameObject.Find("SkillSelector");
        Destroy(playerSet);
        Destroy(gameObject);
        Destroy(gameManager);
        Destroy(rewardManager);
        Destroy(skillSelector);
        SceneManager.LoadScene(0);
        
    }
}
