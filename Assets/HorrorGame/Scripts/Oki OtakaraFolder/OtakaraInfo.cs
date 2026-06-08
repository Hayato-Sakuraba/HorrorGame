using UnityEngine;

public class OtakaraInfo : MonoBehaviour
{
    [SerializeField] public Otakara otakara;

    public void Pickup(Inventory inventory)
    {
        inventory.AddItem(otakara);
        gameObject.SetActive(false);
    }
}
