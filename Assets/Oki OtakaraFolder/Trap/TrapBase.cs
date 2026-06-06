using UnityEngine;

public class TrapBase : MonoBehaviour
{
    public AudioSource audioSource;

    protected void PlaySE(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}