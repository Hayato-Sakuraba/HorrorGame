using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    [Header("ワープ先の地点")]
    [SerializeField] private Transform destination; 

    // プレイヤーがこのオブジェクトの範囲に入った瞬間に実行
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突した相手がプレイヤーかどうかをタグで判定
        if (collision.CompareTag("Player"))
        {
            Warp(collision.transform);
        }
    }

    private void Warp(Transform playerTransform)
    {
        if (destination == null)
        {
            Debug.LogWarning("ワープ先が設定されていません！");
            return;
        }

        // プレイヤーの位置をワープ先の座標に上書き
        playerTransform.position = destination.position;
        
        Debug.Log("Warped to: " + destination.name);
    }
}