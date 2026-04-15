using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 300;
    private int health;

    [Header("Attack")]
    public float attackRange = 3f;
    public float attackDamage = 20f;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private bool isDead = false;

    [Header("References")]
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

   // [Header("Room (optional)")]
   // public EnemyRoomController roomController;

    private void Start()
    {
        health = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null)
        {
            agent.stoppingDistance = attackRange * 0.8f;
        }
    }

    private void Update()
    {
        if (isDead || player == null || agent == null) return;
        if (!agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            agent.isStopped = true;

            if (animator != null)
                animator.SetBool("isWalking", false);

            Attack();
        }
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;

        if (animator != null)
            animator.SetTrigger("Attack");

        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.DamagePlayer((int)attackDamage);
            }
        }
    }

    // 💥 DAMAGE FUNCTION (call this from player)
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        Debug.Log("Boss HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Boss defeated!");

        agent.isStopped = true;

        if (animator != null)
            animator.SetTrigger("Die");

        // notify room
     //   if (roomController != null)
        {
    //        roomController.OnBossDefeated();
        }

        Destroy(gameObject, 3f);
    }
}