using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardMenu : MonoBehaviour
{
    public GameObject rewardMenuUI;

    public static RewardMenu instance;

    // Start is called before the first frame update

    void Awake(){
        if (instance == null) {
            instance = this;
        } 
        else if (instance != this){
            Destroy (gameObject);
        }
        GameManager.OnGameStateChanged += GameManagerOnGameStageChanged;
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnGameStageChanged;
    }

    private void GameManagerOnGameStageChanged(GameState state) {
        rewardMenuUI.SetActive(state == GameState.RewardSelect);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
