using System.Collections;
using System.Collections.Generic;
using System.IO.Hashing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public float range = 20f;
    public float verticalRange = 20f;
    public float fireRate = 1f;
    public float damage = 2f;

    private float nextTimeToFire = 0f;
    private BoxCollider gunTrigger;

    
    public Transform playerCamera;
    public LayerMask raycastLayerMask;
    public EnemyManager enemyManager;
    private void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(range, verticalRange, range);
        gunTrigger.center = new Vector3(0, verticalRange / 2, range / 2);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time > nextTimeToFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range, raycastLayerMask))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                float distanceToEnemy = hit.distance;

                float finalDamage = 2f * (1 - (distanceToEnemy / range));
                finalDamage = Mathf.Clamp(finalDamage, 0.5f, 2f);

                enemy.TakeDamage(finalDamage);

                Debug.Log("Hit enemy!");
            }
        }

        nextTimeToFire = Time.time + 1f / fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
       Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemyManager.RemoveEnemy(enemy);
            Debug.Log("Enemy entered the gun's range!");
        }
    }
}
