using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int health;

    [Header("Armor")]
    public int maxArmor = 100;
    private int armor;

    private void Start()
    {
        health = maxHealth;
        armor = maxArmor;
    }

    private void Update()
    {
        // TEST DAMAGE
        if (Keyboard.current != null && Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            DamagePlayer(25);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetArmor()
    {
        return armor;
    }

    public void DamagePlayer(int damage)
    {
        int remainingDamage = damage;

        //Armor absorbs damage first
        if (armor > 0)
        {
            int armorAbsorb = Mathf.Min(armor, remainingDamage);
            armor -= armorAbsorb;
            remainingDamage -= armorAbsorb;

            Debug.Log("Armor absorbed: " + armorAbsorb + " | Armor left: " + armor);
        }

        //Apply remaining damage to health
        if (remainingDamage > 0)
        {
            health -= remainingDamage;
            Debug.Log("Health took: " + remainingDamage + " | Health left: " + health);
        }

        // Clamp values
        health = Mathf.Clamp(health, 0, maxHealth);
        armor = Mathf.Clamp(armor, 0, maxArmor);

        // Check death
        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log("Healed: " + amount + " | Health: " + health);
    }

    public void AddArmor(int amount)
    {
        armor += amount;
        armor = Mathf.Clamp(armor, 0, maxArmor);

        Debug.Log("Armor gained: " + amount + " | Armor: " + armor);
    }

    void Die()
    {
        Debug.Log("Player died!");
        // TODO: Add respawn / restart / UI
    }
}