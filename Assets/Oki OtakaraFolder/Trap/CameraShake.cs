using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPos;

    private void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake(float duration, float strength)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, strength));
    }

    IEnumerator ShakeCoroutine(float duration, float strength)
    {
        float timer = 0f;

        while (timer < duration)
        {
            transform.localPosition =
                originalPos +
                Random.insideUnitSphere * strength;

            timer += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}