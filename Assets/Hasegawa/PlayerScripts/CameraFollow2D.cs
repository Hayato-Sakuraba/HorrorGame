using UnityEngine;

// 2D用カメラ追従スクリプト
// シーン内の Main Camera に追加する。
// Canvas の Render Mode は「World Space」に設定する。

public class CameraFollow2D : MonoBehaviour
{
    // 追従するプレイヤー
    [SerializeField] private Transform target;

    // カメラが追従する速さ
    [SerializeField] private float followSpeed = 5f;

    // カメラとプレイヤーの位置のずれ
    [SerializeField] private Vector2 offset;

    // 全てのオブジェクトが移動した後に実行
    private void LateUpdate()
    {
        // プレイヤーが設定されていない場合は処理しない
        if (target == null)
        {
            return;
        }

        // プレイヤーの位置 + オフセットを追従位置にする
        Vector3 targetPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        // なめらかにプレイヤーへ追従する
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}