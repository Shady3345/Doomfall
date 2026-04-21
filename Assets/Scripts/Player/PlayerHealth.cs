using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int health;

    public int maxArmor = 100;
    private int armor;

    public Image hurtOverlay;

    void Start()
    {
        health = maxHealth;
        armor = 0;

        CanvasManager.Instance.UpdateHealth(health);
        CanvasManager.Instance.UpdateArmor(armor);
    }

    public void DamagePlayer(int damage)
    {
        int remainingDamage = damage;

        // armor absorbs first
        if (armor > 0)
        {
            int absorbed = Mathf.Min(armor, remainingDamage);
            armor -= absorbed;
            remainingDamage -= absorbed;
        }

        // rest goes to health
        if (remainingDamage > 0)
        {
            health -= remainingDamage;
        }

        health = Mathf.Clamp(health, 0, maxHealth);
        armor = Mathf.Clamp(armor, 0, maxArmor);

        if (health <= 0)
            Die();

        CanvasManager.Instance.UpdateHealth(health);
        CanvasManager.Instance.UpdateArmor(armor);
    }
    public void GiveHealth(int amount, GameObject pickup)
    {
        if (health < maxHealth)
        {
            health += amount;

            // clamp so it doesn't exceed max
            health = Mathf.Clamp(health, 0, maxHealth);

            Destroy(pickup);
        }

        CanvasManager.Instance.UpdateHealth(health);
    }

    public void GiveArmor(int amount, GameObject pickup)
    {
        if (armor >= maxArmor)
        {
            Destroy(pickup);
            return;
        }

        armor += amount;

        armor = Mathf.Clamp(armor, 0, maxArmor);

        CanvasManager.Instance.UpdateArmor(armor);

        Destroy(pickup);
    }
    void Die()
    {
        Debug.Log("Player died");

        DeathScreen.Instance.ShowDeathScreen();
    }
}