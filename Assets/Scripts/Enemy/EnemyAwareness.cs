using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public float awarenessRadius = 5f;
    public float chaseRadius = 10f;
    public bool isAggro;
    private Transform playerTransform;

    private void FixedUpdate()
    {
        // Lazy find - waits until player is fully initialized
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                return; // player not spawned yet, wait
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (!isAggro && distance < awarenessRadius)
        {
            isAggro = true;
        }
        else if (isAggro && distance > chaseRadius)
        {
            isAggro = false;
        }
    }
}