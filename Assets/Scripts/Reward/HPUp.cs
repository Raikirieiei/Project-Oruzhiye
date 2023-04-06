using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "RewardList/HPUp")]
public class HPUp : StatReward
{
    public override void Selected(CharacterStats characterStat){
        Debug.Log("Reward Selected: " + this.name);
        characterStat.baseHealth.addModifier(this.baseValue);
        characterStat.currentHealth += this.baseValue;
    }
}
