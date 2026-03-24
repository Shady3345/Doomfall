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
            }
            else if (isGreenKey)
            {
                other.GetComponent<PlayerInventory>().hasGreen = true;
            }
            else if (isBlueKey)
            {
                other.GetComponent<PlayerInventory>().hasBlue = true;
            }
            Destroy(gameObject);
        }
    }
}

