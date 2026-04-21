using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{
    public static BossUI Instance;

    public GameObject root;
    public Slider healthBar;        // instant HP
    public Slider delayedBar;       // smooth/delay HP
    public TMP_Text bossNameText;

    public float smoothSpeed = 5f;

    private float targetHealth;

    private void Awake()
    {
        Instance = this;
        root.SetActive(false);
    }

    private void Update()
    {
        // smoothly move delayed bar toward real HP
        if (delayedBar.value > targetHealth)
        {
            delayedBar.value = Mathf.Lerp(delayedBar.value, targetHealth, Time.deltaTime * smoothSpeed);
        }
        else
        {
            delayedBar.value = targetHealth;
        }
    }

    public void ShowBoss(string name, int maxHealth)
    {
        root.SetActive(true);

        bossNameText.text = name;

        healthBar.maxValue = maxHealth;
        delayedBar.maxValue = maxHealth;

        healthBar.value = maxHealth;
        delayedBar.value = maxHealth;

        targetHealth = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        // instant drop
        healthBar.value = currentHealth;

        // delayed bar will follow
        targetHealth = currentHealth;
    }

    public void HideBoss()
    {
        root.SetActive(false);
    }
}