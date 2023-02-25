using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardMenu : MonoBehaviour
{
    public GameObject rewardMenuUI;

    public List<StatReward> rewardPools;

    public static RewardMenu instance;
    private CharacterStats characterStats;
    private GameObject button1;
    private GameObject button2;
    private GameObject button3;

    // Start is called before the first frame update

    void Awake(){
        if (instance == null) {
            instance = this;
        } 
        else if (instance != this){
            Destroy (gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        GameManager.OnGameStateChanged += GameManagerOnGameStageChanged;
        button1 = GameObject.Find("Reward1");
        button2 = GameObject.Find("Reward2");
        button3 = GameObject.Find("Reward3");
    }

    void Start(){
        characterStats = GameObject.Find("Player 1").GetComponent<CharacterStats>();
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnGameStageChanged;
    }

    private void GameManagerOnGameStageChanged(GameState state) {
        rewardMenuUI.SetActive(state == GameState.RewardSelect);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += RewardAssign;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= RewardAssign;
    }

    private void ChangeRewardText(GameObject button, StatReward reward){
        Text[] textList;
        textList = button.GetComponentsInChildren<Text>();
        textList[0].text = reward.name;
        textList[1].text = reward.desc;
        // Debug.Log(reward.name + reward.desc);
    }

    // this function assign new reward from reward pools when scene changed.
    public void RewardAssign(Scene scene, LoadSceneMode mode){
        
        // make a copy of reward pool for randomize 3 distinct reward
        // TODO make a copy of reward pool
        List<StatReward> instancePool = new List<StatReward>(rewardPools);
        
        // random 3 of the reward
        // TODO after random a reward, pop that reward from the copy pool
        StatReward reward = instancePool[Random.Range(0,instancePool.Count)];
        instancePool.Remove(instancePool.Find(x => x == reward)); 
        ChangeRewardText(button1, reward);

        StatReward reward2 = instancePool[Random.Range(0,instancePool.Count)];
        instancePool.Remove(instancePool.Find(x => x == reward2)); 
        ChangeRewardText(button2, reward2);

        StatReward reward3 = instancePool[Random.Range(0,instancePool.Count)];
        instancePool.Remove(instancePool.Find(x => x == reward3)); 
        ChangeRewardText(button3, reward3);

        // assign onClick event to buttons
        button1.GetComponent<Button>().onClick.AddListener( delegate {
            reward.Selected(characterStats);
            rewardPools.Remove(reward);
            GameManager.instance.UpdateGameState(GameState.Normal);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        });

        button2.GetComponent<Button>().onClick.AddListener( delegate {
            reward2.Selected(characterStats);
            rewardPools.Remove(reward2);
            GameManager.instance.UpdateGameState(GameState.Normal);
            gameObject.SetActive(false);
        });

        button3.GetComponent<Button>().onClick.AddListener( delegate {
            reward3.Selected(characterStats);
            rewardPools.Remove(reward3);
            GameManager.instance.UpdateGameState(GameState.Normal);
            gameObject.SetActive(false);
        });
    }

}
