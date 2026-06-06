using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 3f;

    public float investigateTime = 3f;

    private Vector3 targetPosition;

    private bool isMoving = false;

    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;

            Debug.Log("到着して調査開始");

            StartCoroutine(Investigate());
        }
    }

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;

        isMoving = true;

        Debug.Log("音の場所へ移動");
    }

    IEnumerator Investigate()
    {
        Debug.Log("調査中...");

        yield return new WaitForSeconds(investigateTime);

        Debug.Log("調査終了");
    }
}