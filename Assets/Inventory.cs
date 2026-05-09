using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("容量")]
    public int maxSize = 100;
    public int currentSize = 0;

    [Header("所持アイテム")]
    public List<Otakara> items = new List<Otakara>();

    [Header("クレジット")]
    public CreditManager creditManager;

    // 拾う
    public bool AddItem(Otakara item)
    {
        if (currentSize + item.guram > maxSize)
        {
            Debug.Log("容量オーバー！");
            return false;
        }

        items.Add(item);
        currentSize += item.guram;

        Debug.Log("取得: 価値 " + item.price + " / 重さ " + item.guram);
        return true;
    }

    // 合計価値
    public int GetTotalPrice()
    {
        int total = 0;

        foreach (var item in items)
        {
            total += item.price;
        }

        return total;
    }

    // クレジット変換
    public void ConvertToCredit()
    {
        int total = GetTotalPrice();

        if (total <= 0)
        {
            Debug.Log("アイテムがない");
            return;
        }

        creditManager.AddCredit(total);

        Debug.Log("変換: " + total);

        items.Clear();
        currentSize = 0;
    }

    // デバッグ確認
    public void DebugInventory()
    {
        Debug.Log("==== インベントリ ====");
        foreach (var item in items)
        {
            Debug.Log("価値:" + item.price + " / 重さ:" + item.guram);
        }
        Debug.Log("容量: " + currentSize + "/" + maxSize);
    }
}
