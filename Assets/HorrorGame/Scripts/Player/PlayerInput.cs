using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions _actions;
    private InputSystem_Actions.PlayerActions _PlayerinputAction;

    /// <summary>
    ///     マウスを使用しているならtrue
    ///     移動量がマウス由来の際はdeltaTimeを乗算しないようにするために使用
    /// </summary>

    public Vector2 MoveDirection { get; private set; }
    public bool ismoving => MoveDirection.magnitude > 0;
    public bool Interact => _PlayerinputAction.Interact.triggered;
    public bool Sprint => _PlayerinputAction.Sprint.IsPressed();

    private void Awake()
    {
        _actions = new InputSystem_Actions();
        _PlayerinputAction = _actions.Player;
    }

    private void OnEnable()
    {
        _PlayerinputAction.Enable();
    }

    private void OnDisable()
    {
        _PlayerinputAction.Disable();
    }

    private void Update()
    {
        MoveDirection = _PlayerinputAction.Move.ReadValue<Vector2>();
    }
}
