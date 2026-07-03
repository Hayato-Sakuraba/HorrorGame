using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPanelController : MonoBehaviour
{
    public static bool IsInventoryOpen;

    public GameObject inventoryPanel;

    private bool isOpen = false;

    private void Start()
    {
        inventoryPanel.SetActive(false);

        IsInventoryOpen = false;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        if (isOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        IsInventoryOpen = isOpen;

        inventoryPanel.SetActive(isOpen);

        if (isOpen)
        {
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}