using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat baseAttack;
    public int baseMaxHealth = 100;
    public int currentHealth {get; private set;}

    private void Awake() {
        currentHealth = baseMaxHealth;
    }

}
