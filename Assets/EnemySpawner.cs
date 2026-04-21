using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] enemyPrefabs; // Assign up to 4 (or more) enemy prefabs here

    [Header("Spawning")]
    public int maxEnemies = 10;
    public float spawnInterval = 3f;
    public float spawnHeight = 1f;

    [Header("Spawn Area")]
    public float spawnRangeX = 20f;
    public float spawnRangeZ = 20f;

    [Header("Player")]
    public float minDistanceFromPlayer = 5f;

    private Transform player;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private float timer = 0f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        // Clean up destroyed enemies from list
        spawnedEnemies.RemoveAll(e => e == null);

        if (spawnedEnemies.Count >= maxEnemies) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnEnemy();
        }
    }

    void TrySpawnEnemy()
    {
        // Nothing to spawn if no prefabs assigned
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned to EnemySpawner!");
            return;
        }

        // Try up to 10 times to find a valid spawn point
        for (int i = 0; i < 10; i++)
        {
            // Pick a random XZ position within the spawn area, cast from above
            Vector3 randomPoint = new Vector3(
                transform.position.x + Random.Range(-spawnRangeX, spawnRangeX),
                transform.position.y + 10f,
                transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ)
            );

            // Raycast down to find the floor surface
            if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, 30f))
            {
                // Only land on flat floor — skip walls by checking the surface normal
                // Floor normals point upward (Y close to 1); walls have Y near 0
                if (hit.normal.y < 0.7f) continue;

                Vector3 spawnPoint = hit.point + Vector3.up * spawnHeight;

                // Make sure it's not too close to the player
                if (player != null)
                {
                    float distToPlayer = Vector3.Distance(spawnPoint, player.position);
                    if (distToPlayer < minDistanceFromPlayer) continue;
                }

                // Pick a random enemy prefab from the array
                GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                GameObject enemy = Instantiate(prefabToSpawn, spawnPoint, Quaternion.identity);
                spawnedEnemies.Add(enemy);
                Debug.Log($"Spawned {prefabToSpawn.name} on floor at: {spawnPoint}");
                return;
            }
        }

        Debug.Log("Could not find valid spawn point on floor!");
    }

    // Draw spawn area in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRangeX * 2, 1f, spawnRangeZ * 2));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromPlayer);
    }
}
