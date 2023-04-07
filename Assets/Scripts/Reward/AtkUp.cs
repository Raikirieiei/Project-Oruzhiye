using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "RewardList/AtkUp")]
public class AtkUp : StatReward
{
    public override void Selected(CharacterStats characterStat){
        Debug.Log("Reward Selected: " + this.name);
        characterStat.baseAttack.addModifier(this.baseValue);
    }
}
