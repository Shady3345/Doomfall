using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator spriteAnim;
    private AngleToPlayer angleToPlayer;
    private EnemyManager enemyManager;

    private float enemyHealth = 2f;

    public GameObject gunHitEffect;

    private void Start()
    {
        spriteAnim = GetComponentInChildren<Animator>();
        angleToPlayer = GetComponent<AngleToPlayer>();

        // find manager in scene
        enemyManager = FindAnyObjectByType<EnemyManager>();
    }

    void Update()
    {
        // check death
        if (enemyHealth <= 0)
        {
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        // spawn hit effect
        Instantiate(gunHitEffect, transform.position, Quaternion.identity);

        enemyHealth -= damage;

        Debug.Log("Enemy took damage! Remaining health: " + enemyHealth);
    }
}