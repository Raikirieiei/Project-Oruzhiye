using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability[] ability;
    float cooldownTime;
    float activeTime;
    
    enum AbilityState
    {
        ready,
        active,
        cooldown,
    }

    private KeyCode[] key = {KeyCode.X , KeyCode.C};
    private int keyIndex;
    AbilityState[] state = {AbilityState.ready, AbilityState.ready};
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.X)){
            keyIndex = 0;
        }
        else if (Input.GetKey(KeyCode.C)){
            keyIndex = 1;
        }

        switch (state[keyIndex])
        {
            case AbilityState.ready:
                if(Input.GetKeyDown(key[keyIndex])){
                    ability[keyIndex].Activate(gameObject);
                    state[keyIndex] = AbilityState.active;
                    activeTime = ability[keyIndex].activeTime;
                } 
            break;
            case AbilityState.active:
                if(activeTime > 0){
                    activeTime -= Time.deltaTime;
                }
                else{
                    ability[keyIndex].BeginCooldown(gameObject);
                    state[keyIndex] = AbilityState.cooldown;
                    cooldownTime = ability[keyIndex].cooldownTime;
                }
            break;
            case AbilityState.cooldown:
                if(cooldownTime > 0){
                    cooldownTime -= Time.deltaTime;
                }
                else{
                    state[keyIndex] = AbilityState.ready;
                }
            break;
        }
        
    }
}
