using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder2 : MonoBehaviour
{
    public Ability ability;
    public Player charChoose;
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
                if(Input.GetKeyDown(key) && charChoose.name == "Player 1"){
                    if(ability.name == "Sword Wave"){
                        animator.SetBool("Projectile Slash", true);
                    }
                    else{
                        animator.SetBool("Projectile Slash 2", true);
                    }
                } 
                else if (Input.GetKeyDown(key) && charChoose.name == "Player 2"){
                    
                    if(ability.name == "Triple Stab"){
                        animator.SetBool("TripleStab", true);
                    }
                    else{
                        animator.SetBool("TripleStab 2", true);
                    }
                }
                else if (Input.GetKeyDown(key) && charChoose.name == "Player 3"){

                    if(ability.name == "Shield Barrier"){
                        animator.SetBool("ShieldBarrier", true);
                    }
                    else{
                        animator.SetBool("ShieldBarrier 2", true);
                    }
                    
                }
            break;
            case AbilityState.active:
                if(activeTime > 0){
                    activeTime -= Time.deltaTime;
                }
                else{
                    if(charChoose.name == "Player 1"){
                        if(ability.name == "Sword Wave"){
                            animator.SetBool("Projectile Slash", false);
                        }
                        else{
                            animator.SetBool("Projectile Slash 2", false);
                        }
                    }
                    else if(charChoose.name == "Player 2"){
                        if(ability.name == "Triple Stab"){
                            animator.SetBool("TripleStab", false);
                        }
                        else{
                            animator.SetBool("TripleStab 2", false);
                        }
                    }
                    else if(charChoose.name == "Player 3"){
                        DefReturnToNormal();
                    }
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

    void useSkillC(){
        ability.Activate(gameObject);
        state = AbilityState.active;
        activeTime = ability.activeTime;
    }

    void useSkillCNoCD(){
        ability.Activate(gameObject);
    }

    void DefReturnToNormal(){
        charChoose.defend = charChoose.GetComponent<CharacterStats>().baseDefend.getValue();
    }

    public float getCooldownTime(){
        return this.cooldownTime;
    }

    public float getActiveTime(){
        return this.activeTime;
    }
}

