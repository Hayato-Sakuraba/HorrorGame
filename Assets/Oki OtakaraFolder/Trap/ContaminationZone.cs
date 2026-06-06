using System.Collections;
using UnityEngine;

public class ContaminationZone : MonoBehaviour,TrapInterface
{
    public AudioSource audioSource;
    public AudioClip ContaminationSE;
    public float contaminationTime = 20f;

    private Coroutine contaminationCoroutine;
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
        if (!player.CompareTag("Player"))
        {
            return;
        }

        Inventory inventory = player.GetComponent<Inventory>();

        if (inventory != null)
        {
            contaminationCoroutine =
                StartCoroutine(Contaminate(inventory));

            Debug.Log("汚染開始");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (contaminationCoroutine != null)
        {
            StopCoroutine(contaminationCoroutine);

            Debug.Log("汚染終了");
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

            // ランダムお宝取得
            Otakara item =
                inventory.items[
                    Random.Range(0, inventory.items.Count)
                ];

            // 半額
            item.currentPrice /= 2;

            Debug.Log(
                item.name +
                " が汚染！ 現在価値 : " +
                item.currentPrice
            );

            // 10以下なら破壊
            if (item.currentPrice <= 10)
            {
                inventory.items.Remove(item);

                inventory.currentSize -= item.guram;

                Debug.Log(
                    item.name +
                    " は汚染で崩壊した！"
                );
            }
        }
    }
}