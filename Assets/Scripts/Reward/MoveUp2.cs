using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveUp2 : StatReward
{
    public override void Selected(CharacterStats characterStat){
        Debug.Log("Reward Selected: " + this.name);
        characterStat.baseMoveSpeed.addModifier(this.baseValue);
    }
}
