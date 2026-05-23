using UnityEngine;

public class LanternLight : MonoBehaviour
{
    [SerializeField] private Light lanternLight;
    [SerializeField] private Transform lanternSprite;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("明かりの範囲")]
    [SerializeField] private float normalRange = 8f; // 徒歩
    [SerializeField] private float dashRange = 5f; // ダッシュ

    [Header("スプライトのサイズ")]
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 dashScale = new Vector3(0.8f, 0.8f, 1f);

    [Header("スプライト拡大縮小")]
    [SerializeField] private float normalPulseAmount = 0.1f; // 通常時の変動量
    [SerializeField] private float normalPulseInterval = 0.4f; // 通常時の変動間隔（秒）
    [SerializeField] private float dashPulseAmount = 0.05f; // ダッシュ時の変動量
    [SerializeField] private float dashPulseInterval = 0.2f; // ダッシュ時の変動間隔（秒）

    [Header("切り替えのなめらかさ")]
    [SerializeField] private float changeSpeed = 5f; // ライト変化速度
    [SerializeField] private float scaleChangeSpeed = 5f; // スプライト変化速度

    private void Reset()
    {
        lanternLight = GetComponent<Light>();
        lanternSprite = GetComponentInChildren<SpriteRenderer>()?.transform;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        if (playerMovement == null) return;

        if (lanternLight != null)
        {
            float targetRange = playerMovement.IsDashing ? dashRange : normalRange;
            lanternLight.range = Mathf.Lerp(
                lanternLight.range,
                targetRange,
                changeSpeed * Time.deltaTime
            );
        }

        if (lanternSprite != null)
        {
            Vector3 baseScale = playerMovement.IsDashing ? dashScale : normalScale;
            Vector3 targetScale = baseScale;

            if (playerMovement.MoveInput.sqrMagnitude > 0.01f)
            {
                float pulseAmount = playerMovement.IsDashing ? dashPulseAmount : normalPulseAmount;
                float pulseInterval = playerMovement.IsDashing ? dashPulseInterval : normalPulseInterval;
                float interval = Mathf.Max(0.01f, pulseInterval);
                float pulse = Mathf.Sin(Time.time * (Mathf.PI * 2f / interval)) * pulseAmount;
                float scaleFactor = 1f + pulse;
                targetScale = new Vector3(
                    baseScale.x * scaleFactor,
                    baseScale.y * scaleFactor,
                    baseScale.z
                );
            }

            lanternSprite.localScale = Vector3.Lerp(
                lanternSprite.localScale,
                targetScale,
                scaleChangeSpeed * Time.deltaTime
            );
        }
    }
}