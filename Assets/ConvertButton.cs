using UnityEngine;

public class ConvertButton : MonoBehaviour
{
    public Inventory inventory;

    public void OnClickConvert()
    {
        inventory.ConvertToCredit();

    }
}