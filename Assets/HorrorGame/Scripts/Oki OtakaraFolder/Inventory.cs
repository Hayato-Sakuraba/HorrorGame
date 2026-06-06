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

    // デバッグ用お宝
    public Otakara testItem;

    private void Start()
    {
        UpdateUI();

        if (testItem != null)
        {
            AddItem(testItem);
        }
    }

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

        return true;
    }

    // 合計計算
    public int GetTotalPrice()
    {
        int total = 0;

        foreach (var item in items)
        {
            total += item.currentPrice;
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

        // リセット
        items.Clear();
        currentSize = 0;

        UpdateUI();

        Debug.Log("変換完了: " + total);
    }

    // ランダムなお宝破壊
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

        Debug.Log(brokenItem.name + " が壊れた！");
    }

    
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
}