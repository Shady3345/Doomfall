using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public int enemyCount = 6;

    [Header("Spawn Circle")]
    public float spawnRadius = 10f;
    public int spawnPointCount = 8;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public float bossSpawnDelay = 2f;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    private bool bossSpawned = false;
    private bool enemiesSpawned = false;

    private Vector3[] circleSpawnPoints;

    void Start()
    {
        GenerateCircleSpawnPoints();
        SpawnEnemies();
    }

    void GenerateCircleSpawnPoints()
    {
        // create evenly spaced points in a circle
        circleSpawnPoints = new Vector3[spawnPointCount];

        for (int i = 0; i < spawnPointCount; i++)
        {
            float angle = i * (360f / spawnPointCount) * Mathf.Deg2Rad;

            float x = transform.position.x + Mathf.Cos(angle) * spawnRadius;
            float z = transform.position.z + Mathf.Sin(angle) * spawnRadius;

            circleSpawnPoints[i] = new Vector3(x, transform.position.y, z);
        }
    }

    void Update()
    {
        // remove dead enemies from list
        aliveEnemies.RemoveAll(e => e == null);

        // if all enemies are dead → spawn boss
        if (enemiesSpawned && !bossSpawned && aliveEnemies.Count == 0)
        {
            bossSpawned = true;
            Invoke(nameof(SpawnBoss), bossSpawnDelay);
        }
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("No enemy prefabs assigned!");
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            // pick random spawn point
            Vector3 spawnPos = circleSpawnPoints[Random.Range(0, circleSpawnPoints.Length)];

            // pick random enemy
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            aliveEnemies.Add(enemy);
        }

        enemiesSpawned = true;
    }

    void SpawnBoss()
    {
        if (bossSpawnPoint == null)
        {
            Debug.LogError("BossSpawnPoint not assigned!");
            return;
        }

        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

        Boss bossScript = boss.GetComponent<Boss>();

        // show boss UI
        if (BossUI.Instance != null && bossScript != null)
        {
            BossUI.Instance.ShowBoss(bossScript.bossName, bossScript.maxHealth);
        }

        Debug.Log("Boss spawned!");
    }

    // draw spawn points in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < spawnPointCount; i++)
        {
            float angle = i * (360f / spawnPointCount) * Mathf.Deg2Rad;

            float x = transform.position.x + Mathf.Cos(angle) * spawnRadius;
            float z = transform.position.z + Mathf.Sin(angle) * spawnRadius;

            Gizmos.DrawSphere(new Vector3(x, transform.position.y, z), 0.5f);
        }
    }
}