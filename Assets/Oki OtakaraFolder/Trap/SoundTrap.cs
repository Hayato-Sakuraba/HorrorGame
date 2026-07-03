using System.Collections;
using UnityEngine;

public class SoundTrap : MonoBehaviour, TrapInterface
{
    public float callRange = 100f;

    public AudioSource audioSource;
    public AudioClip alarmSE;

    public float alarmDuration = 3f;
    public float alarmInterval = 0.5f;

    private bool isPlaying = false;

    public void ActiveTrap(GameObject player)
    {
        Debug.Log("âπÇ™ñ¬Ç¡ÇΩÅI");

        if (!isPlaying)
        {
            StartCoroutine(PlayAlarm());
        }

        CallNearestEnemy();
    }

    public void UnActiveTrap()
    {
        // ó£ÇÍÇƒÇ‡é~ÇﬂÇ»Ç¢
    }

    IEnumerator PlayAlarm()
    {
        isPlaying = true;

        float timer = 0f;

        while (timer < alarmDuration)
        {
            if (audioSource != null && alarmSE != null)
            {
                audioSource.PlayOneShot(alarmSE);
            }

            yield return new WaitForSeconds(alarmInterval);

            timer += alarmInterval;
        }

        isPlaying = false;
    }

    void CallNearestEnemy()
    {
        EnemyMove[] enemies =
            FindObjectsByType<EnemyMove>(
                FindObjectsSortMode.None);

        EnemyMove nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (EnemyMove enemy in enemies)
        {
            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position);

            if (distance < nearestDistance &&
                distance <= callRange)
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