//換金するときのスクリプト
using UnityEngine;
using UnityEngine.InputSystem;

public class SellPoint : MonoBehaviour
{
    [SerializeField]
    private string itemId = "Treasure";

    [SerializeField]
    private int sellPrice = 100;

    private bool canSell = false;

    private void Update()
    {
        if (canSell &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            bool result =
                InventoryManager.Instance.RemoveItem(itemId, 1);

            if (result)
            {
                InventoryManager.Instance.AddMoney(sellPrice);

                Debug.Log(itemId + " を換金");
            }
            else
            {
                Debug.Log("宝を持っていない");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSell = true;

            Debug.Log("Eキーで換金");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSell = false;
        }
    }
}