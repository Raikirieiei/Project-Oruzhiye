using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder2 : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;
    public Animator animator;
    
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
        ActivateSkill();
    }

    void ActivateSkill(){
        switch (state)
        {
            case AbilityState.ready:
                if(Input.GetKeyDown(key)){
                    animator.SetBool("Projectile Slash", true);
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
                    animator.SetBool("Projectile Slash", false);
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

    void useSwordWave(){
        ability.Activate(gameObject);
        state = AbilityState.active;
        activeTime = ability.activeTime;
    }
}

