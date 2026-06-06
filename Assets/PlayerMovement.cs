using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 9f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public bool IsDashing { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        bool dashKey =
            Keyboard.current.leftShiftKey.isPressed;

        IsDashing = dashKey;

        float speed =
            dashKey ? dashSpeed : walkSpeed;

        rb.linearVelocity =
            moveInput.normalized * speed;
    }
}