using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Input))]
public class PlayerMovement : MonoBehaviour
{
    public Vector2 MoveDirection { get; private set; }

    [SerializeField] private Transform _body;

    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private Jump _jumpData;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _movementSpeedWhenJump;
    [SerializeField] private float _maxMagnitude;

    private Coroutine _jumpRoutine;

    private void OnEnable()
    {
        _input.OnFlip += Flip;
        _input.OnJump += TryJump;
    }

    private void OnDisable()
    {
        _input.OnFlip -= Flip;
        _input.OnJump -= TryJump;
    }

    private void Update()
    {
        _jumpData.CheckOnFloor();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_input.MovementInputState == PlayerInput.MovementInput.MoveRight)
        {
            MoveDirection = transform.right;
        }
        else if (_input.MovementInputState == PlayerInput.MovementInput.MoveLeft)
        {
            MoveDirection = -transform.right;
        }
        else
        {
            _animator.ChangeState(PlayerAnimator.States.Idle);
            return;
        }
        _animator.ChangeState(PlayerAnimator.States.Run);
        var speed = _jumpData.IsOnFloor ? _movementSpeed : _movementSpeedWhenJump;
        _rigidbody.AddForce(MoveDirection * speed, ForceMode2D.Force);

        if (_rigidbody.velocity.magnitude > _maxMagnitude && _jumpData.IsOnFloor && _jumpRoutine == null)
            _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _maxMagnitude);
    }

    private void TryJump()
    {
        if (_jumpData.IsOnFloor)
        {
            if(_jumpRoutine == null)
            {
                _jumpRoutine = StartCoroutine(JumpRoutine());
            }
        }
    }
    private IEnumerator JumpRoutine()
    {
        _rigidbody.AddForce(transform.up * _jumpData.JumpForce, ForceMode2D.Impulse);
        if (_rigidbody.velocity.magnitude > _jumpData.MaxJumpMagnitude)
            _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _jumpData.MaxJumpMagnitude);
        yield return new WaitForSeconds(0.5f);
        _jumpRoutine = null;
    }

    private void Flip()
    {
        var desiredScale = _body.transform.localScale;
        desiredScale.x = ((_input.JoystickX > 0 ? -1 : 1) * Mathf.Abs(_body.transform.localScale.x));
        _body.transform.localScale = desiredScale;
    }
}

[System.Serializable]
public class Jump
{
    public bool IsOnFloor { get; private set; }

    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float MaxJumpMagnitude { get; private set; }

    [SerializeField] private LayerMask FloorLayers;
    [SerializeField] private float _checkRadius;
    [SerializeField] private Transform _checkPoint;

    public bool CheckOnFloor()
    {
        IsOnFloor = Physics2D.OverlapCircle(_checkPoint.position, _checkRadius, FloorLayers);
        return IsOnFloor;
    }

}
