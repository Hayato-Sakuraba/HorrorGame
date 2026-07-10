using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContaminationOverlay : MonoBehaviour
{
    public Image overlayImage;

    // 最大透明度
    public float maxAlpha = 0.25f;

    // フェード速度
    public float fadeSpeed = 1f;

    private Coroutine fadeCoroutine;

    // 徐々に色を付ける
    public void SetProgress(float progress)
    {
        Color color = overlayImage.color;

        color.a = Mathf.Lerp(0f, maxAlpha, progress);

        overlayImage.color = color;
    }

    // 徐々に透明に戻す
    public void ResetOverlay()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeTo(0f));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        while (Mathf.Abs(overlayImage.color.a - targetAlpha) > 0.01f)
        {
            Color c = overlayImage.color;

            c.a = Mathf.MoveTowards(
                c.a,
                targetAlpha,
                fadeSpeed * Time.deltaTime);

            overlayImage.color = c;

            yield return null;
        }

        Color end = overlayImage.color;
        end.a = targetAlpha;
        overlayImage.color = end;

        fadeCoroutine = null;
    }
}