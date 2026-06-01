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
    private Rigidbody rb;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;
    private bool isDead = false;
    private bool staminaEmpty = false;
    public bool canMove = true;

    public void SetDead(bool dead)
    {
        isDead = dead;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                staminaEmpty = true;
            }
        }
        else
        {
            currentStamina += staminaRecovery * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        if (currentStamina >= maxStamina - 0.01f)
        {
            currentStamina = maxStamina;
            staminaEmpty = false;
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

        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        bool dashKey =
            Keyboard.current.leftShiftKey.isPressed &&
            moveInput != Vector2.zero &&
            !staminaEmpty;
        IsDashing = dashKey;

        float speed = dashKey ? dashSpeed : walkSpeed;

        rb.linearVelocity = move.normalized * speed;
    }
    public bool IsDashing { get; private set; }
    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
}