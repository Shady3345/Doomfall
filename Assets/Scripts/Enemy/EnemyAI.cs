using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [Header("Attack")]
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private float lastAttackTime;
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    private EnemyAwareness enemyAwareness;
    private NavMeshAgent agent;

    private Animator animator;

    private void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // or GetComponentInChildren if mesh is a child
        agent.stoppingDistance = 0f;

    }

    private void Update()
    {
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
            else return;
        }

        if (enemyAwareness == null) return;
        if (!agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (enemyAwareness.isAggro)
        {
            if (distance > attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(playerTransform.position);
                animator.SetBool("isWalking", true);  // ✅ walk animation
                animator.ResetTrigger("Attack");
            }
            else
            {
                agent.isStopped = true;
                animator.SetBool("isWalking", false); // ✅ stop walk
                Attack();
            }
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            animator.SetBool("isWalking", false);     
        }
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;

       
        PlayerHealth player = playerTransform.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.DamagePlayer((int)attackDamage);
        }
    }

}