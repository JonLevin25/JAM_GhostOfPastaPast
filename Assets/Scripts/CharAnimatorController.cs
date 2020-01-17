using System;
using UnityEngine;

public enum RunDirection
{
    Right,
    Idle,
    Left
}

public class CharAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharStateHistory _stateHistory;
    [SerializeField] private SpriteRenderer _rend;

    private const string AnimJump = "Jump";
    private const string AnimAirborne = "Airborne";
    private const string AnimRun = "Run";

    [SerializeField] private AudioClip _jumpAudio;
    [SerializeField] private AudioClip _landAudio;

    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        
    }


    public void OnJump()
    {
        SetAnimTrigger(AnimJump);
        SetAnimBool(AnimAirborne, true);
        _audioSource.clip = _jumpAudio;
        _audioSource.Play();
    }

    public void SetGrounded(bool grounded)
    {
        if (grounded && _animator.GetBool(AnimAirborne) == false)
        {
            // _audioSource.clip = _landAudio;
            // _audioSource.Play();
        }
        SetAnimBool(AnimAirborne, !grounded);
    }

    public void SetRunning(bool running) => SetAnimBool(AnimRun, running);
    
    public void ConfigByVelocity(Vector2 velocity)
    {
        var isRunning = Mathf.Abs(velocity.x) > 0.1f;
        SetRunning(isRunning);

        var newDirection = velocity.GetDirection();
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
}

public static class Extensions
{
    public static RunDirection GetDirection(this Vector2 velocity)
    {
        if (Mathf.Approximately(velocity.x, 0f)) return RunDirection.Idle;
        return velocity.x > 0f ? RunDirection.Right : RunDirection.Left;
    }
}