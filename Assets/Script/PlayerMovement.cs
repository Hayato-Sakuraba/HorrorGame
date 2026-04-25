using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Send Messages で呼ばれる関数
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 forward = transform.forward;    // プレイヤーの前
        Vector3 right = transform.right;   // プレイヤーの右

        Vector3 move = (forward * moveInput.y + right * moveInput.x).normalized;

        rb.linearVelocity = move * moveSpeed;
    }
}