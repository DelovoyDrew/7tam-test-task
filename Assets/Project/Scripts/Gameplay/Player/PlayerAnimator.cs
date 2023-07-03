using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private static readonly int RUN = Animator.StringToHash("Run");
    private static readonly int IDLE = Animator.StringToHash("Edle");
    private static readonly int JUMP = Animator.StringToHash("Jump");
    private static readonly int SHOOT = Animator.StringToHash("Shoot");

    private int _current;

    public enum States
    {
        Run,
        Idle,
        Jump,
        Shoot
    }

    public static int StateToInt(States state)
    {
        switch (state)
        {
            case States.Idle:
                return IDLE;
            case States.Run:
                return RUN;
            case States.Jump:
                return JUMP;
            case States.Shoot:
                return SHOOT;
        }

        return 0;
    }

    public void ChangeState(States state)
    {
        if(StateToInt(state) == _current)
        {
            if(state == States.Idle || state == States.Run)
            {
                return;
            }
        }

        _animator.SetBool(_current, false);
        _current = StateToInt(state);

        if(state == States.Shoot)
            _animator.SetTrigger(_current);
        else
            _animator.SetBool(_current, true);
    }

}
