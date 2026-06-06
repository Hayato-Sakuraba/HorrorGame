using UnityEngine;

public class SoundTrap : MonoBehaviour, TrapInterface
{
    public float callRange = 100f;

    public AudioSource audioSource;
    public AudioClip alarmSE;

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            ActiveTrap(player.gameObject);
        }
    }

    public void ActiveTrap(GameObject player)
    {
        if (!player.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("âπÇ™ñ¬Ç¡ÇΩÅI");

        CallNearestEnemy();
    }

    void CallNearestEnemy()
    {
        EnemyMove[] enemies =
            FindObjectsByType<EnemyMove>(FindObjectsSortMode.None);

        EnemyMove nearestEnemy = null;

        float nearestDistance = Mathf.Infinity;

        foreach (EnemyMove enemy in enemies)
        {
            float distance =
                Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < nearestDistance && distance <= callRange)
            {
                nearestDistance = distance;

                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            nearestEnemy.MoveTo(transform.position);

            Debug.Log("ìGÇåƒÇÒÇæÅI");
        }
    }
}