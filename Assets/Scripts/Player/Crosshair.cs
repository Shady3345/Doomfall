using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image topLine;
    public Image bottomLine;
    public Image leftLine;
    public Image rightLine;
    public Image centerDot;

    public Color inRangeColor = Color.red;
    public Color outRangeColor = new Color(1f, 1f, 1f, 0.5f);

    public float gapInRange = 5f;
    public float gapOutRange = 20f;

    public float animSpeed = 10f;

    private Gun gun;
    private float currentGap;

    void Start()
    {
        gun = FindFirstObjectByType<Gun>();
        currentGap = gapOutRange;
    }

    void Update()
    {
        if (gun == null) return;

        // check if something shootable is in range
        bool enemyInRange = false;

        if (Physics.Raycast(Camera.main.transform.position,
            Camera.main.transform.forward,
            out RaycastHit hit,
            gun.weapons[gun.currentWeaponIndex].range))
        {
            if (hit.collider.GetComponent<Enemy>() ||
                hit.collider.GetComponent<Boss>())
                enemyInRange = true;
        }

        float targetGap = enemyInRange ? gapInRange : gapOutRange;

        // smooth animation
        currentGap = Mathf.Lerp(currentGap, targetGap, Time.deltaTime * animSpeed);

        Color color = enemyInRange ? inRangeColor : outRangeColor;
        SetColor(color);

        // move lines
        topLine.rectTransform.anchoredPosition = new Vector2(0, currentGap);
        bottomLine.rectTransform.anchoredPosition = new Vector2(0, -currentGap);
        leftLine.rectTransform.anchoredPosition = new Vector2(-currentGap, 0);
        rightLine.rectTransform.anchoredPosition = new Vector2(currentGap, 0);
    }

    void SetColor(Color c)
    {
        topLine.color = c;
        bottomLine.color = c;
        leftLine.color = c;
        rightLine.color = c;
        if (centerDot) centerDot.color = c;
    }
}