using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/SwordWave")]
public class SwordWave : Ability
{
    public GameObject swordWaveProjectile;
    public override void Activate(GameObject parent){
        PlayerCombat playerCombat = parent.GetComponent<PlayerCombat>();
        GameObject projectile = Instantiate(swordWaveProjectile, parent.transform.GetChild(0).position, parent.transform.rotation);
    }
}
