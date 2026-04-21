using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{
    public static BossUI Instance; // simple global access

    public GameObject root; // whole UI container

    public Slider healthBar;    // instant HP bar
    public Slider delayedBar;   // smooth HP bar

    public TMP_Text bossNameText;

    public float smoothSpeed = 5f; // speed of delayed bar

    private float targetHealth; // where delayed bar moves to

    private void Awake()
    {
        Instance = this;

        // hide UI at start
        root.SetActive(false);
    }

    private void Update()
    {
        // smooth follow effect
        if (delayedBar.value > targetHealth)
        {
            delayedBar.value = Mathf.Lerp(
                delayedBar.value,
                targetHealth,
                Time.deltaTime * smoothSpeed
            );
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

        // setup bars
        healthBar.maxValue = maxHealth;
        delayedBar.maxValue = maxHealth;

        healthBar.value = maxHealth;
        delayedBar.value = maxHealth;

        targetHealth = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        // instant bar updates directly
        healthBar.value = currentHealth;

        // delayed bar moves smoothly
        targetHealth = currentHealth;
    }

    public void HideBoss()
    {
        root.SetActive(false);
    }
}