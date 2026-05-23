using UnityEngine;

// 必須コンポーネントの指定。アタッチ忘れを防ぐ
[RequireComponent(typeof(Collider2D))]
public class WarpZone : MonoBehaviour
{
    [Header("ワープ先")]
    [SerializeField] private Transform destination;

    // ワープを受け付ける状態かどうかのフラグ
    private bool isReadyToWarp = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ワープ準備ができていない場合は弾く
        if (!isReadyToWarp) 
        {
            Debug.Log("ワープ準備ができていません。");
            return;
        }

        // 接触したのがプレイヤーであればワープ実行
        if (other.CompareTag("Player"))
        {
            // 1. ワープ先の「ワープ受付」を一時的に無効化する（無限ループ防止）
            WarpZone destZone = destination.GetComponent<WarpZone>();
            if (destZone != null)
            {
                destZone.DisableWarpTemporarily();
            }

            // 2. プレイヤーを移動させる
            other.transform.position = destination.position;
            
            Debug.Log($"{destination.name} へワープしました！");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // プレイヤーが完全に範囲から出たタイミングでワープ受付を再開
        if (other.CompareTag("Player"))
        {
            isReadyToWarp = true;
        }
    }

    // 外部（ワープ元）から呼ばれる無効化処理
    public void DisableWarpTemporarily()
    {
        isReadyToWarp = false;
    }
}