using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasRed, hasBlue, hasGreen;

    private void Start()
    {
        // reset keys UI
        CanvasManager.Instance.ClearKeys();
    }
}