using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHand : MonoBehaviour
{
    [Header("Basic Attribute")]
    [SerializeField] int damage;
    private bool collideOnce = false;

    [Header("Player Detection")]
    [SerializeField] Transform player;
    [SerializeField] LayerMask playerLayer;

    void Start()
    {
        Invoke(nameof(Find_player), 0.25f);
        if (player == null) return;
    }

    private void Find_player()
    {
        try
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        catch (NullReferenceException)
        {
            Debug.Log("target gameObjects is not present in hierarchy ");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      if (!collideOnce)
      {
        Player playerScript = player.GetComponent<Player>();
        playerScript.TakeDamage(damage, new Vector2(0f, 0f));
        collideOnce = true;
      }
    }
}
