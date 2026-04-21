using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public float awarenessRadius = 5f;
    public float chaseRadius = 10f;

    public bool isAggro;

    private Transform playerTransform;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip aggroSound;

    private bool hasPlayedSound = false;

    void Start()
    {
        // lower default volume
        audioSource.volume = 0.3f;
    }

    private void FixedUpdate()
    {
        // find player if needed
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                playerTransform = player.transform;
            else
                return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // adjust volume based on distance
        audioSource.volume = Mathf.Clamp01(1f / distance);

        // enter aggro
        if (!isAggro && distance < awarenessRadius)
        {
            isAggro = true;

            // play sound once
            if (!hasPlayedSound)
            {
                audioSource.PlayOneShot(aggroSound);
                hasPlayedSound = true;
            }
        }
        // leave aggro
        else if (isAggro && distance > chaseRadius)
        {
            isAggro = false;

            // reset sound trigger
            hasPlayedSound = false;
        }
    }
}