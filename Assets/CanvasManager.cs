using UnityEngine;
using TMPro;
using UnityEditorInternal;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI health;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI ammo;

    [Header("Keys")]
    public GameObject redKey;
    public GameObject greenKey;
    public GameObject blueKey;


    private static CanvasManager _instance;
    public static CanvasManager Instance

    {
        get { return _instance; }   

    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // --- HEALTH ---
    public void UpdateHealth(int healthValue)
    {
        if (health != null)
            health.text = healthValue.ToString();
    }

    // --- ARMOR ---
    public void UpdateArmor(int armorValue)
    {
        if (armor != null)
            armor.text = armorValue.ToString();
    }

    // --- AMMO ---
    public void UpdateAmmo(int ammoValue)
    {
        if (ammo != null)
            ammo.text = ammoValue.ToString();
    }

    // --- KEYS ---
    public void UpdateKeys(bool hasRed, bool hasGreen, bool hasBlue)
    {
        if (redKey != null)
            redKey.SetActive(hasRed);

        if (greenKey != null)
            greenKey.SetActive(hasGreen);

        if (blueKey != null)
            blueKey.SetActive(hasBlue);
    }
}