using System.Collections;
using UnityEngine;

public class Radioactivity : MonoBehaviour, TrapInterface
{
    public AudioSource audioSource;
    public AudioClip RadioactivitySE;
    public int damage = 5;
    public float interval = 1f;

    private Coroutine damageCoroutine;
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
        if (player.CompareTag("Player"))
        {
            damageCoroutine = StartCoroutine(DamageLoop(player.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
        }
    }

    IEnumerator DamageLoop(GameObject target)
    {
        PlayerHealth health = target.GetComponent<PlayerHealth>();

        while (true)
        {
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            yield return new WaitForSeconds(interval);
        }
    }
}