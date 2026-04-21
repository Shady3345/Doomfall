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
        audioSource.volume = 0.3f;
    }

    private void FixedUpdate()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
            else
                return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        audioSource.volume = Mathf.Clamp01(1f / distance);

        if (!isAggro && distance < awarenessRadius)
        {
            isAggro = true;

            // 🔊 Play sound when player enters range
            if (!hasPlayedSound)
            {
                audioSource.PlayOneShot(aggroSound);
                hasPlayedSound = true;
            }
        }
        else if (isAggro && distance > chaseRadius)
        {
            isAggro = false;

            // 🔁 Reset so it can play again next time
            hasPlayedSound = false;
        }
    }
}