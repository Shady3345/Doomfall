using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{

    [Header("Weapon Models")]
    public GameObject pistolObject;
    public GameObject machinePistolObject;
    public GameObject shotgunObject;

    // ──────────────────────────────────────────
    //  REFERENCES
    // ──────────────────────────────────────────
    public Transform playerCamera;
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;
    public EnemyManager enemyManager;
    public float gunShotRadius = 10f;

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
    private int currentWeaponIndex = 0;
    private float nextTimeToFire = 0f;
    private BoxCollider gunTrigger;

    Weapon CurrentWeapon => weapons[currentWeaponIndex];

    // ──────────────────────────────────────────
    //  INIT
    // ──────────────────────────────────────────
    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();

        foreach (var w in weapons) w.ammo = 0;
        weapons[0].unlocked = true;

        RefreshTrigger();
        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
        UpdateWeaponVisibility();

    }

    // ──────────────────────────────────────────
    //  UPDATE
    // ──────────────────────────────────────────
    void Update()
    {
        HandleWeaponSwitch();
        HandleFire();
        HandleMelee();
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

    void TrySwitchTo(int index)
    {
        if (index >= weapons.Count) return;
        if (!weapons[index].unlocked) { Debug.Log(weapons[index].type + " not unlocked yet!"); return; }
        if (index == currentWeaponIndex) return;

        currentWeaponIndex = index;
        RefreshTrigger();
        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
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
        AudioSource audio = GetComponent<AudioSource>();
        if (audio) { audio.Stop(); audio.Play(); }

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
    //  PUBLIC: UNLOCK WEAPON
    // ──────────────────────────────────────────
    public void UnlockWeapon(WeaponType type)
    {
        foreach (var w in weapons)
        {
            if (w.type == type && !w.unlocked)
            {
                w.unlocked = true;
                w.ammo = w.maxAmmo;
                Debug.Log(type + " unlocked!");

                currentWeaponIndex = weapons.IndexOf(w);
                RefreshTrigger();
                CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
                UpdateWeaponVisibility();
            }
        }
    }

    void UpdateWeaponVisibility()
    {
        pistolObject.SetActive(CurrentWeapon.type == WeaponType.Pistol);
        machinePistolObject.SetActive(CurrentWeapon.type == WeaponType.MachinePistol);
        shotgunObject.SetActive(CurrentWeapon.type == WeaponType.Shotgun);
    }

    // ──────────────────────────────────────────
    //  PUBLIC: GIVE AMMO
    // ──────────────────────────────────────────
    public void GiveAmmo(int amount, GameObject pickup)
    {
        if (CurrentWeapon.ammo >= CurrentWeapon.maxAmmo)
        {
            Debug.Log("Ammo already full!");
            return;
        }

        CurrentWeapon.ammo = Mathf.Min(CurrentWeapon.ammo + amount, CurrentWeapon.maxAmmo);
        Destroy(pickup);
        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
    }

    // ──────────────────────────────────────────
    //  TRIGGER ZONE
    // ──────────────────────────────────────────
    void RefreshTrigger()
    {
        if (gunTrigger == null) return;
        float r = CurrentWeapon.range;
        gunTrigger.size = new Vector3(r, r, r);
        gunTrigger.center = new Vector3(0, r / 2f, r / 2f);
        UpdateWeaponVisibility();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e) enemyManager.AddEnemy(e);
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e) enemyManager.RemoveEnemy(e);
    }
}