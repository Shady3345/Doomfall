using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair Parts")]
    public Image topLine;
    public Image bottomLine;
    public Image leftLine;
    public Image rightLine;
    public Image centerDot;

    [Header("Settings")]
    public Color inRangeColor = new Color(1f, 0f, 0f, 1f);    // red = in range
    public Color outRangeColor = new Color(1f, 1f, 1f, 0.5f); // white = out of range
    public float gapInRange = 5f;
    public float gapOutRange = 20f;
    public float animSpeed = 10f;

    private Gun gun;
    private float currentGap;

    void Start()
    {
        gun = Object.FindFirstObjectByType<Gun>();
        currentGap = gapOutRange;
    }

    void Update()
    {
        if (gun == null) return;

        // Check if any weapon is unlocked
        bool hasWeapon = false;
        foreach (var w in gun.weapons)
            if (w.unlocked) { hasWeapon = true; break; }

        if (!hasWeapon)
        {
            SetCrosshairVisible(false);
            return;
        }

        SetCrosshairVisible(true);

        // Safety check before accessing current weapon
        if (gun.weapons == null || gun.weapons.Count == 0) return;
        if (gun.currentWeaponIndex < 0 || gun.currentWeaponIndex >= gun.weapons.Count) return;

        Gun.Weapon current = gun.weapons[gun.currentWeaponIndex];
        if (current == null) return;

        // Raycast to check if enemy is in range
        bool enemyInRange = false;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, current.range))
        {
            if (hit.collider.GetComponent<Enemy>() != null)
                enemyInRange = true;
        }

        // Animate gap
        float targetGap = enemyInRange ? gapInRange : gapOutRange;
        currentGap = Mathf.Lerp(currentGap, targetGap, Time.deltaTime * animSpeed);

        // Update color
        Color targetColor = enemyInRange ? inRangeColor : outRangeColor;
        SetColor(targetColor);

        // Update positions
        topLine.rectTransform.anchoredPosition = new Vector2(0, currentGap + topLine.rectTransform.sizeDelta.y / 2);
        bottomLine.rectTransform.anchoredPosition = new Vector2(0, -currentGap - bottomLine.rectTransform.sizeDelta.y / 2);
        leftLine.rectTransform.anchoredPosition = new Vector2(-currentGap - leftLine.rectTransform.sizeDelta.x / 2, 0);
        rightLine.rectTransform.anchoredPosition = new Vector2(currentGap + rightLine.rectTransform.sizeDelta.x / 2, 0);
    }

    void SetColor(Color color)
    {
        topLine.color = color;
        bottomLine.color = color;
        leftLine.color = color;
        rightLine.color = color;
        if (centerDot) centerDot.color = color;
    }

    void SetCrosshairVisible(bool visible)
    {
        topLine.gameObject.SetActive(visible);
        bottomLine.gameObject.SetActive(visible);
        leftLine.gameObject.SetActive(visible);
        rightLine.gameObject.SetActive(visible);
        if (centerDot) centerDot.gameObject.SetActive(visible);
    }
}