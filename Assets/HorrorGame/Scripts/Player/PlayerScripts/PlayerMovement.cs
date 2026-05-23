using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float dashSpeed = 7f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

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
        Vector2 move = moveInput;

        bool dashKey = Keyboard.current.leftShiftKey.isPressed;
        IsDashing = dashKey;

        float speed = dashKey ? dashSpeed : walkSpeed;

        rb.linearVelocity = move.normalized * speed;
    }
    public bool IsDashing { get; private set; }
}