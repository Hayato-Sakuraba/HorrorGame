using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("方向画像")]
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    private Vector2 lastMoveDirection = Vector2.down;

    private void Reset()
    {
        playerImage = GetComponent<Image>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (playerImage == null || playerMovement == null)
        {
            return;
        }

        Vector2 moveInput = playerMovement.MoveInput;

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }

        if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
        {
            playerImage.sprite = lastMoveDirection.x > 0 ? rightSprite : leftSprite;
        }
        else
        {
            playerImage.sprite = lastMoveDirection.y > 0 ? backSprite : frontSprite;
        }
    }
}