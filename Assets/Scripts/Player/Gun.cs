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

    public Transform playerCamera;

    public LayerMask raycastLayerMask; // what raycast can hit
    public LayerMask enemyLayerMask;   // used for enemy detection

    public EnemyManager enemyManager;

    public float gunShotRadius = 10f; // radius to alert enemies

    [Header("Weapon Models")]
    public GameObject pistolObject;
    public GameObject machinePistolObject;
    public GameObject shotgunObject;

    [Header("UI Weapon Icons")]
    public GameObject uiPistol;
    public GameObject uiShotgun;
    public GameObject uiMachinePistol;

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
        new Weapon { type = WeaponType.Pistol, damage = 25f, range = 30f, fireRate = 2f, maxAmmo = 30, ammo = 0, unlocked = false },
        new Weapon { type = WeaponType.MachinePistol, damage = 12f, range = 20f, fireRate = 10f, maxAmmo = 100, ammo = 0, unlocked = false },
        new Weapon { type = WeaponType.Shotgun, damage = 15f, range = 15f, fireRate = 1f, maxAmmo = 50, ammo = 0, unlocked = false },
    };

    public int currentWeaponIndex = 0;

    private float nextTimeToFire = 0f;
    private AudioSource audioSource;

    Weapon CurrentWeapon => weapons[currentWeaponIndex];

    void Start()
    {
        // reset all ammo
        foreach (var w in weapons) w.ammo = 0;

        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);

        UpdateWeaponVisibility();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleWeaponSwitch();
        HandleFire();
        HandleMelee();
        UpdateEnemyList();
    }

    void UpdateEnemyList()
    {
        // get enemies in range of weapon
        float r = CurrentWeapon.range;

        Collider[] hits = Physics.OverlapSphere(transform.position, r, enemyLayerMask);

        enemyManager.ClearEnemies();

        foreach (var col in hits)
        {
            Enemy e = col.GetComponent<Enemy>();
            if (e != null)
                enemyManager.AddEnemy(e);
        }
    }

    void HandleWeaponSwitch()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) TrySwitchTo(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) TrySwitchTo(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) TrySwitchTo(2);
    }

    void TrySwitchTo(int index)
    {
        if (index >= weapons.Count) return;

        // can't switch if not unlocked
        if (!weapons[index].unlocked)
        {
            Debug.Log(weapons[index].type + " not unlocked yet!");
            return;
        }

        if (index == currentWeaponIndex) return;

        currentWeaponIndex = index;

        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
        UpdateWeaponVisibility();
    }

    void HandleFire()
    {
        // automatic weapon uses hold, others single click
        bool triggerPressed =
            CurrentWeapon.type == WeaponType.MachinePistol
            ? Mouse.current.leftButton.isPressed
            : Mouse.current.leftButton.wasPressedThisFrame;

        if (triggerPressed && Time.time > nextTimeToFire && CurrentWeapon.ammo > 0)
            Fire();
    }

    void Fire()
    {
        // play sound
        switch (CurrentWeapon.type)
        {
            case WeaponType.Pistol:
                if (pistolSound) audioSource.PlayOneShot(pistolSound);
                break;

            case WeaponType.MachinePistol:
                if (machinePistolSound) audioSource.PlayOneShot(machinePistolSound);
                break;

            case WeaponType.Shotgun:
                if (shotgunSound) audioSource.PlayOneShot(shotgunSound);
                break;
        }

        // muzzle flash
        switch (CurrentWeapon.type)
        {
            case WeaponType.Pistol:
                if (pistolMuzzleFlash) pistolMuzzleFlash.Play();
                break;

            case WeaponType.MachinePistol:
                if (machinePistolMuzzleFlash) machinePistolMuzzleFlash.Play();
                break;

            case WeaponType.Shotgun:
                if (shotgunMuzzleFlash) shotgunMuzzleFlash.Play();
                break;
        }

        // alert enemies nearby
        foreach (var col in Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask))
        {
            EnemyAwareness a = col.GetComponent<EnemyAwareness>();
            if (a) a.isAggro = true;
        }

        // shoot logic
        if (CurrentWeapon.type == WeaponType.Shotgun)
            ShotgunBlast();
        else
            SingleRaycast(CurrentWeapon.damage, CurrentWeapon.range);

        nextTimeToFire = Time.time + 1f / CurrentWeapon.fireRate;

        CurrentWeapon.ammo--;

        CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
    }

    void SingleRaycast(float damage, float range)
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, range, raycastLayerMask))
        {
            // enemy hit
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                return;
            }

            // boss hit
            Boss boss = hit.collider.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage((int)damage);
            }
        }
    }

    void ShotgunBlast()
    {
        // multiple rays for spread
        for (int i = 0; i < 6; i++)
        {
            Vector3 spread = playerCamera.forward + new Vector3(
                Random.Range(-0.08f, 0.08f),
                Random.Range(-0.08f, 0.08f),
                0f);

            if (Physics.Raycast(playerCamera.position, spread, out RaycastHit hit, CurrentWeapon.range))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(CurrentWeapon.damage);
                    continue;
                }

                Boss boss = hit.collider.GetComponent<Boss>();
                if (boss != null)
                    boss.TakeDamage((int)CurrentWeapon.damage);
            }
        }
    }

    void HandleMelee()
    {
        // knife attack
        if (!Keyboard.current.vKey.wasPressedThisFrame) return;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, 2f))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(40f);
                return;
            }

            Boss boss = hit.collider.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(40);
            }
        }
    }

    void UpdateWeaponVisibility()
    {
        // enable only current weapon model
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

    public void UnlockWeapon(WeaponType type)
    {
        foreach (var w in weapons)
        {
            if (w.type == type && !w.unlocked)
            {
                w.unlocked = true;
                w.ammo = w.maxAmmo;

                currentWeaponIndex = weapons.IndexOf(w);

                CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
                UpdateWeaponVisibility();
            }
        }
    }

    public void GiveAmmo(int amount, WeaponType targetType, GameObject pickup)
    {
        Weapon target = weapons.Find(w => w.type == targetType);
        if (target == null) return;

        target.ammo = Mathf.Min(target.ammo + amount, target.maxAmmo);

        Destroy(pickup);

        if (target.type == CurrentWeapon.type)
            CanvasManager.Instance.UpdateAmmo(CurrentWeapon.ammo);
    }
}