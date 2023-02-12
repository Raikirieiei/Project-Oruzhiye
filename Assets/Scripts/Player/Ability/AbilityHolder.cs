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
    AbilityState state = AbilityState.ready;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.X)){
            keyIndex = 0;
            Debug.Log(key[keyIndex]);
        }
        // else if (Input.GetKey(KeyCode.C)){
        //     keyIndex = 1;
        //     Debug.Log(key[keyIndex]);
        // }

        switch (state)
        {
            case AbilityState.ready:
                if(Input.GetKeyDown(key[keyIndex])){
                    ability[keyIndex].Activate(gameObject);
                    state = AbilityState.active;
                    activeTime = ability[keyIndex].activeTime;
                } 
            break;
            case AbilityState.active:
                if(activeTime > 0){
                    activeTime -= Time.deltaTime;
                }
                else{
                    ability[keyIndex].BeginCooldown(gameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = ability[keyIndex].cooldownTime;
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
