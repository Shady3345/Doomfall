using UnityEngine;

public class Door : MonoBehaviour
{

    public GameObject areaToSpawn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Door[] doors = FindObjectsByType<Door>(FindObjectsSortMode.None);

            areaToSpawn.SetActive(true);
        }
    }
}

