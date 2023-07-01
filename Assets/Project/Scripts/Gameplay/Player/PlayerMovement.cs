using UnityEngine;

[RequireComponent(typeof(Input))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Rigidbody2D _rigidbody;

    [SerializeField] private Jump _jumpData;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _movementSpeedWhenJump;
    [SerializeField] private float _maxMagnitude;

    private Vector2 _direction;

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

    private void FixedUpdate()
    {
        _jumpData.CheckOnFloor();
        TryMove();
    }

    private void TryMove()
    {
        if (_input.MovementInputState != PlayerInput.MovementInput.Stay)
            Move();
    }

    private void Move()
    {
        if (_input.MovementInputState == PlayerInput.MovementInput.MoveRight)
        {
            _direction = transform.right;
        }
        else if (_input.MovementInputState == PlayerInput.MovementInput.MoveLeft)
        {
            _direction = -transform.right;
        }
        else
        {
            return;
        }

        var speed = _jumpData.IsOnFloor ? _movementSpeed : _movementSpeedWhenJump;
        _rigidbody.AddForce(_direction * speed, ForceMode2D.Force);

        if (_rigidbody.velocity.magnitude > _maxMagnitude)
            _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _maxMagnitude);
    }

    private void TryJump()
    {
        if (_jumpData.IsOnFloor)
            _rigidbody.AddForce(transform.up * _jumpData.JumpForce, ForceMode2D.Force);
    }

    private void Flip()
    {
        transform.localRotation = Quaternion.Euler(_direction);
    }
}

[System.Serializable]
public class Jump
{
    public bool IsOnFloor { get; private set; }

    [field: SerializeField] public float JumpForce { get; private set; }

    [SerializeField] private LayerMask FloorLayers;
    [SerializeField] private float _checkRadius;
    [SerializeField] private Transform _checkPoint;

    public bool CheckOnFloor()
    {
        IsOnFloor = Physics2D.OverlapCircle(_checkPoint.position, _checkRadius, FloorLayers);
        return IsOnFloor;
    }

}
