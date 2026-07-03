using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    //容量
    public int maxSize = 100;
    public int currentSize = 0;

    //所持アイテム
    public List<Otakara> items = new List<Otakara>();

    //クレジット
    public CreditManager creditManager;

    //枠UI
    public TextMeshProUGUI capacityText;

    //インベントリ表示UI
    public InventoryUI inventoryUI;

    //デバッグ用お宝
    public Otakara testItem;

    private void Start()
    {
        UpdateUI();

        if (inventoryUI != null)
        {
            inventoryUI.RefreshUI();
        }

        if (testItem != null)
        {
            AddItem(testItem);
        }
    }

    //アイテム追加
    public bool AddItem(Otakara item)
    {
        if (currentSize + item.guram > maxSize)
        {
            Debug.Log("容量オーバー！");
            return false;
        }

        items.Add(item);
        currentSize += item.guram;

        UpdateUI();

        if (inventoryUI != null)
        {
            inventoryUI.RefreshUI();
        }

        return true;
    }

    //合計金額
    public int GetTotalPrice()
    {
        int total = 0;

        foreach (var item in items)
        {
            total += item.currentPrice;
        }

        return total;
    }

    //換金
    public void ConvertToCredit()
    {
        int total = GetTotalPrice();

        if (total <= 0)
        {
            Debug.Log("アイテムがない");
            return;
        }

        creditManager.AddCredit(total);

        items.Clear();
        currentSize = 0;

        UpdateUI();

        if (inventoryUI != null)
        {
            inventoryUI.RefreshUI();
        }

        Debug.Log("変換完了 : " + total);
    }

    //ランダム破壊
    public void DestroyRandomItem()
    {
        if (items.Count <= 0)
        {
            Debug.Log("壊すアイテムなし");
            return;
        }

        Otakara brokenItem =
            items[Random.Range(0, items.Count)];

        currentSize -= brokenItem.guram;

        items.Remove(brokenItem);

        UpdateUI();

        if (inventoryUI != null)
        {
            inventoryUI.RefreshUI();
        }

        Debug.Log(brokenItem.itemName + " が壊れた！");
    }

    //容量表示更新
    void UpdateUI()
    {
        if (capacityText != null)
        {
            capacityText.text =
                "枠 : " +
                currentSize +
                " / " +
                maxSize;
        }
    }
    public void RemoveItem(Otakara item)
    {
        if (!items.Contains(item))
            return;

        currentSize -= item.guram;

        items.Remove(item);

        UpdateUI();

        if (inventoryUI != null)
        {
            inventoryUI.RefreshUI();
        }
    }
}