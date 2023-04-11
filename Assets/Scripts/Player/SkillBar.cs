using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    public Image[] SkillIcon;
    public Text[] CDText;
    public Image[] CDimage;
    public Image[] ATimage;
    public Text[] ActiveTimeText;
    public Player player;
    string skillCDx;
    string skillCDy;

    // Start is called before the first frame update
    void Start()
    {
        SkillIcon[0].sprite = player.GetComponent<AbilityHolder>().ability.icon;
        SkillIcon[1].sprite = player.GetComponent<AbilityHolder2>().ability.icon;
        GameManager.OnGameStateChanged += GameManagerOnGameStageChanged;
    }

    // Update is called once per frame
    void Update()
    {
        SkillCooldown();
        SkillActive();
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= GameManagerOnGameStageChanged;
    }

    private void GameManagerOnGameStageChanged(GameState state) {
        if(state == GameState.AdjustSkill){
            SkillIcon[0].sprite = player.GetComponent<AbilityHolder>().ability.icon;
            SkillIcon[1].sprite = player.GetComponent<AbilityHolder2>().ability.icon;
            GameManager.instance.UpdateGameState(GameState.Normal);
        }
    }
       

    public void SkillCooldown(){
        string CDx = player.GetComponent<AbilityHolder>().getCooldownTime().ToString("F1");
        string CDc = player.GetComponent<AbilityHolder2>().getCooldownTime().ToString("F1");

        CDText[0].text = CDx;
        CDText[1].text = CDc;

        if(float.Parse(CDx) <= 0){
            CDimage[0].enabled = false;
            CDText[0].enabled = false;
        }else{
            CDimage[0].enabled = true;
            CDText[0].enabled = true;
        }

        if(float.Parse(CDc) <= 0){
            CDimage[1].enabled = false;
            CDText[1].enabled = false;
        }else{
            CDimage[1].enabled = true;
            CDText[1].enabled = true;
        }
    }

    public void SkillActive(){
        string ATx = player.GetComponent<AbilityHolder>().getActiveTime().ToString("F1");
        string ATc = player.GetComponent<AbilityHolder2>().getActiveTime().ToString("F1");

        ActiveTimeText[0].text = ATx;
        ActiveTimeText[1].text = ATc;


        if(float.Parse(ATx) <= 0){
            ATimage[0].enabled = false;
            ActiveTimeText[0].enabled = false;
        }else{
            ATimage[0].enabled = true;
            ActiveTimeText[0].enabled = true;
        }

        if(float.Parse(ATc) <= 0){
            ATimage[1].enabled = false;
            ActiveTimeText[1].enabled = false;
        }else{
            ATimage[1].enabled = true;
            ActiveTimeText[1].enabled = true;
        }
    }
}
