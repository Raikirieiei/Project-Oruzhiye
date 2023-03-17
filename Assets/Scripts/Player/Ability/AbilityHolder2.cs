using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder2 : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;
    
    enum AbilityState
    {
        ready,
        active,
        cooldown,
    }

    private KeyCode key = KeyCode.C;

    AbilityState state =  AbilityState.ready;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                if(Input.GetKeyDown(key)){
                    ability.Activate(gameObject);
                    state = AbilityState.active;
                    activeTime = ability.activeTime;
                } 
            break;
            case AbilityState.active:
                if(activeTime > 0){
                    activeTime -= Time.deltaTime;
                }
                else{
                    ability.BeginCooldown(gameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = ability.cooldownTime;
                }
            break;
            case AbilityState.cooldown:
                if(cooldownTime > 0){
                    cooldownTime -= Time.deltaTime;
                }
                else{
                    state = AbilityState.ready;
                }
            break;
        }
        
    }
}

