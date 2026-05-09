using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float dashSpeed = 7f;


    private Rigidbody2D rb;
    private Vector2 moveInput;
    private UnityEngine.InputSystem.InputAction inputAction;

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
        Vector2 Up = transform.up;
        Vector2 right = transform.right;

        Vector2 move = (Up * moveInput.y + right * moveInput.x).normalized;
        Debug.Log(moveInput.y);

        bool dashKey = Keyboard.current.leftShiftKey.isPressed;
        IsDashing=dashKey;
        float speed = dashKey ? dashSpeed : walkSpeed;

        rb.linearVelocity = move * speed;
    }
    public bool IsDashing { get; private set; }
}