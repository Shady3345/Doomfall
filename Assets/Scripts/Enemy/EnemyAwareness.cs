using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public float awarenessRadius = 5f;   // detection range
    public float chaseRadius = 10f;      // how far it keeps chasing
    public Material aggroMat;
    public bool isAggro;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (!isAggro && distance < awarenessRadius)
        {
            isAggro = true;   // player entered detection range → aggro on
        }
        else if (isAggro && distance > chaseRadius)
        {
            isAggro = false;  // player ran far enough away → give up
        }
        // between awarenessRadius and chaseRadius: keep current state
    }
}