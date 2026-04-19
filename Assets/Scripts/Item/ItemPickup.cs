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

        if (isHealth)
        {
            other.GetComponent<PlayerHealth>().GiveHealth(amount, this.gameObject);
            Debug.Log("Player picked up health!");
        }

        if (isArmor)
        {
            other.GetComponent<PlayerHealth>().GiveArmor(amount, this.gameObject);
            Debug.Log("Player picked up armor!");
        }

        if (isAmmo)
        {
            Gun gun = other.GetComponentInChildren<Gun>();
            if (gun != null)
                gun.GiveAmmo(amount, ammoWeaponType, this.gameObject);
            Debug.Log("Player picked up ammo!");
        }
    }
}