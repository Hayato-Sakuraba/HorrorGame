using UnityEngine;

public class FallingDebrisZone : MonoBehaviour, TrapInterface
{
    public AudioSource audioSource;
    public AudioClip FallingSE;
    [Range(0f, 1f)]
    public float deathChance = 0.2f;

    public float checkInterval = 1f;

    private float timer;
    private void OnTriggerStay(Collider player)
    {
        if (!player.CompareTag("Player"))
        {
            return;
        }

        ActiveTrap(player.gameObject);
    }

    public void ActiveTrap(GameObject player)
    {
        if (!player.CompareTag("Player"))
        {
            return;
        }

        // 動いてるか確認
        DebugMove move =
            player.GetComponent<DebugMove>();

        if (move == null)
        {
            return;
        }

        // 入力なしなら安全
        if (move.GetMoveInput() == Vector2.zero)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer < checkInterval)
        {
            return;
        }

        timer = 0f;

        float random =
            Random.Range(0f, 1f);

        if (random <= deathChance)
        {
            PlayerHealth health =
                player.GetComponent<PlayerHealth>();

            if (health != null)
            {
                Debug.Log("瓦礫直撃！");

                health.InstantDeath();
            }
        }
        else
        {
            Debug.Log("瓦礫回避");
        }
    }
}