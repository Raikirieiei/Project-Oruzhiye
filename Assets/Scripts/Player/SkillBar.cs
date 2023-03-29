using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    public Image[] SkillIcon;
    public Text[] CDText;
    public Image[] CDimage;
    public Player player;
    string skillCDx;
    string skillCDy;

    // Start is called before the first frame update
    void Start()
    {
        SkillIcon[0].sprite = player.GetComponent<AbilityHolder>().ability.icon;
        SkillIcon[1].sprite = player.GetComponent<AbilityHolder2>().ability.icon;
    }

    // Update is called once per frame
    void Update()
    {
        SkillCooldown();
    }

    public void SkillCooldown(){
        string CDx = player.GetComponent<AbilityHolder>().getCooldownTime().ToString();
        string CDc = player.GetComponent<AbilityHolder2>().getCooldownTime().ToString();

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
}
