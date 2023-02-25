using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat baseAttack;
    public Stat baseMoveSpeed;
    public int baseMaxHealth = 100;
    public int currentHealth;

    // public static event Action<GameState> OnStatChanged;

    private void Awake() {
        currentHealth = baseMaxHealth;
    }

}
