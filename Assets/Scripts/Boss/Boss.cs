using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [Header("Info")]
    public string bossName = "Machine Lord";

    [Header("Stats")]
    public int maxHealth = 300;
    private int health;

    [Header("Attack")]
    public float attackRange = 3f;
    public float attackDamage = 20f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    private bool isDead = false;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyAwareness enemyAwareness;

    private void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAwareness = GetComponent<EnemyAwareness>();

        if (agent != null)
            agent.stoppingDistance = attackRange * 0.8f;

        // BossUI is shown by ArenaSpawner (or whoever spawns this boss)
    }

    private void Update()
    {
        if (isDead || player == null || agent == null) return;
        if (!agent.isOnNavMesh) return;

        // Respect awareness — only chase if aggro
        if (enemyAwareness != null && !enemyAwareness.isAggro)
        {
            agent.isStopped = true;
            if (animator != null) animator.SetBool("isWalking", false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            if (animator != null) animator.SetBool("isWalking", true);
        }
        else
        {
            agent.isStopped = true;
            if (animator != null) animator.SetBool("isWalking", false);
            Attack();
        }
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;

        if (animator != null) animator.SetTrigger("Attack");

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null) ph.DamagePlayer((int)attackDamage);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        // Getting shot makes the boss aggro immediately
        if (enemyAwareness != null) enemyAwareness.isAggro = true;

        if (BossUI.Instance != null) BossUI.Instance.UpdateHealth(health);

        if (health <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (agent != null) agent.isStopped = true;
        if (animator != null) animator.SetTrigger("Die");
        if (BossUI.Instance != null) BossUI.Instance.HideBoss();

        Destroy(gameObject, 3f);
    }
}