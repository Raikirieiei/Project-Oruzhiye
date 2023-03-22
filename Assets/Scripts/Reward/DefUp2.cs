using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DefUp2 : StatReward
{
    public override void Selected(CharacterStats characterStat){
        Debug.Log("Reward Selected: " + this.name);
        characterStat.baseDefend.addModifier(this.baseValue);
    }
}
