using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public bool isRedkey, isBlueKey, isGreenKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isRedkey)
            {
                other.GetComponent<PlayerInventory>().hasRed = true;
                CanvasManager.Instance.UpdateKeys("red");
            }
            else if (isGreenKey)
            {
                other.GetComponent<PlayerInventory>().hasGreen = true;
                CanvasManager.Instance.UpdateKeys("green");
            }
            else if (isBlueKey)
            {
                other.GetComponent<PlayerInventory>().hasBlue = true;
                CanvasManager.Instance.UpdateKeys("blue");
            }
            Destroy(gameObject);
        }
    }
}

