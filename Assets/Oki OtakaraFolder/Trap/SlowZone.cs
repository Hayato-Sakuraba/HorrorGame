using System.Collections;
using UnityEngine;

public class SlowZone : MonoBehaviour, TrapInterface
{
    public float slowMultiplier = 0.5f;

    public AudioSource audioSource;
    public AudioClip slowSE;

    private DebugMove currentPlayer;
    private Coroutine soundCoroutine;

    public void ActiveTrap(GameObject player)
    {
        currentPlayer = player.GetComponent<DebugMove>();

        if (currentPlayer != null)
        {
            currentPlayer.SetSpeed(
                currentPlayer.moveSpeed * slowMultiplier
            );

            Debug.Log("ˆÚ“®‘¬“x’á‰º");

            if (soundCoroutine == null)
            {
                soundCoroutine = StartCoroutine(CheckMoveSound());
            }
        }
    }

    public void UnActiveTrap()
    {
        if (currentPlayer != null)
        {
            currentPlayer.ResetSpeed();
            currentPlayer = null;

            Debug.Log("‘¬“x–ß‚Á‚½");
        }

        if (soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine);
            soundCoroutine = null;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    IEnumerator CheckMoveSound()
    {
        audioSource.clip = slowSE;
        audioSource.loop = true;

        while (currentPlayer != null)
        {
            if (currentPlayer.GetMoveInput() != Vector2.zero)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }

            yield return null;
        }
    }
}