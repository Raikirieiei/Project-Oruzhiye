using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
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

    private KeyCode key = KeyCode.X;

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
                    animator.SetBool("Spinning Slash", true);
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
                    animator.SetBool("Spinning Slash", false);
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

    void useSpinningSlash(){
        ability.Activate(gameObject);
        state = AbilityState.active;
        activeTime = ability.activeTime;
    }
}

