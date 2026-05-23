using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConvertDebug : MonoBehaviour
{
    public Inventory inventory;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            inventory.ConvertToCredit();

        }
    }
}