using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    public Action OnShoot;

    public Action OnJump;
    public Action OnFlip;
    public Action<MovementInput> OnMoveStateUpdated;

    public float JoystickX => _movementJoystick.Horizontal;
    public float JoystickY => _movementJoystick.Vertical;

    public MovementInput MovementInputState { get; private set; }

    [SerializeField] private float _joystickInaccuracyY;
    [SerializeField] private float _joystickInaccuracyX;

    public enum MovementInput
    {
        MoveRight,
        MoveLeft,
        Stay
    }

    private FloatingJoystick _movementJoystick;

    private bool _isButtonShootPressed;

    public void Initialize(FloatingJoystick joyStick)
    {
        _movementJoystick = joyStick;
    }

    private void Update()
    {
        CheckJoystickInput();

        if (_isButtonShootPressed)
            ShootClick();
    }

    public void PressShootingButton(bool isPressed)
    {
        _isButtonShootPressed = isPressed;
    }
   
    private void ShootClick()
    {
        OnShoot?.Invoke();
    }

    private void CheckJoystickInput()
    {
        if (JoystickY > _joystickInaccuracyY)
        {
            OnJump?.Invoke();
        }

        var previousState = MovementInputState;

        if (JoystickX < -_joystickInaccuracyX )
            MovementInputState = MovementInput.MoveLeft;

        else if (JoystickX > _joystickInaccuracyX)
            MovementInputState = MovementInput.MoveRight;

        else
            MovementInputState = MovementInput.Stay;

        if (previousState != MovementInputState)
        {
            OnMoveStateUpdated?.Invoke(MovementInputState);

            if(MovementInputState != MovementInput.Stay)
            {
                OnFlip?.Invoke();
            }
        }
    }
}
