using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScriptTest : MonoBehaviour
{
	private float speed = 5f;
	private InputAction moveAction;
	private void Start()
	{
		moveAction = InputSystem.actions.FindAction("Move");
	}
	void Update()
	{
		var moveValue = moveAction.ReadValue<Vector2>();
		var move = new Vector3(moveValue.x, 0f, moveValue.y) * speed * Time.deltaTime;
		transform.Translate(move);
	}
}
