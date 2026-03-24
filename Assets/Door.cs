using UnityEngine;

public class Door : MonoBehaviour
{

    public GameObject areaToSpawn;

    public bool requiresKey;
    public bool reqRed, reqGreen, reqBlue;
    private void OnTriggerEnter(Collider other)
    {

        if (!requiresKey)
        {
            if(reqRed && other.GetComponent<PlayerInventory>().hasRed)
            {
                areaToSpawn.SetActive(true);
            }
            else if (reqGreen && other.GetComponent<PlayerInventory>().hasGreen)
            {
                areaToSpawn.SetActive(true);
            }
            else if (reqBlue && other.GetComponent<PlayerInventory>().hasBlue)
            {
                areaToSpawn.SetActive(true);
            }
        }
        else
        {
            areaToSpawn.SetActive(true);
        }

        if (other.CompareTag("Player"))
        {
            Door[] doors = FindObjectsByType<Door>(FindObjectsSortMode.None);

            areaToSpawn.SetActive(true);
        }
    }
}

