using System.Collections;
using UnityEngine;

public class PitfallTrap : MonoBehaviour
{
    private int hitCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        hitCount++;

        if (hitCount == 1)
        {
            Debug.Log("ヒビが入った！");
            return;
        }

        Debug.Log("穴に落ちた！");

        // 動けなくする
        PlayerMovement move = other.GetComponent<PlayerMovement>();

        if (move != null)
        {
            StartCoroutine(StopMove(move));
        }

        // 50%でお宝破壊
        if (Random.Range(0, 100) < 50)
        {
            InventoryManager.Instance.DestroyRandomItem();

            Debug.Log("お宝が壊れた！");
        }
    }

    private IEnumerator StopMove(PlayerMovement move)
    {
        move.canMove = false;

        Debug.Log("1秒動けない！");

        yield return new WaitForSeconds(1f);

        move.canMove = true;

        Debug.Log("復帰！");
    }
}