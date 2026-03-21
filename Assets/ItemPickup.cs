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
                other.GetComponent<PlayerHealth>().GiveArmor(amount, this.gameObject);
                // Logic for ammo pickup
                Debug.Log("Player picked up ammo!");
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

