using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerEnemy : Enemy
{
    [Header("Basic Attributes")]
    private int defend = 15;
    private bool enrage = false;

    private int finalDamage;
    private float damageReduction;
    [SerializeField] AudioSource hurtAudio;

    public override void TakeDamage(int damage){
        float playerPosition = player.position.x - transform.position.x;
        float knockbackDir = -playerPosition/Math.Abs(playerPosition);
        
        damageReduction = damage * (float)defend/100;
        finalDamage = damage - (int)damageReduction;
        currentHealth -= finalDamage;

        // Instantiate Damagepopup
        GameObject dmgPopUp = Instantiate(damagePopUp, transform.position, Quaternion.identity);
        dmgPopUp.transform.GetChild(0).GetComponent<TextMesh>().text = finalDamage.ToString();
        hurtAudio.Play();

        enemyRB.velocity = Vector3.zero;
        enemyRB.AddForce(new Vector2(2 * knockbackDir, 1), ForceMode2D.Impulse);
        if (currentHealth <= 0)
        {
            Die();
        } else if (defend == 15 && currentHealth <= 300 && !enrage)
        {
            GameObject engage = Instantiate(damagePopUp, transform.position, Quaternion.identity);
            engage.transform.GetChild(0).GetComponent<TextMesh>().text = "250";
            engage.transform.GetChild(0).GetComponent<TextMesh>().color = Color.green;
            enrage = true;
            currentHealth += 250;
            defend = 20;
        }
    }
}
