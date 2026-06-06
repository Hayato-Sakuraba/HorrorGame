using UnityEngine;
using UnityEngine.InputSystem;

public class OtakaraClick : MonoBehaviour
{
    public Otakara data;
    public Inventory inventory;
    public PopupManager popupManager;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray =
                Camera.main.ScreenPointToRay(
                    Mouse.current.position.ReadValue()
                );

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (inventory.AddItem(data))
                    {
                        // Popup•\Ž¦
                        popupManager.ShowPopup(data);

                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}