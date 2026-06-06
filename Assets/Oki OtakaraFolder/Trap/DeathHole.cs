using UnityEngine;

public class DeathHole : MonoBehaviour,TrapInterface
{
    public AudioSource audioSource;
    public AudioClip deathSE;
    private void OnTriggerEnter(Collider player)

    {
        if (!player.CompareTag("Player"))
        {
            return;
        }

        ActiveTrap(player.gameObject);
    }

    public void ActiveTrap(GameObject player)
    {
        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.InstantDeath();
        }
    }
}