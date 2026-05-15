using System.Collections;
using UnityEngine;

public class ParalysisTrap : MonoBehaviour
{
    public float paralysisTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DebugMove move = other.GetComponent<DebugMove>();

            if (move != null)
            {
                StartCoroutine(Paralyze(move));
            }
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