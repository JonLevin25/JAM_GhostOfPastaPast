using UnityEngine;

public class CharAnimatorController : MonoBehaviour
{
    public enum RunDirection
    {
        Right,
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

    public void OnGrounded() => SetAnimBool(AnimAirborne, false);

    public void SetRunning(bool running) => SetAnimBool(AnimRun, running);
    public void SetDirection(RunDirection direction)
    {
        var shouldFlip = direction == RunDirection.Left;
        _rend.flipX = shouldFlip;
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

    public void SetGrounded(bool grounded)
    {
        // WIP
    }
}