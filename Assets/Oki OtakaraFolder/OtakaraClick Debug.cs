using UnityEngine;

public class OtakaraClick : MonoBehaviour
{
    public Otakara data;
    public Inventory inventory;

    void OnMouseDown()
    {
        // 容量チェック
        if (inventory.AddItem(data))
        {
            Debug.Log("クリックで取得");
            Destroy(gameObject);
        }
        else
        {
            // 容量オーバー時

            Debug.Log("容量オーバーで拾えない");
        }
    }
}