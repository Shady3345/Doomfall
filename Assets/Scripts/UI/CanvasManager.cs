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
            Destroy(this.gameObject);
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
        if (keyColor == "red") redKey.SetActive(true);
        if (keyColor == "blue") blueKey.SetActive(true);
        if (keyColor == "green") greenKey.SetActive(true);
    }

    public void ClearKeys()
    {
        redKey.SetActive(false);
        blueKey.SetActive(false);
        greenKey.SetActive(false);
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