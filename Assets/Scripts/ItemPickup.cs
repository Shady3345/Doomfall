using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool isHealth;
    public bool isAmmo;
    public bool isArmor;

    public int amount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   

            if (isHealth)
            {
                other.GetComponent<PlayerHealth>().GiveHealth(amount, this.gameObject); // Heal the player by 25 health points
                // Logic for health pickup
                Debug.Log("Player picked up health!");
            }

            if (isArmor)
            {
                Debug.Log("ARMOR PICKUP TRIGGERED");

                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.GiveArmor(amount, gameObject);
                    Debug.Log("Armor GIVEN");
                }
                else
                {
                    Debug.LogError("PlayerHealth NOT FOUND!");
                }
            }

            if (isAmmo)
            {
                other.GetComponentInChildren<Gun>().GiveAmmo(amount, this.gameObject);
                // Logic for armor pickup
                Debug.Log("Player picked up armor!");
           
            }            
        }
    }
}

