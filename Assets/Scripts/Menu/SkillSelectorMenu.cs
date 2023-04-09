using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillSelectorMenu : MonoBehaviour
{

    public GameObject skillSelectorMenuUI;

    private GameObject character;

    private AbilityHolder characterAbilityHolder;
    private AbilityHolder2 characterAbilityHolder2;

    public List<Ability> swordPools;
    public List<Ability> spearPools;
    public List<Ability> greatSwordPools;

    public static SkillSelectorMenu instance;

    public Button[] rewardButton;
    
    public Text[] rewardName;
    public Text[] rewardDesc;
    public Image[] rewardImg;

    public Ability reward;
    public Ability reward2;

    private void Awake() {
        Invoke(nameof(Find_player), 1);
        if (character == null) return;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        } 
        else if (instance != this){
            Destroy (gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Invoke(nameof(SkillAssign), (float)1.2);
        GameManager.OnGameStateChanged += GameManagerOnGameStageChanged;
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnGameStageChanged;
    }

    private void GameManagerOnGameStageChanged(GameState state) {
        skillSelectorMenuUI.SetActive(state == GameState.SkillSelect);
    }

    private void Find_player()
    {
        character = GameObject.FindWithTag("Player");
        characterAbilityHolder = character.GetComponent<AbilityHolder>();
        characterAbilityHolder2 = character.GetComponent<AbilityHolder2>();
    }

    private void ChangeRewardText(int button, Ability reward){
        rewardName[button].text = reward.name;
        rewardDesc[button].text = reward.desc;
        rewardImg[button].sprite = reward.icon;
    }

    // this function assign new reward from reward pools when scene changed.
    public void SkillAssign(){
        if(character.name == "Player 1"){
            Debug.Log("sword");
            List<Ability> instancePool = new List<Ability>(swordPools);
            reward = instancePool[0];
            ChangeRewardText(0, reward);

            reward2 = instancePool[1];
            ChangeRewardText(1, reward2);
        }
        else if (character.name == "Player 2"){
            Debug.Log("spear");
            List<Ability> instancePool = new List<Ability>(spearPools);
            reward = instancePool[0];
            ChangeRewardText(0, reward);

            reward2 = instancePool[1];
            ChangeRewardText(1, reward2);
        }
        else if (character.name == "Player 3"){
            Debug.Log("GS");
            List<Ability> instancePool = new List<Ability>(greatSwordPools);
            reward = instancePool[0];
            ChangeRewardText(0, reward);

            reward2 = instancePool[1];
            ChangeRewardText(1, reward2);
        }
    }

    public void SelectOptionOne(){
        
        characterAbilityHolder.ability = reward;
        rewardButton[0].interactable = false;
        rewardDesc[0].text = "Already Chosen This Upgrade!";
        rewardDesc[0].color = Color.red;
 
        GameManager.instance.UpdateGameState(GameState.AdjustSkill);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SelectOptionTwo(){
        characterAbilityHolder2.ability = reward2;
        rewardButton[1].interactable = false;
        rewardDesc[1].text = "Already Chosen This Upgrade!";
        rewardDesc[1].color = Color.red;

        GameManager.instance.UpdateGameState(GameState.AdjustSkill);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
