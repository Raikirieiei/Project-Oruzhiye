using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatMenu : MonoBehaviour
{
    public GameObject characterStatUI;
    public static bool isOpen = false;

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
        characterStats = GameObject.FindWithTag("Player").GetComponent<CharacterStats>();
        statText = characterStatUI.transform.Find("StatAmount").gameObject;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateStat();
    }

    private void UpdateStat(){
        Text[] textList = statText.GetComponentsInChildren<Text>(true);
        foreach (var item in textList)
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
                default:
                    Debug.Log("noth found stat amount");
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.J))){
            if (isOpen){
                CloseMenu();
            }else{
                OpenMenu();
            }
        }
        
    }

    private void OpenMenu(){
        characterStatUI.SetActive(true);
        UpdateStat();
        Time.timeScale = 0;
        isOpen = true;
    }

    private void CloseMenu(){
        characterStatUI.SetActive(false);
        Time.timeScale = 1;
        isOpen = false;
    }
}
