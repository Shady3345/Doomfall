using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    private Transform target;
    void Start()
    {
        target = FindFirstObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
