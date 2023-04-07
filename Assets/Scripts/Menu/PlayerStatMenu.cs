using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatMenu : MonoBehaviour
{
    public GameObject characterStatUI;
    public Text[] StatAmount;
    public static bool isOpen = false;
    private bool canOpen = true;

    private CharacterStats characterStats;
    private GameObject statText;

    public static PlayerStatMenu instance;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        } 
        else if (instance != this){
            Destroy (gameObject);
        }
        GameManager.OnGameStateChanged += GameManagerOnGameStageChanged;
        characterStats = GameObject.FindWithTag("Player").GetComponent<CharacterStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateStat();
    }
        // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.J)) && canOpen){
            if (isOpen){
                CloseMenu();
            }else{
                OpenMenu();
            }
        }
        
    }

    private void GameManagerOnGameStageChanged(GameState state) {
        if(state == GameState.RewardSelect || state == GameState.Pause ){
            canOpen = false;
        }else{
            canOpen = true;
        }
    }

    private void UpdateStat(){
        foreach (var item in StatAmount)
        {
            switch (item.name)
            {
                case "HP":
                    item.text = characterStats.currentHealth.ToString();
                    break;
                case "Attack":
                    item.text = characterStats.baseAttack.getValue().ToString();
                    break;
                case "Defend":
                    item.text = characterStats.baseDefend.getValue().ToString();
                    break; 
                case "MoveSpeed":
                    item.text = characterStats.baseMoveSpeed.getValue().ToString();
                    break;
                case "MaxHp":
                    item.text = characterStats.baseHealth.getValue().ToString();
                    break;
                default:
                    Debug.Log("noth found stat amount");
                    break;
            }
        }
    }

    public void OpenMenu(){
        characterStatUI.SetActive(true);
        UpdateStat();
        Time.timeScale = 0;
        isOpen = true;
        GameManager.instance.UpdateGameState(GameState.StatMenu);
    }

    public void CloseMenu(){
        characterStatUI.SetActive(false);
        Time.timeScale = 1;
        isOpen = false;
        GameManager.instance.UpdateGameState(GameState.Normal);
    }
}
