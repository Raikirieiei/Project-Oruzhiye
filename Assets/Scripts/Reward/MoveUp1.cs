using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "RewardList/MoveUp1")]
public class MoveUp1 : StatReward
{
    public override void Selected(CharacterStats characterStat){
        Debug.Log("Reward Selected: " + this.name);
        characterStat.baseMoveSpeed.addModifier(this.baseValue);
    }
}
