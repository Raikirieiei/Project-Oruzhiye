using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "RewardList/DefUp")]
public class DefUp : StatReward
{
    public override void Selected(CharacterStats characterStat){
        Debug.Log("Reward Selected: " + this.name);
        characterStat.baseDefend.addModifier(this.baseValue);
    }
}
