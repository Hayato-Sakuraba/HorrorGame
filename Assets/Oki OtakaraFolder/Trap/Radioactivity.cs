using System.Collections;
using UnityEngine;

public class Radioactivity : MonoBehaviour, TrapInterface
{
    [Header("ê›íË")]
    public int damage = 5;
    public float interval = 1f;

    [Header("å¯â âπ")]
    public AudioSource audioSource;
    public AudioClip damejiSE;

    private Coroutine damageCoroutine;

    public void ActiveTrap(GameObject player)
    {
        if (damageCoroutine == null)
        {
            damageCoroutine =
                StartCoroutine(
                    DamageLoop(player)
                );
        }
    }

    public void UnActiveTrap()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }

    }

    IEnumerator DamageLoop(GameObject target)
    {
        PlayerHealth health =
            target.GetComponent<PlayerHealth>();

        while (true)
        {
            if (health != null)
            {
                health.TakeDamage(damage);

                if (audioSource != null &&
                    damejiSE != null)
                {
                    audioSource.PlayOneShot(damejiSE);
                }

                Debug.Log(
                    "ï˙éÀê¸É_ÉÅÅ[ÉW : " +
                    damage
                );
            }

            yield return new WaitForSeconds(interval);
        }
    }
}