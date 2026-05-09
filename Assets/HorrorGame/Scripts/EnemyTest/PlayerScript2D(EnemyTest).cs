using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript2D : MonoBehaviour
{
	private float speed = 5f;
	private float verticalSpeed = 5f; // 上下移動用

	private InputAction moveAction;

	private void Start()
	{
		moveAction = InputSystem.actions.FindAction("Move");
	}

	void Update()
	{
		var moveValue = moveAction.ReadValue<Vector2>();

		// 横移動（XZ）
		Vector3 move = new Vector3(moveValue.x, 0f, moveValue.y);

		// 上下移動（Y）
		if (Keyboard.current.leftShiftKey.isPressed)
		{
			move.y += 1f;
		}
		if (Keyboard.current.leftCtrlKey.isPressed)
		{
			move.y -= 1f;
		}

		// 合成して移動
		transform.Translate(move * speed * Time.deltaTime);
	}
}