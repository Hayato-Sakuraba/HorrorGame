using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    [Header("ワープ地点（空のオブジェクトでOK）")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("ワープが発動する範囲（半径）")]
    [SerializeField] private float triggerRadius = 0.5f;

    // 無限ループ防止用のクールダウンタイマー
    private float warpCooldown = 5f;
    private float lastWarpTime = -10f;

    private void Update()
    {
        // どちらかの地点が設定されていない場合はエラーを防ぐ
        if (pointA == null || pointB == null) return;

        // ワープ直後のクールダウン中は判定をスキップ（無限ループ防止）
        if (Time.time - lastWarpTime < warpCooldown) return;

        // A地点にプレイヤーがいるかチェック
        Collider2D hitA = Physics2D.OverlapCircle(pointA.position, triggerRadius);
        if (hitA != null && hitA.CompareTag("Player"))
        {
            WarpPlayer(hitA.transform, pointB);
            return; // 同時発動を防ぐため、ここで処理を終了
        }

        // B地点にプレイヤーがいるかチェック
        Collider2D hitB = Physics2D.OverlapCircle(pointB.position, triggerRadius);
        if (hitB != null && hitB.CompareTag("Player"))
        {
            WarpPlayer(hitB.transform, pointA);
        }
    }

    private void WarpPlayer(Transform playerTransform, Transform destination)
    {
        // プレイヤーの位置を目的地に移動
        playerTransform.position = destination.position;
        
        // ワープした時間を記録してクールダウン開始
        lastWarpTime = Time.time;
        
        Debug.Log($"{destination.gameObject.name} へワープしました！");
    }

    // 【おまけ】Unityエディタ上でワープの判定範囲を「緑色の円」で可視化する機能
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.4f); // 半透明の緑
        if (pointA != null) Gizmos.DrawSphere(pointA.position, triggerRadius);
        if (pointB != null) Gizmos.DrawSphere(pointB.position, triggerRadius);
    }
}