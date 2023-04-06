using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "RewardList/MoveUp")]
public class MoveUp : StatReward
{
    public override void Selected(CharacterStats characterStat){
        Debug.Log("Reward Selected: " + this.name);
        characterStat.baseMoveSpeed.addModifier(this.baseValue);
    }
}
