using UnityEngine;

public class DeathHole : MonoBehaviour, TrapInterface
{
    public AudioSource audioSource;
    public AudioClip trapSE;

    public void ActiveTrap(GameObject player)
    {
        if (audioSource != null &&
            trapSE != null)
        {
            audioSource.PlayOneShot(trapSE);
        }

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.InstantDeath();
        }
    }

    public void UnActiveTrap()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}