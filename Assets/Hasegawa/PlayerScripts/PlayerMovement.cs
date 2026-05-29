using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float dashSpeed = 7f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRecovery = 20f;
    [SerializeField] private float staminaConsumption = 30f;

    private float currentStamina;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;
    private bool isDead = false;

    public void SetDead(bool dead)
    {
        isDead = dead;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        if (IsDashing)
        {
            currentStamina -= staminaConsumption * Time.deltaTime;

            if (currentStamina < 0f)
            {
                currentStamina = 0f;
            }
        }
        else
        {
            currentStamina += staminaRecovery * Time.deltaTime;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
        Debug.Log(currentStamina);
    }
    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }
        Vector2 move = new Vector2(moveInput.x, moveInput.y);

        bool dashKey =
            Keyboard.current.leftShiftKey.isPressed &&
            moveInput != Vector2.zero &&
            currentStamina > 0f;
        IsDashing = dashKey;

        float speed = dashKey ? dashSpeed : walkSpeed;

        rb.linearVelocity = move.normalized * speed;
    }
    public bool IsDashing { get; private set; }
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
}