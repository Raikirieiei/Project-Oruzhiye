using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float length, startpos;
    private Transform cam;
    public float parallaxEffect;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        Invoke(nameof(Find_MainCamera), 0.5f);
        if (cam == null) return;
    }

    private void Find_MainCamera()
    {
        try
        {
            cam = GameObject.FindWithTag("MainCamera").transform;
        }
        catch (NullReferenceException)
        {
            Debug.Log("target gameObjects is not present in hierarchy ");
        }
    }

    void FixedUpdate()
    {
        float temp = (cam.position.x * (1 - parallaxEffect));
        float dist = (cam.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
