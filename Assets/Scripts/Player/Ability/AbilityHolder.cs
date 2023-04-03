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
        Debug.Log(charChoose.name);
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
                    animator.SetBool("Spinning Slash", true);
                } 
                else if (Input.GetKeyDown(key) && charChoose.name == "Player 2"){
                    animator.SetBool("CrossSlash", true);
                }
                else if (Input.GetKeyDown(key) && charChoose.name == "Player 3"){
                    animator.SetBool("ChargeSlash", true);
                }
            break;
            case AbilityState.active:
                if(activeTime > 0){
                    activeTime -= Time.deltaTime;
                }
                else{
                    if(charChoose.name == "Player 1"){
                        animator.SetBool("Spinning Slash", false);
                    }
                    else if(charChoose.name == "Player 2"){
                        animator.SetBool("CrossSlash", false);
                    }
                    else if (charChoose.name == "Player 3"){
                        animator.SetBool("ChargeSlash", false);
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

    void Slow(){
        charChoose.runSpeed = 0;
    }

    void ReturnToNormal(){
        charChoose.runSpeed = charChoose.GetComponent<CharacterStats>().baseMoveSpeed.getValue();
    }

    public float getCooldownTime(){
        return this.cooldownTime;
    }
}

