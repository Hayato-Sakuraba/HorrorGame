//宝を拾った時のスクリプト
using UnityEngine;
using UnityEngine.InputSystem;

public class Treasure : MonoBehaviour
{
    [SerializeField]
    private string itemId = "Treasure";

    [SerializeField]
    private int amount = 1;//インベントリ容量

    private bool canPickup = false;

    private void Update()
    {
        if (canPickup &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            InventoryManager.Instance.AddItem(itemId, amount);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;

            Debug.Log("Eキーで拾う");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }
}