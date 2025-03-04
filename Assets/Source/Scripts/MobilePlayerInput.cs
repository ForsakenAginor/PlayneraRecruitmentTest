using UnityEngine;

public class MobilePlayerInput : IPlayerInput
{
    private InputSystem_Actions _input;

    public MobilePlayerInput()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    public Vector2 GetPointerPosition()
    {
        return _input.Player.Position.ReadValue<Vector2>();
    }

    public Vector2 GetRotation()
    {
        return _input.Player.Look.ReadValue<Vector2>();
    }
}
