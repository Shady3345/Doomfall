using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool isHealth;
    public bool isAmmo;
    public bool isArmor;

    public int amount;

    public Gun.WeaponType ammoWeaponType;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // health pickup
        if (isHealth)
            other.GetComponent<PlayerHealth>().GiveHealth(amount, gameObject);

        // armor pickup
        if (isArmor)
            other.GetComponent<PlayerHealth>().GiveArmor(amount, gameObject);

        // ammo pickup
        if (isAmmo)
        {
            Gun gun = other.GetComponentInChildren<Gun>();
            if (gun != null)
                gun.GiveAmmo(amount, ammoWeaponType, gameObject);
        }
    }
}