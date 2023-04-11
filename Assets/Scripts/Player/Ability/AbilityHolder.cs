using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
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

    private KeyCode key = KeyCode.X;

    AbilityState state =  AbilityState.ready;
    // Update is called once per frame

    private void Start() {
  
    }
    void Update()
    {
        ActivateSkill();
        
    }

    void ActivateSkill(){
        switch (state)
        {
            case AbilityState.ready:
                if(Input.GetKeyDown(key) && charChoose.name == "Player 1"){
                    
                    if(ability.name == "Spinning Slash"){
                        animator.SetBool("Spinning Slash", true);
                    }
                    else{
                        animator.SetBool("Spinning Slash 2", true);
                    }
                } 
                else if (Input.GetKeyDown(key) && charChoose.name == "Player 2"){
                    
                    if(ability.name == "Cross Slash"){
                        animator.SetBool("CrossSlash", true);
                    }
                    else{
                        animator.SetBool("CrossSlash 2", true);
                    }
                }
                else if (Input.GetKeyDown(key) && charChoose.name == "Player 3"){
                   
                    if(ability.name == "Charge Slash"){
                        animator.SetBool("ChargeSlash", true);
                    }
                    else{
                        animator.SetBool("ChargeSlash 2", true);
                    }
                }
            break;
            case AbilityState.active:
                if(activeTime > 0){
                    activeTime -= Time.deltaTime;
                }
                else{
                    if(charChoose.name == "Player 1"){
                        if(ability.name == "Spinning Slash"){
                            animator.SetBool("Spinning Slash", false);
                        }
                        else{
                            animator.SetBool("Spinning Slash 2", false);
                        }
                    }
                    else if(charChoose.name == "Player 2"){
                        if(ability.name == "Cross Slash"){
                            animator.SetBool("CrossSlash", false);
                        }
                        else{
                            animator.SetBool("CrossSlash 2", false);
                        }
                    }
                    else if (charChoose.name == "Player 3"){
                        if(ability.name == "Charge Slash"){
                            animator.SetBool("ChargeSlash", false);
                        }
                        else{
                            animator.SetBool("ChargeSlash 2", false);
                        }
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

    void useSkillX(){
        ability.Activate(gameObject);
        state = AbilityState.active;
        activeTime = ability.activeTime;
    }

    void useSkillXNoCD(){
        ability.Activate(gameObject);
    }

    public float getCooldownTime(){
        return this.cooldownTime;
    }

    public float getActiveTime(){
        return this.activeTime;
    }
}

