using System.Collections;
using UnityEngine;

public class Radioactivity : MonoBehaviour, TrapInterface
{
    [Header("ê›íË")]
    public float deathTime = 20f;

    [Header("å¯â âπ")]
    public AudioSource areaAudioSource;
    public AudioSource seAudioSource;

    public AudioClip radiationLoopSE;
    public AudioClip deathSE;

    private Coroutine deathCoroutine;

    public void ActiveTrap(GameObject player)
    {
        if (areaAudioSource != null &&
            radiationLoopSE != null &&
            !areaAudioSource.isPlaying)
        {
            areaAudioSource.clip = radiationLoopSE;
            areaAudioSource.loop = true;
            areaAudioSource.Play();
        }

        if (deathCoroutine == null)
        {
            deathCoroutine =
                StartCoroutine(DeathTimer(player));
        }
        RadiationEffect.Instance.StartRadiation(deathTime);
    }

    public void UnActiveTrap()
    {
        if (deathCoroutine != null)
        {
            StopCoroutine(deathCoroutine);
            deathCoroutine = null;
        }

        // ä¬ã´âπí‚é~
        if (areaAudioSource != null)
        {
            areaAudioSource.Stop();
        }
        RadiationEffect.Instance.StopRadiation();
    }

    IEnumerator DeathTimer(GameObject player)
    {
        yield return new WaitForSeconds(deathTime);

        // ä¬ã´âπí‚é~
        if (areaAudioSource != null)
        {
            areaAudioSource.Stop();
        }

        // éÄñSSE
        if (seAudioSource != null &&
            deathSE != null)
        {
            seAudioSource.PlayOneShot(deathSE);
        }

        Debug.Log("ï˙éÀê¸Ç≈éÄñS");

        deathCoroutine = null;
        RadiationEffect.Instance.StopRadiation();
    }
}