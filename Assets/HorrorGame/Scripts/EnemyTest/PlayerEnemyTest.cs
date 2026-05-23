using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMove2D : MonoBehaviour
{
	public float moveSpeed = 5f;

	public Tilemap wallTilemap;

	private InputAction moveAction;

	void Start()
	{
		moveAction =
			InputSystem.actions.FindAction("Move");
	}

	void Update()
	{
		Vector2 moveInput =
			moveAction.ReadValue<Vector2>();

		Vector3 move =
			new Vector3(
				moveInput.x,
				moveInput.y,
				0f
			) * moveSpeed * Time.deltaTime;

		Vector3 nextPos =
			transform.position + move;

		// ŽŸ‚ÌˆÊ’u‚Ìƒ^ƒCƒ‹Žæ“¾
		Vector3Int cell =
			wallTilemap.WorldToCell(nextPos);

		// •Ç‚¶‚á‚È‚¯‚ê‚ÎˆÚ“®
		if (!wallTilemap.HasTile(cell))
		{
			transform.position = nextPos;
		}
	}
}