using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{

    public float awarenessRadius = 3f;
    public Material aggroMat;
    public bool isAggro;
    private Transform playerTransform;
     
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        var distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance < awarenessRadius)
        {
           isAggro = true;
        }

    }

    private void Update()
    {
        if (isAggro)
        {
            GetComponent <MeshRenderer>().material = aggroMat;  
        }
    }  
}
