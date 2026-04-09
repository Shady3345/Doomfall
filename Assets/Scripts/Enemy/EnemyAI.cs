using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    private EnemyAwareness enemyAwareness;
    private NavMeshAgent agent;

    [Header("Settings")]
    public float stoppingDistance = 0.5f;

    private void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();
        agent = GetComponent<NavMeshAgent>();

        // Falls du vergessen hast den Player zu setzen
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player not found! Set tag 'Player' or assign manually.");
            }
        }

        agent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        if (playerTransform == null || enemyAwareness == null) return;

        if (enemyAwareness.isAggro)
        {
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            agent.ResetPath();
        }
        Debug.Log($"isAggro: {enemyAwareness.isAggro} | hasPath: {agent.hasPath} | pathStatus: {agent.pathStatus} | remainingDist: {agent.remainingDistance}");
    }
}