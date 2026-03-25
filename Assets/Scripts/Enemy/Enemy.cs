using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Animator spriteAnim;
    private AngleToPlayer angleToPlayer;
    private EnemyManager enemyManager;
    private float enemyHealth = 2f;

    public GameObject gunHitEfect;


    private void Start()
    {
        spriteAnim = GetComponentInChildren<Animator>();
        angleToPlayer = GetComponent<AngleToPlayer>();

        enemyManager = FindAnyObjectByType<EnemyManager>();
    }

    void Update()
    {
        if (enemyHealth <= 0)
        {
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
        }
    } 

    public void TakeDamage(float damage)
    {
        Instantiate(original: gunHitEfect, position: transform.position, rotation: Quaternion.identity);
        enemyHealth -= damage;
        Debug.Log("Enemy took damage! Remaining health: " + enemyHealth);
    }

}
