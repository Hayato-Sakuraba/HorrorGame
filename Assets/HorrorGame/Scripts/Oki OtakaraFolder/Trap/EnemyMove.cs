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

            Debug.Log("“ž’…‚µ‚Ä’²¸ŠJŽn");

            StartCoroutine(Investigate());
        }
    }

    public void MoveTo(Vector3 position)
    {
        targetPosition = position;

        isMoving = true;

        Debug.Log("‰¹‚ÌêŠ‚ÖˆÚ“®");
    }

    IEnumerator Investigate()
    {
        Debug.Log("’²¸’†...");

        yield return new WaitForSeconds(investigateTime);

        Debug.Log("’²¸I—¹");
    }
}