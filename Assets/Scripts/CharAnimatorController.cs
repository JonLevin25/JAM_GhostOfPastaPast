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
        SetAnimBool(AnimJump, true);
        SetAnimBool(AnimAirborne, true);
    }

    public void OnGrounded() => SetAnimBool(AnimAirborne, false);

    public void SetRunning(bool running) => SetAnimBool(AnimRun, running);

    private void SetAnimBool(string animProp, bool value)
    {
        _animator.SetBool(animProp, value);
        _stateHistory.AddAnimBool(animProp, value);
    }
}