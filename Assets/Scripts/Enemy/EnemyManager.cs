using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    // private Component[] enemyArray;
    // private bool isEnemyDead = false;    

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("All enemy has been defeated. Stage Clear!!");
            // GameManager.instance.UpdateGameState(GameState.RewardSelect);
            Destroy(gameObject);
        }

    }

}
