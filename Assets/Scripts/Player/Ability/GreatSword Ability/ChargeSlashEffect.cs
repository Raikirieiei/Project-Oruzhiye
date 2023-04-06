using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlashEffect : MonoBehaviour
{
  // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StayTime(0.3f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StayTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
