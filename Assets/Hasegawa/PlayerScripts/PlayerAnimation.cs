using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    private Vector2 lastMoveDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Vector2 moveInput = playerMovement.MoveInput;

        // 入力がある時だけ方向更新
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }

        animator.SetFloat("MoveX", lastMoveDirection.x);
        animator.SetFloat("MoveY", lastMoveDirection.y);
    }
}