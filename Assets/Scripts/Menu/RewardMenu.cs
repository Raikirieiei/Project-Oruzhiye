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

    public StatReward reward;
    public StatReward reward2;
    public StatReward reward3;


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
        Invoke(nameof(FindCharStat), 1);
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnGameStageChanged;
    }

    private void FindCharStat(){
        characterStats = GameObject.FindWithTag("Player").GetComponent<CharacterStats>();
    }

    private void GameManagerOnGameStageChanged(GameState state) {
        Debug.Log("rewardmenu");
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
        Image[] img = button.GetComponentsInChildren<Image>();
        img[1].sprite = reward.img;
    }

    // this function assign new reward from reward pools when scene changed.
    public void RewardAssign(Scene scene, LoadSceneMode mode){
        List<StatReward> instancePool = new List<StatReward>(rewardPools);

        reward = instancePool[Random.Range(0,instancePool.Count)];
        instancePool.Remove(instancePool.Find(x => x == reward)); 
        ChangeRewardText(button1, reward);

        reward2 = instancePool[Random.Range(0,instancePool.Count)];
        instancePool.Remove(instancePool.Find(x => x == reward2)); 
        ChangeRewardText(button2, reward2);

        reward3 = instancePool[Random.Range(0,instancePool.Count)];
        instancePool.Remove(instancePool.Find(x => x == reward3)); 
        ChangeRewardText(button3, reward3);
    }

    public void SelectOptionOne(){
        reward.Selected(characterStats);
        rewardPools.Remove(reward);
        GameManager.instance.UpdateGameState(GameState.AdjustStat);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SelectOptionTwo(){
        reward2.Selected(characterStats);
        rewardPools.Remove(reward2);
        GameManager.instance.UpdateGameState(GameState.AdjustStat);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SelectOptionThree(){
        reward3.Selected(characterStats);
        rewardPools.Remove(reward3);
        GameManager.instance.UpdateGameState(GameState.AdjustStat);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
