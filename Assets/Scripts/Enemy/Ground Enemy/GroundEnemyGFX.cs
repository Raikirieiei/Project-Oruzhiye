using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GroundEnemyGFX : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if(target.position.x > gameObject.transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (target.position.x <= gameObject.transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
