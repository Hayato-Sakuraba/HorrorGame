using UnityEngine;

public class CameraFollowXZ : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.1f;
    public Vector3 offset;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        desired.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed);
    }
}
