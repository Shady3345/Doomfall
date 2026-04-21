using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Gun.WeaponType weaponType;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Gun gun = other.GetComponentInChildren<Gun>();

        if (gun != null)
        {
            gun.UnlockWeapon(weaponType);
            Destroy(gameObject);
        }
    }
}