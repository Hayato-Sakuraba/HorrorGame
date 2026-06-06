using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    [HideInInspector]
    public float currentSpeed;

    public bool canMove = true;

    private Vector2 moveInput;

    private void Start()
    {
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        Vector3 move =
            new Vector3(
                moveInput.x,
                0,
                moveInput.y
            );

        transform.position +=
            move * currentSpeed * Time.deltaTime;
    }

    // InputSystem
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // ë¨ìxïœçX
    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    // å≥Ç…ñﬂÇ∑
    public void ResetSpeed()
    {
        currentSpeed = moveSpeed;
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
}