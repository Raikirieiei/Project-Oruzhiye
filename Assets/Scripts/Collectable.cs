using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    [SerializeField] AudioSource destroyAudio;
    [SerializeField] AudioSource dropAudio;
    [SerializeField] float time;

    private int GROUND_LAYER;

    void Start()
    {
        GROUND_LAYER = LayerMask.NameToLayer("Ground");
        DestroyObjectDelayed();
    }

    void DestroyObjectDelayed()
    {
        // Kills the game object in 5 seconds after loading the object
        Destroy(gameObject, time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GROUND_LAYER)
        {
            dropAudio.Play();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            destroyAudio.Play();
            Destroy(gameObject, 0.05f);
        }
    }
}
