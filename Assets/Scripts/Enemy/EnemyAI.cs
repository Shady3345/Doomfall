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

<<<<<<< HEAD

=======
>>>>>>> saved-work
    private void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        agent = GetComponent<NavMeshAgent>();
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
            }
            else
            {
                agent.isStopped = true;
                Attack();
            }
        }
        else
        {
            agent.isStopped = true;
<<<<<<< HEAD
            agent.ResetPath();
=======
            agent.ResetPath();  
>>>>>>> saved-work
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