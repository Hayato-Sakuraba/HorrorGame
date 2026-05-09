using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
//容量
    public int maxSize = 100;
    public int currentSize = 0;
//所持アイテム
    public List<Otakara> items = new List<Otakara>();
//クレジット
    public CreditManager creditManager;


    public bool AddItem(Otakara item)
    {
        if (currentSize + item.guram > maxSize)
        {
            Debug.Log("容量オーバー！");
            return false;
        }

        items.Add(item);
        currentSize += item.guram;
        return true;
    }

    // 合計計算
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

        // リセット（また拾える）
        items.Clear();
        currentSize = 0;

        Debug.Log("変換完了: " + total);
    }
}