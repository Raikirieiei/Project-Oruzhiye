using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{

    public float delayDestroyTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delayDestroyTime);
    }
}
