using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [Header("Info")]
    public string bossName = "Machine Lord"; // name shown in boss UI

    [Header("Stats")]
    public int maxHealth = 300;
    private int health; // current health

    [Header("Attack")]
    public float attackRange = 3f;       // how close player needs to be
    public float attackDamage = 20f;     // damage per hit
    public float attackCooldown = 1.5f;  // time between attacks

    private float lastAttackTime; // last time boss attacked
    private bool isDead = false;  // prevent double death

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyAwareness enemyAwareness;

    private void Start()
    {
        health = maxHealth;

        // find player by tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // get required components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAwareness = GetComponent<EnemyAwareness>();

        // stop a bit before reaching player so it doesn't overlap
        if (agent != null)
            agent.stoppingDistance = attackRange * 0.8f;

        // boss UI gets enabled by spawner, not here
    }

    private void Update()
    {
        // stop logic if dead or missing stuff
        if (isDead || player == null || agent == null) return;

        // safety check (important for navmesh)
        if (!agent.isOnNavMesh) return;

        // only move if boss is aggro
        if (enemyAwareness != null && !enemyAwareness.isAggro)
        {
            agent.isStopped = true;

            if (animator != null)
                animator.SetBool("isWalking", false);

            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // move towards player
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            // stop and attack when close enough
            agent.isStopped = true;

            if (animator != null)
                animator.SetBool("isWalking", false);

            Attack();
        }
    }

    void Attack()
    {
        // check cooldown so boss doesn't spam attacks
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        // trigger attack animation
        if (animator != null)
            animator.SetTrigger("Attack");

        // deal damage to player
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.DamagePlayer((int)attackDamage);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // reduce health
        health -= damage;

        // clamp so it doesn't go below 0
        health = Mathf.Clamp(health, 0, maxHealth);

        // getting hit forces aggro
        if (enemyAwareness != null)
            enemyAwareness.isAggro = true;

        // update boss UI
        if (BossUI.Instance != null)
            BossUI.Instance.UpdateHealth(health);

        // check death
        if (health <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        // stop movement
        if (agent != null)
            agent.isStopped = true;

        // play death animation
        if (animator != null)
            animator.SetTrigger("Die");

        // hide boss UI
        if (BossUI.Instance != null)
            BossUI.Instance.HideBoss();

        // destroy after delay (so animation can play)
        Destroy(gameObject, 3f);
    }
}