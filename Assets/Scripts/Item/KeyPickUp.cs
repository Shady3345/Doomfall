using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public bool isRedkey, isBlueKey, isGreenKey;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();

        if (isRedkey)
        {
            inv.hasRed = true;
            CanvasManager.Instance.UpdateKeys("red");
        }
        else if (isGreenKey)
        {
            inv.hasGreen = true;
            CanvasManager.Instance.UpdateKeys("green");
        }
        else if (isBlueKey)
        {
            inv.hasBlue = true;
            CanvasManager.Instance.UpdateKeys("blue");
        }

        Destroy(gameObject);
    }
}