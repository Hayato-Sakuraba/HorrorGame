using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemId;
    public int count;
}

[System.Serializable]//所持アイテムとお金の保存先
public class InventorySaveData
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public int money;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventoryItem> items = new List<InventoryItem>();

    public int money = 0;

    private const string SaveKey = "InventorySave";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // アイテム追加
    public void AddItem(string itemId, int amount)
    {
        InventoryItem item = items.Find(x => x.itemId == itemId);

        if (item != null)
        {
            item.count += amount;
        }
        else
        {
            items.Add(new InventoryItem
            {
                itemId = itemId,
                count = amount
            });
        }

        SaveInventory();

        Debug.Log(itemId + " を取得");
    }

    // アイテム削除
    public bool RemoveItem(string itemId, int amount)
    {
        InventoryItem item = items.Find(x => x.itemId == itemId);

        if (item == null)
        {
            return false;
        }

        item.count -= amount;

        if (item.count <= 0)
        {
            items.Remove(item);
        }

        SaveInventory();

        return true;
    }

    // お金追加
    public void AddMoney(int amount)
    {
        money += amount;

        SaveInventory();

        Debug.Log("お金：" + money);
    }

    // セーブ
    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData();

        data.items = items;
        data.money = money;

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(SaveKey, json);

        PlayerPrefs.Save();
    }

    // ロード
    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            return;
        }

        string json = PlayerPrefs.GetString(SaveKey);

        InventorySaveData data =
            JsonUtility.FromJson<InventorySaveData>(json);

        items = data.items;
        money = data.money;
    }
}