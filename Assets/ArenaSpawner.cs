using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawner : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public int enemyCount = 6;
    public Transform[] spawnPoints;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public float bossSpawnDelay = 2f;

    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool bossSpawned = false;

    void Start()
    {
        SpawnEnemies();
    }

    void Update()
    {
        // remove dead enemies
        aliveEnemies.RemoveAll(e => e == null);

        // spawn boss when all enemies are dead
        if (!bossSpawned && aliveEnemies.Count == 0)
        {
            bossSpawned = true;
            Invoke(nameof(SpawnBoss), bossSpawnDelay);
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy = Instantiate(prefab, spawn.position, Quaternion.identity);
            aliveEnemies.Add(enemy);
        }
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

        Boss bossScript = boss.GetComponent<Boss>();

        if (BossUI.Instance != null && bossScript != null)
        {
            BossUI.Instance.ShowBoss(bossScript.bossName, bossScript.maxHealth);
        }

        Debug.Log("Boss spawned!");
    }
}