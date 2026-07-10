using System.Collections;
using UnityEngine;

public class ContaminationZone : MonoBehaviour, TrapInterface
{
    [Header("Audio")]
    public AudioSource areaAudioSource;
    public AudioSource seAudioSource;

    public AudioClip ContaminationSE; // 汚染エリア音
    public AudioClip valueDownSE;     // 価値減少音
    public AudioClip breakItemSE;     // 崩壊音

    [Header("Settings")]
    public float contaminationTime = 20f;

    private Coroutine contaminationCoroutine;

    private bool overlayPlaying = false;

    public ContaminationOverlay contaminationOverlay;

    public void UnActiveTrap()
    {
        if (contaminationCoroutine != null)
        {
            StopCoroutine(contaminationCoroutine);
            contaminationCoroutine = null;
        }

        overlayPlaying = false;

        if (areaAudioSource != null)
        {
            areaAudioSource.Stop();
        }

        if (contaminationOverlay != null)
        {
            contaminationOverlay.ResetOverlay();
        }

        Debug.Log("汚染終了");
    }
    public void ActiveTrap(GameObject player)
    {
        Inventory inventory =
            player.GetComponent<Inventory>();

        if (inventory != null &&
            contaminationCoroutine == null)
        {
            contaminationCoroutine =
                StartCoroutine(
                    Contaminate(inventory)
                );

            Debug.Log("汚染開始");
        }

        if (areaAudioSource != null &&
            ContaminationSE != null &&
            !areaAudioSource.isPlaying)
        {
            areaAudioSource.clip = ContaminationSE;
            areaAudioSource.loop = true;
            areaAudioSource.Play();
        }
        if (contaminationOverlay != null && !overlayPlaying)
        {
            overlayPlaying = true;
            StartCoroutine(FadeOverlay());
        }
    }




    IEnumerator Contaminate(Inventory inventory)
    {
        while (true)
        {
            yield return new WaitForSeconds(contaminationTime);

            if (inventory.items.Count <= 0)
            {
                continue;
            }

            // ランダムなお宝を選択
            Otakara item =
                inventory.items[
                    Random.Range(0, inventory.items.Count)
                ];

            // 半額にする
            item.currentPrice /= 2;

            // 価値減少音
            if (seAudioSource != null && valueDownSE != null)
            {
                seAudioSource.PlayOneShot(valueDownSE);
            }

            Debug.Log(
                item.name +
                " が汚染！ 現在価値 : " +
                item.currentPrice
            );

            // 価値が10以下なら破壊
            if (item.currentPrice <= 10)
            {
                if (seAudioSource != null &&
                    breakItemSE != null)
                {
                    seAudioSource.PlayOneShot(breakItemSE);
                }

                inventory.RemoveItem(item);

                Debug.Log(
                    item.name +
                    " は汚染で崩壊した！"
                );
            }
        }
    }
    IEnumerator FadeOverlay()
    {
        float timer = 0f;
        float fadeTime = 2f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            if (contaminationOverlay != null)
            {
                contaminationOverlay.SetProgress(timer / fadeTime);
            }

            yield return null;
        }

        if (contaminationOverlay != null)
        {
            contaminationOverlay.SetProgress(1f);
        }
    }
}