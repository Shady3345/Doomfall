using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI health;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI ammo;

    [Header("Weapon Sprite")]
    public Image weaponImage;
    public Sprite pistolSprite;
    public Sprite machinePistolSprite;
    public Sprite shotgunSprite;

    [Header("Keys")]
    public GameObject redKey;
    public GameObject greenKey;
    public GameObject blueKey;

    [Header("Death Screen")]
    public GameObject deathScreen;

    private static CanvasManager _instance;
    public static CanvasManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // A new scene loaded with fresh UI references — hand them to the surviving instance
            _instance.AbsorbReferences(this);
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Copies all UI references from a newly loaded CanvasManager into this one
    private void AbsorbReferences(CanvasManager other)
    {
        health = other.health;
        armor = other.armor;
        ammo = other.ammo;
        weaponImage = other.weaponImage;
        pistolSprite = other.pistolSprite;
        machinePistolSprite = other.machinePistolSprite;
        shotgunSprite = other.shotgunSprite;
        redKey = other.redKey;
        greenKey = other.greenKey;
        blueKey = other.blueKey;
        deathScreen = other.deathScreen;
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

    // --- WEAPON SPRITE ---
    public void UpdateWeaponSprite(Gun.WeaponType type)
    {
        if (weaponImage == null) return;
        weaponImage.sprite = type switch
        {
            Gun.WeaponType.Pistol => pistolSprite,
            Gun.WeaponType.MachinePistol => machinePistolSprite,
            Gun.WeaponType.Shotgun => shotgunSprite,
            _ => null
        };
    }

    // --- KEYS ---
    public void UpdateKeys(string keyColor)
    {
        if (keyColor == "red" && redKey != null) redKey.SetActive(true);
        if (keyColor == "blue" && blueKey != null) blueKey.SetActive(true);
        if (keyColor == "green" && greenKey != null) greenKey.SetActive(true);
    }

    public void ClearKeys()
    {
        if (redKey != null) redKey.SetActive(false);
        if (blueKey != null) blueKey.SetActive(false);
        if (greenKey != null) greenKey.SetActive(false);
    }

    // --- DEATH SCREEN ---
    public void ShowDeathScreen()
    {
        if (deathScreen != null)
            deathScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        Debug.Log("Game Quit");
    }
}