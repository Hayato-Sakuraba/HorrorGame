using UnityEngine;

public class TrapSE : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayLoop(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySE(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    public void StopSE()
    {
        if (audioSource == null)
        {
            return;
        }

        audioSource.Stop();
    }
}