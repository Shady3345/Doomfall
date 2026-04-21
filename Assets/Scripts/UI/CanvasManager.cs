using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI ammo;

    public Image weaponImage;
    public Sprite pistolSprite;
    public Sprite machinePistolSprite;
    public Sprite shotgunSprite;

    public GameObject redKey;
    public GameObject greenKey;
    public GameObject blueKey;

    public GameObject deathScreen;

    private static CanvasManager _instance;
    public static CanvasManager Instance => _instance;

    private void Awake()
    {
        // simple singleton
        if (_instance != null && _instance != this)
        {
            // copy references from new canvas
            _instance.AbsorbReferences(this);
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void AbsorbReferences(CanvasManager other)
    {
        // update UI references when scene reloads
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

    public void UpdateHealth(int value)
    {
        if (health != null)
            health.text = value.ToString();
    }

    public void UpdateArmor(int value)
    {
        if (armor != null)
            armor.text = value.ToString();
    }

    public void UpdateAmmo(int value)
    {
        if (ammo != null)
            ammo.text = value.ToString();
    }

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

    public void UpdateKeys(string keyColor)
    {
        if (keyColor == "red" && redKey != null) redKey.SetActive(true);
        if (keyColor == "blue" && blueKey != null) blueKey.SetActive(true);
        if (keyColor == "green" && greenKey != null) greenKey.SetActive(true);
    }

    public void ClearKeys()
    {
        if (redKey) redKey.SetActive(false);
        if (blueKey) blueKey.SetActive(false);
        if (greenKey) greenKey.SetActive(false);
    }

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
    }
}