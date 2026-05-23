using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private int currentStamina;

    private void Awake()
    {
        // ここで初期化や他コンポーネントの参照取得などを行う
    }
    // プレイヤーのステータスは一括管理するため、他のスクリプトからアクセスしやすいようにプロパティを用意

}
