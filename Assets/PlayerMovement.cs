using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float dashSpeed = 7f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 move = (forward * moveInput.y + right * moveInput.x).normalized;

        bool dashKey = Keyboard.current.leftShiftKey.isPressed;

        float speed = dashKey ? dashSpeed : walkSpeed;

        rb.linearVelocity = move * speed;
    }
}