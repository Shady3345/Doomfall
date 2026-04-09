using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject areaToSpawn;

    public bool requiresKey;
    public bool reqRed, reqGreen, reqBlue;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        if (!requiresKey)
        {
            areaToSpawn.SetActive(true);
            return;
        }

        if (reqRed && inv.hasRed)
        {
            areaToSpawn.SetActive(true);
        }
        else if (reqGreen && inv.hasGreen)
        {
            areaToSpawn.SetActive(true);
        }
        else if (reqBlue && inv.hasBlue)
        {
            areaToSpawn.SetActive(true);
        }
        else
        {
            Debug.Log("Door locked");
        }
    }
}