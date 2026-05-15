using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMove : MonoBehaviour
{
    public float speed = 5f;

    public bool canMove = true;

    void Update()
    {
        if (!canMove)
            return;

        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            move += Vector3.forward;

        if (Keyboard.current.sKey.isPressed)
            move += Vector3.back;

        if (Keyboard.current.aKey.isPressed)
            move += Vector3.left;

        if (Keyboard.current.dKey.isPressed)
            move += Vector3.right;

        transform.position += move.normalized * speed * Time.deltaTime;
    }
}