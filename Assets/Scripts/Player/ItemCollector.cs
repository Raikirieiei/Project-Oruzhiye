using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] Text coinsText;
    private int coins = 0;

    private string COIN_TAG = "Coin";
    private string HEART_TAG = "Heart";
    private string POTION_TAG = "Potion";

    private CharacterStats characterStats;
    private Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(COIN_TAG))
        {
            coins++;
            coinsText.text = coins.ToString("000");
        } else if (collision.gameObject.CompareTag(HEART_TAG))
        {
            addHealthInPercentage(15f);
        } else if (collision.gameObject.CompareTag(POTION_TAG))
        {
            addHealthInPercentage(50f);
        }
    }

    private void addHealthInPercentage(float percent)
    {
        characterStats = GetComponent<CharacterStats>();
        player = GetComponent<Player>();
        if (percent <= 0)
        {
            return;
        }

        float amount = (percent/100) * characterStats.baseHealth.getValue();
        player.currentHealth += (int)amount;
        if (player.currentHealth > characterStats.baseHealth.getValue())
        {
            player.currentHealth = characterStats.baseHealth.getValue();
        }
        Debug.Log("player hp+: " + amount);
    }
}
