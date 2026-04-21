using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int health;

    [Header("Armor")]
    public int maxArmor = 100;
    private int armor;

    [Header("Hurt Effect")]
    public Image hurtOverlay;        // drag a full-screen red UI Image here
    public float hurtFadeDuration = 0.5f;
    private float hurtTimer = 0f;
    private bool isHurt = false;

    [Header("Hurt Sound")]
    public AudioClip hurtSound;
    private AudioSource audioSource;

    void Start()
    {
        health = maxHealth;
        armor = 0;
        CanvasManager.Instance.UpdateHealth(health);
        CanvasManager.Instance.UpdateArmor(armor);
        audioSource = GetComponent<AudioSource>();

        if (hurtOverlay != null)
            hurtOverlay.color = new Color(1f, 0f, 0f, 0f); // start invisible
    }

    void Update()
    {
        // Fade out the hurt overlay
        if (isHurt)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtOverlay != null)
            {
                float alpha = Mathf.Clamp01(hurtTimer / hurtFadeDuration);
                hurtOverlay.color = new Color(1f, 0f, 0f, alpha * 0.5f);
            }

            if (hurtTimer <= 0f)
                isHurt = false;
        }
    }

    void ShowHurtEffect()
    {
        isHurt = true;
        hurtTimer = hurtFadeDuration;

        if (hurtSound != null && audioSource != null)
            audioSource.PlayOneShot(hurtSound);
    }

    public int GetHealth() => health;
    public int GetArmor() => armor;

    public void DamagePlayer(int damage)
    {
        int remainingDamage = damage;

        if (armor > 0)
        {
            int armorAbsorb = Mathf.Min(armor, remainingDamage);
            armor -= armorAbsorb;
            remainingDamage -= armorAbsorb;
            Debug.Log("Armor absorbed: " + armorAbsorb + " | Armor left: " + armor);
        }

        if (remainingDamage > 0)
        {
            health -= remainingDamage;
            Debug.Log("Health took: " + remainingDamage + " | Health left: " + health);
        }

        health = Mathf.Clamp(health, 0, maxHealth);
        armor = Mathf.Clamp(armor, 0, maxArmor);

        ShowHurtEffect(); // ← plays on every hit

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
            Destroy(pickup);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
            Debug.Log("Health gained: " + amount + " | Health: " + health);
        }

        CanvasManager.Instance.UpdateHealth(health);
    }

    public void GiveArmor(int amount, GameObject pickup)
    {
        if (armor >= maxArmor)
        {
            Debug.Log("Armor already full!");
            Destroy(pickup);
            return;
        }

        armor += amount;
        armor = Mathf.Clamp(armor, 0, maxArmor);
        Debug.Log("Armor gained: " + amount + " | Armor: " + armor);
        CanvasManager.Instance.UpdateArmor(armor);
        Destroy(pickup);
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
        DeathScreen.Instance.ShowDeathScreen();
    }
}