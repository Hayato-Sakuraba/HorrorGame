using System.Collections;
using UnityEngine;

public class ParalysisTrap : MonoBehaviour, TrapInterface
{
    public AudioSource audioSource;
    public AudioClip ParalysisSE;

    public float paralysisTime = 3f;

    public void ActiveTrap(GameObject player)
    {
        DebugMove move =
            player.GetComponent<DebugMove>();

        if (move != null)
        {
            if (audioSource != null &&
                ParalysisSE != null)
            {
                audioSource.PlayOneShot(ParalysisSE);
            }

            StartCoroutine(Paralyze(move));
        }
    }

    public void UnActiveTrap()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
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