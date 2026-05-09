using UnityEngine;

public class LanternLight : MonoBehaviour
{
    [SerializeField] private Light lanternLight;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("明かりの範囲")]
    [SerializeField] private float normalRange = 8f;//徒歩
    [SerializeField] private float dashRange = 5f;//ダッシュ

    [Header("切り替えのなめらかさ")]
    [SerializeField] private float changeSpeed = 5f;//ライト変化速度

    private void Reset()
    {
        lanternLight = GetComponent<Light>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        if (lanternLight == null || playerMovement == null) return;

        float targetRange = playerMovement.IsDashing ? dashRange : normalRange;

        lanternLight.range = Mathf.Lerp(
            lanternLight.range,
            targetRange,
            changeSpeed * Time.deltaTime
        );
    }
}