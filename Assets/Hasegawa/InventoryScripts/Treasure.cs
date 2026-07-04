// 宝を拾った時のスクリプト
using UnityEngine;
using UnityEngine.InputSystem;

public class Treasure : MonoBehaviour
{
    // お宝データ
    public Otakara data;

    // インベントリ
    public Inventory inventory;

    // ポップアップ表示（完成したら使用）
    // public PopupManager popupManager;

    // 自分のインベントリ用
    [SerializeField] private string itemId = "Treasure";

    // 取得数
    [SerializeField] private int amount = 1;

    // プレイヤーがお宝を拾える範囲にいるか
    private bool canPickup = false;

    private void Update()
    {
        // Eキーを押したらお宝を拾う
        if (canPickup && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Pickup();
        }
    }

    // お宝を拾う処理
    private void Pickup()
    {
        // 自分のインベントリ
        InventoryManager.Instance.AddItem(itemId, amount);

        // チーム側インベントリ
        if (inventory != null && data != null)
        {
            inventory.AddItem(data);
        }

        /*
        // ポップアップ表示（完成したら使用）
        if (popupManager != null && data != null)
        {
            popupManager.ShowPopup(data);
        }
        */

        // お宝を削除
        Destroy(gameObject);
    }

    // プレイヤーがお宝に近づいた時
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;

            Debug.Log("Eキーで拾う");
        }
    }

    // プレイヤーがお宝から離れた時
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }
}