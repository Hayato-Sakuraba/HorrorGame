using UnityEngine;

public class FallingDebrisZone : MonoBehaviour, TrapInterface
{
    [Header("å¯â âπ")]
    public AudioSource audioSource;
    public AudioClip deathSE;

    [Range(0f, 1f)]
    public float deathChance = 0.2f;

    public float checkInterval = 1f;

    private float timer = 0f;
    private GameObject currentPlayer;

    private void Update()
    {
        if (currentPlayer == null)
        {
            return;
        }

        DebugMove move =
            currentPlayer.GetComponent<DebugMove>();

        if (move == null)
        {
            return;
        }

        // ìÆÇ¢ÇƒÇ¢Ç»Ç¢Ç»ÇÁîªíËÇµÇ»Ç¢
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

        float random = Random.Range(0f, 1f);

        if (random <= deathChance)
        {
            if (audioSource != null && deathSE != null)
            {
                audioSource.PlayOneShot(deathSE);
            }

            PlayerHealth health =
                currentPlayer.GetComponent<PlayerHealth>();

            if (health != null)
            {
                Debug.Log("ä¢‚IíºåÇÅI");
                health.InstantDeath();
            }
        }
        else
        {
            Debug.Log("ä¢‚IâÒî");
        }
    }

    public void ActiveTrap(GameObject player)
    {
        currentPlayer = player;

        if (audioSource != null &&
            !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void UnActiveTrap()
    {
        currentPlayer = null;
        timer = 0f;

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}