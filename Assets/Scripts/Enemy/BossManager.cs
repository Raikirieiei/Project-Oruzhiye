using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("All boss has been defeated. Stage Clear!!");
            GameManager.instance.UpdateGameState(GameState.SkillSelect);
            Destroy(gameObject);
        }

    }

}
