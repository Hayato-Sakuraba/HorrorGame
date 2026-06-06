using System.Collections;
using UnityEngine;

public class PitfallTrap : MonoBehaviour, TrapInterface
{
    [Header("Œø‰Ê‰¹")]
    public AudioSource audioSource;

    public AudioClip crackSE;
    public AudioClip fallSE;
    public AudioClip breakItemSE;

    private int hitCount = 0;

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            ActiveTrap(player.gameObject);
        }
    }

    public void ActiveTrap(GameObject player)
    {
        hitCount++;

        if (hitCount == 1)
        {
            PlaySE(crackSE);
            return;
        }

        PlaySE(fallSE);

        DebugMove move = player.GetComponent<DebugMove>();

        if (move != null)
        {
            StartCoroutine(StopMove(move));
        }

        Inventory inventory = player.GetComponent<Inventory>();

        if (inventory != null)
        {
            if (Random.Range(0, 100) < 50)
            {
                inventory.DestroyRandomItem();

                PlaySE(breakItemSE);
            }
        }
    }

    IEnumerator StopMove(DebugMove move)
    {
        move.canMove = false;

        yield return new WaitForSeconds(1f);

        move.canMove = true;
    }

    void PlaySE(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}