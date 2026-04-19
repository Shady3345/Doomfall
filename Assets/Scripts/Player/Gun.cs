using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip pistolSound;
    public AudioClip machinePistolSound;
    public AudioClip shotgunSound;

    [Header("Effects")]
    public ParticleSystem pistolMuzzleFlash;
    public ParticleSystem machinePistolMuzzleFlash;
    public ParticleSystem shotgunMuzzleFlash;

    [Header("Weapon Prefabs")]
    public GameObject pistolPickupPrefab;
    public GameObject machinePistolPickupPrefab;
    public GameObject shotgunPickupPrefab;

    // ──────────────────────────────────────────
    //  REFERENCES
    // ──────────────────────────────────────────
    public Transform playerCamera;
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;
    public EnemyManager enemyManager;
    public float gunShotRadius = 10f;

    // ──────────────────────────────────────────
    //  WEAPON MODELS
    // ──────────────────────────────────────────
    [Header("Weapon Models")]
    public GameObject pistolObject;
    public GameObject machinePistolObject;
    public GameObject shotgunObject;

    // ──────────────────────────────────────────
    //  UI WEAPON ICONS
    // ──────────────────────────────────────────
    [Header("UI Weapon Icons")]
    public GameObject uiPistol;
    public GameObject uiShotgun;
    public GameObject uiMachinePistol;

    // ──────────────────────────────────────────
    //  WEAPON DEFINITION
    // ──────────────────────────────────────────
    public enum WeaponType { Pistol, MachinePistol, Shotgun }

    [System.Serializable]
    public class Weapon
    {
        public WeaponType type;
        public float damage;
        public float range;
        public float fireRate;
        public int maxAmmo;
        public int ammo;
        public bool unlocked;
    }

    public List<Weapon> weapons = new List<Weapon>
    {
        new Weapon { type = WeaponType.Pistol,        damage = 25f, range = 30f, fireRate = 2f,  maxAmmo = 12, ammo = 0, unlocked = false },
        new Weapon { type = WeaponType.MachinePistol, damage = 12f, range = 20f, fireRate = 10f, maxAmmo = 60, ammo = 0, unlocked = false },
        new Weapon { type = WeaponType.Shotgun,       damage = 15f, range = 15f, fireRate = 1f,  maxAmmo = 8,  ammo = 0, unlocked = false },
    };

    // ──────────────────────────────────────────
    //  STATE
    // ──────────────────────────────────────────
    public int currentWeaponIndex = 0;
    private float nextTimeToFire = 0f;
    private AudioSource audioSource;

    Weapon CurrentWeapon => weapons[currentWeaponIndex];

    // ──────────────────────────────────────────
    //  INIT
    // ──────────────────────────────────────────
    void Start()
    {
        foreach (var w in weapons) w.ammo = 0;

        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
        UpdateWeaponVisibility();
        audioSource = GetComponent<AudioSource>();
    }

    // ──────────────────────────────────────────
    //  UPDATE
    // ──────────────────────────────────────────
    void Update()
    {
        HandleWeaponSwitch();
        HandleFire();
        HandleMelee();
        UpdateEnemyList();
    }

    // ──────────────────────────────────────────
    //  ENEMY DETECTION
    // ──────────────────────────────────────────
    void UpdateEnemyList()
    {
        float r = CurrentWeapon.range;
        Collider[] hits = Physics.OverlapSphere(transform.position, r, enemyLayerMask);

        enemyManager.ClearEnemies();

        foreach (var col in hits)
        {
            Enemy e = col.GetComponent<Enemy>();
            if (e != null) enemyManager.AddEnemy(e);
        }
    }

    // ──────────────────────────────────────────
    //  WEAPON SWITCHING
    // ──────────────────────────────────────────
    void HandleWeaponSwitch()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) TrySwitchTo(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) TrySwitchTo(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) TrySwitchTo(2);
    }

    void DropCurrentWeapon()
    {
        if (!CurrentWeapon.unlocked) return;

        GameObject prefab = null;

        switch (CurrentWeapon.type)
        {
            case WeaponType.Pistol: prefab = pistolPickupPrefab; break;
            case WeaponType.MachinePistol: prefab = machinePistolPickupPrefab; break;
            case WeaponType.Shotgun: prefab = shotgunPickupPrefab; break;
        }

        if (prefab != null)
        {
            Vector3 dropPosition = playerCamera.position + playerCamera.forward * 1.5f;
            Instantiate(prefab, dropPosition, Quaternion.identity);
        }

        CurrentWeapon.unlocked = false;
        CurrentWeapon.ammo = 0;
    }

    void TrySwitchTo(int index)
    {
        if (index >= weapons.Count) return;
        if (!weapons[index].unlocked) { Debug.Log(weapons[index].type + " not unlocked yet!"); return; }
        if (index == currentWeaponIndex) return;

        currentWeaponIndex = index;
        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
        UpdateWeaponVisibility();
        Debug.Log("Switched to " + CurrentWeapon.type);
    }

    // ──────────────────────────────────────────
    //  FIRE INPUT
    // ──────────────────────────────────────────
    void HandleFire()
    {
        bool triggerPressed = CurrentWeapon.type == WeaponType.MachinePistol
            ? Mouse.current.leftButton.isPressed
            : Mouse.current.leftButton.wasPressedThisFrame;

        if (triggerPressed && Time.time > nextTimeToFire && CurrentWeapon.ammo > 0)
            Fire();
    }

    // ──────────────────────────────────────────
    //  FIRE
    // ──────────────────────────────────────────
    void Fire()
    {
        switch (CurrentWeapon.type)
        {
            case WeaponType.Pistol: if (pistolSound) audioSource.PlayOneShot(pistolSound); break;
            case WeaponType.MachinePistol: if (machinePistolSound) audioSource.PlayOneShot(machinePistolSound); break;
            case WeaponType.Shotgun: if (shotgunSound) audioSource.PlayOneShot(shotgunSound); break;
        }

        switch (CurrentWeapon.type)
        {
            case WeaponType.Pistol: if (pistolMuzzleFlash) pistolMuzzleFlash.Play(); break;
            case WeaponType.MachinePistol: if (machinePistolMuzzleFlash) machinePistolMuzzleFlash.Play(); break;
            case WeaponType.Shotgun: if (shotgunMuzzleFlash) shotgunMuzzleFlash.Play(); break;
        }

        foreach (var col in Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask))
        {
            EnemyAwareness a = col.GetComponent<EnemyAwareness>();
            if (a) a.isAggro = true;
        }

        switch (CurrentWeapon.type)
        {
            case WeaponType.Pistol:
            case WeaponType.MachinePistol:
                SingleRaycast(CurrentWeapon.damage, CurrentWeapon.range);
                break;
            case WeaponType.Shotgun:
                ShotgunBlast();
                break;
        }

        nextTimeToFire = Time.time + 1f / CurrentWeapon.fireRate;
        CurrentWeapon.ammo--;
        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
    }

    // ──────────────────────────────────────────
    //  RAYCAST HELPERS
    // ──────────────────────────────────────────
    void SingleRaycast(float baseDamage, float range)
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, range, raycastLayerMask))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float falloff = 1f - (hit.distance / range);
                float finalDamage = Mathf.Clamp(baseDamage * falloff, baseDamage * 0.25f, baseDamage);
                enemy.TakeDamage(finalDamage);
                Debug.Log($"[{CurrentWeapon.type}] Hit enemy for {finalDamage:F1} dmg");
            }
        }
    }

    void ShotgunBlast()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 spread = playerCamera.forward + new Vector3(
                Random.Range(-0.08f, 0.08f),
                Random.Range(-0.08f, 0.08f),
                0f);

            if (Physics.Raycast(playerCamera.position, spread, out RaycastHit hit, CurrentWeapon.range, raycastLayerMask))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.TakeDamage(CurrentWeapon.damage);
            }
        }
    }

    // ──────────────────────────────────────────
    //  MELEE (V key)
    // ──────────────────────────────────────────
    void HandleMelee()
    {
        if (!Keyboard.current.vKey.wasPressedThisFrame) return;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, 2f))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(40f);
                Debug.Log("Knife hit for 40 dmg");
            }
        }
    }

    // ──────────────────────────────────────────
    //  WEAPON VISIBILITY
    // ──────────────────────────────────────────
    void UpdateWeaponVisibility()
    {
        bool pistolActive = CurrentWeapon.type == WeaponType.Pistol && CurrentWeapon.unlocked;
        bool machineActive = CurrentWeapon.type == WeaponType.MachinePistol && CurrentWeapon.unlocked;
        bool shotgunActive = CurrentWeapon.type == WeaponType.Shotgun && CurrentWeapon.unlocked;

        if (pistolObject) pistolObject.SetActive(pistolActive);
        if (machinePistolObject) machinePistolObject.SetActive(machineActive);
        if (shotgunObject) shotgunObject.SetActive(shotgunActive);

        if (uiPistol) uiPistol.SetActive(pistolActive);
        if (uiShotgun) uiShotgun.SetActive(shotgunActive);
        if (uiMachinePistol) uiMachinePistol.SetActive(machineActive);
    }

    // ──────────────────────────────────────────
    //  PUBLIC: UNLOCK WEAPON
    // ──────────────────────────────────────────
    public void UnlockWeapon(WeaponType type)
    {
        foreach (var w in weapons)
        {
            if (w.type == type && !w.unlocked)
            {
                DropCurrentWeapon();

                w.unlocked = true;
                w.ammo = w.maxAmmo;
                Debug.Log(type + " unlocked!");

                currentWeaponIndex = weapons.IndexOf(w);
                CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
                UpdateWeaponVisibility();
            }
        }
    }

    // ──────────────────────────────────────────
    //  PUBLIC: GIVE AMMO
    // ──────────────────────────────────────────
    public void GiveAmmo(int amount, WeaponType targetType, GameObject pickup)
    {
        Weapon target = weapons.Find(w => w.type == targetType);

        if (target == null) return;

        if (target.ammo >= target.maxAmmo)
        {
            Debug.Log("Ammo already full!");
            return;
        }

        target.ammo = Mathf.Min(target.ammo + amount, target.maxAmmo);
        Destroy(pickup);

        if (target.type == CurrentWeapon.type)
            CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);

        Debug.Log($"Gave {amount} ammo to {targetType}");
    }
}