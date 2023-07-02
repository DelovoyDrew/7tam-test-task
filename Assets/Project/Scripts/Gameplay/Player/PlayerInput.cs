using System;
using UnityEngine;
using UnityEngine.UI;

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
    private Button _shootButton;

    private bool _isButtonShootPressed;

    public void Initialize(FloatingJoystick joyStick, Button shootButton)
    {
        _movementJoystick = joyStick;
        _shootButton = shootButton;

        shootButton.onClick.AddListener(ShootClick);
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
   
    public void Enable(bool isEnabled)
    {
        _movementJoystick.enabled = isEnabled;
        _shootButton.enabled = isEnabled;
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

            if (MovementInputState != MovementInput.Stay)
            {
                OnFlip?.Invoke();
            }
        }
    }
}
