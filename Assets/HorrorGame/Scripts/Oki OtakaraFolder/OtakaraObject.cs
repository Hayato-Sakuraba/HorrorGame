using UnityEngine;

public class OtakaraObject : MonoBehaviour
{
    public Otakara data;

    // E‚í‚ê‚éˆ—
    public bool TryPickup(Inventory inventory)
    {
        if (inventory.AddItem(data))
        {
            gameObject.SetActive(false);

            return true;
        }

        return false;
    }
}