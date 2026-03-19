using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public Image healthFill;
    public Image armorFill;

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        float healthPercent = (float)playerHealth.GetHealth() / playerHealth.maxHealth;
        float armorPercent = (float)playerHealth.GetArmor() / playerHealth.maxArmor;

        healthFill.fillAmount = healthPercent;
        armorFill.fillAmount = armorPercent;
    }
}