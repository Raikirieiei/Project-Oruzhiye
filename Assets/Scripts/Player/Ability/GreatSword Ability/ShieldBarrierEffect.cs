using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBarrierEffect : MonoBehaviour
{
    private GameObject player;
    private bool waitForSecond = true;

    void Awake(){
        Invoke(nameof(Find_player), (float)0.01);
        if (player == null) return;
    }

    void Start()
    {
       StartCoroutine(StayTime(5f));
    }

    // Update is called once per frame
    void Update()
    {   
        if(waitForSecond){
            StartCoroutine(WaitASec(0.01f));
        }else{
            gameObject.transform.position = player.transform.position;
        }
    }
    

    private void Find_player()
    {
        player = GameObject.FindWithTag("Player");
    }

    IEnumerator StayTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    IEnumerator WaitASec(float delay)
    {
        yield return new WaitForSeconds(delay);
        waitForSecond = false;
    }
}
