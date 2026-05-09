using UnityEngine;

public class LitScript : MonoBehaviour
{
    [Header("部屋オブジェクトを登録")]
    [Tooltip("各部屋をドラッグ＆ドロップ")]
    [SerializeField] private GameObject[] targetRooms;

    //SetRoomVisibility(0, true);これで表示
    //SetRoomVisibility(0, false);これで非表示
        public void SetRoomVisibility(int index,bool isVisiable)
    {
        
        if (index < 0 || index >= targetRooms.Length)
        {
            Debug.LogError($"[RoomVisibilityController] エラー: 指定されたインデックス ({index}) は配列の範囲外です。");
            return;
        }

        if (targetRooms[index] == null)
        {
            Debug.LogError($"[RoomVisibilityController] エラー: インデックス ({index}) のオブジェクトがInspectorで未設定（Null）です。");
            return;
        }

        // 表示・非表示の切り替え
        targetRooms[index].SetActive(isVisiable);
    }
}
