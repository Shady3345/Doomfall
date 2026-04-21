using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject areaToSpawn;

    public bool requiresKey;

    public bool reqRed, reqGreen, reqBlue;

    private BoxCollider physicalBlocker;

    void Start()
    {
        // find solid collider
        foreach (BoxCollider bc in GetComponentsInChildren<BoxCollider>())
        {
            if (!bc.isTrigger)
                physicalBlocker = bc;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        if (!requiresKey)
        {
            OpenDoor();
            return;
        }

        // check keys
        bool hasRequired =
            (!reqRed || inv.hasRed) &&
            (!reqGreen || inv.hasGreen) &&
            (!reqBlue || inv.hasBlue);

        if (hasRequired)
            OpenDoor();
        else
            Debug.Log("Door locked");
    }

    void OpenDoor()
    {
        // enable next area
        areaToSpawn.SetActive(true);

        // disable collider so player can pass
        if (physicalBlocker != null)
            physicalBlocker.enabled = false;
    }
}