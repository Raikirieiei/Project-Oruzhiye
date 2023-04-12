using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    void Update()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("All enemy has been defeated. Stage Clear!!");
            GameManager.instance.UpdateGameState(GameState.RewardSelect);
            Destroy(gameObject);
        }
    }

}
