using System.Collections;
using UnityEngine;

public class Radioactivity : MonoBehaviour
{
    public int damage = 5;
    public float interval = 1f;

    private Coroutine damageCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            damageCoroutine = StartCoroutine(DamageLoop(other.gameObject));
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