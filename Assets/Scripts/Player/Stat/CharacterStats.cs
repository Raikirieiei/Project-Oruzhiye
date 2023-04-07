using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat baseAttack;
    public Stat baseMoveSpeed;
    public Stat baseDefend;
    public Stat baseHealth;
    public int currentHealth;

    private void Awake() {
        currentHealth = baseHealth.getValue();
    }

}
