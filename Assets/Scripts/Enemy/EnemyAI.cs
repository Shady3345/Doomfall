using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    private EnemyAwareness enemyAwareness;
    private NavMeshAgent agent;

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
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                return;
        }

        if (enemyAwareness == null) return;

        // Guard: dont call SetDestination if agent isnt on NavMesh yet
        if (!agent.isOnNavMesh) return;

        agent.isStopped = !enemyAwareness.isAggro;

        if (enemyAwareness.isAggro)
        {
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            agent.ResetPath();
        }
    }
}