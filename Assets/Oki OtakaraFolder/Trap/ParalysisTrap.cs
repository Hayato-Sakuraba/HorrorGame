using System.Collections;
using UnityEngine;

public class ParalysisTrap : MonoBehaviour, TrapInterface
{
    public AudioSource audioSource;
    public AudioClip ParalysisSE;
    public float paralysisTime = 3f;
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
            DebugMove move = player.GetComponent<DebugMove>();

            if (move != null)
            {
                StartCoroutine(Paralyze(move));
            }
        }
    }

    IEnumerator Paralyze(DebugMove move)
    {
        Debug.Log("–ƒáƒ!");

        move.canMove = false;

        yield return new WaitForSeconds(paralysisTime);

        move.canMove = true;

        Debug.Log("‰ñ•œ!");
    }
}