using UnityEngine;

public class CharAnimatorController : MonoBehaviour
{
    public enum RunDirection
    {
        Right,
        Idle,
        Left
    }
    
    [SerializeField] private Animator _animator;
    [SerializeField] private CharStateHistory _stateHistory;
    [SerializeField] private SpriteRenderer _rend;

    private const string AnimJump = "Jump";
    private const string AnimAirborne = "Airborne";
    private const string AnimRun = "Run";

    public void OnJump()
    {
        SetAnimTrigger(AnimJump);
        SetAnimBool(AnimAirborne, true);
    }

    public void SetGrounded(bool grounded) => SetAnimBool(AnimAirborne, !grounded);

    public void SetRunning(bool running) => SetAnimBool(AnimRun, running);
    
    public void ConfigByVelocity(Vector2 velocity)
    {
        var isRunning = Mathf.Abs(velocity.x) > 0.1f;
        SetRunning(isRunning);

        var newDirection = GetDirection(velocity);
        if (newDirection != RunDirection.Idle)
        {
            _rend.flipX = newDirection == RunDirection.Left;
        }
    }

    private void SetAnimBool(string animProp, bool value)
    {
        _animator.SetBool(animProp, value);
        _stateHistory.AddAnimBool(animProp, value);
    }

    private void SetAnimTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
        _stateHistory.AddAnimTrigger(triggerName);
    }

    private RunDirection GetDirection(Vector2 velocity)
    {
        if (Mathf.Approximately(velocity.x, 0f)) return RunDirection.Idle;
        return velocity.x > 0f ? RunDirection.Right : RunDirection.Left;
    }
}