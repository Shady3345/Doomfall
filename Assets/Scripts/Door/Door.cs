using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject areaToSpawn;
    public bool requiresKey;
    public bool reqRed, reqGreen, reqBlue;

    private BoxCollider physicalBlocker;

    void Start()
    {
        foreach (BoxCollider bc in GetComponentsInChildren<BoxCollider>())
        {
            if (!bc.isTrigger) physicalBlocker = bc;
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

        // Guard: requiresKey must actually specify at least one colour
        bool anyKeySpecified = reqRed || reqGreen || reqBlue;
        if (!anyKeySpecified)
        {
            Debug.LogWarning("Door has requiresKey=true but no colour selected!");
            return;
        }

        bool hasRequired = (!reqRed || inv.hasRed) &&
                           (!reqGreen || inv.hasGreen) &&
                           (!reqBlue || inv.hasBlue);

        if (hasRequired)
            OpenDoor();
        else
            Debug.Log("Door locked - missing required key");

        Debug.Log($"inv null: {inv == null}");
        if (inv == null) return;

        Debug.Log($"requiresKey: {requiresKey}, hasBlue: {inv.hasBlue}, reqBlue: {reqBlue}");
    }

    private void OpenDoor()
    {
        areaToSpawn.SetActive(true);
        if (physicalBlocker != null)
            physicalBlocker.enabled = false;
    }
}