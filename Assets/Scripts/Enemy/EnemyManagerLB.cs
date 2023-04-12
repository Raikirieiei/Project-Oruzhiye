using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerLB : MonoBehaviour
{

    void Update()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("Last Boss has been defeated !!");
            Destroy(gameObject);
        }
    }
}
