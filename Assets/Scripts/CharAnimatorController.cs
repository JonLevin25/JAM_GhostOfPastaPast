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

    public void SetGrounded(bool grounded) => SetAnimBool(AnimAirborne, !grounded);

    public void SetRunning(bool running) => SetAnimBool(AnimRun, running);
    
    public void ConfigByVelocity(Vector2 velocity)
    {
        var isRunning = Mathf.Abs(velocity.x) > 0.1f;
        SetRunning(isRunning);
        
        var shouldFlip = velocity.x < 0f;
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
}