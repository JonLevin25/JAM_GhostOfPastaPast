using UnityEngine;

public class CharAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharStateHistory _stateHistory;

    private const string AnimJump = "Jump";
    private const string AnimAirborne = "Airborne";
    private const string AnimRun = "Run";
    public void OnJump()
    {
        _animator.SetTrigger(AnimJump);
        _animator.SetBool(AnimAirborne, true);
    }

    public void OnGrounded()
    {
        
    }

    private void SetAnimBool(string animProp, bool value)
    {
        _animator.SetBool(animProp, value);
        _stateHistory
    }
}