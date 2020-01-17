using UnityEngine;

public enum RunDirection
{
    Right,
    Idle,
    Left
}

public class CharAnimatorController : MonoBehaviour
{
    [SerializeField] private PlayerNum _playerNum;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharStateHistory _stateHistory;
    [SerializeField] private SpriteRenderer _rend;

    public const string AnimHit = "Hit";
    
    private const string AnimJump = "Jump";
    private const string AnimAirborne = "Airborne";
    private const string AnimRun = "Run";
    private const string AnimDeath = "Death";

    [SerializeField] private AudioClip _jumpAudio;
    [SerializeField] private AudioClip _landAudio;

    [SerializeField] private AudioSource _audioSource;
    private PlayerConfig _playerConfig;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _playerConfig = PlayerConfig.GetConfig(_playerNum);
        _rend.color = _playerConfig.PlayerColor;
        _playerConfig.PlayerHealth.OnHpChanged += OnHpChanged;
        _playerConfig.PlayerHealth.OnDeath += OnDeath;
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

    private void OnHpChanged(int currhp, int totalhp, int delta)
    {
        if (delta < 0f) OnHit();
    }

    private void OnHit()
    {
        SetAnimTrigger(AnimHit, false);
    }

    private void OnDeath()
    {
        SetAnimTrigger(AnimDeath);
    }

    private void SetAnimBool(string animProp, bool value)
    {
        _animator.SetBool(animProp, value);
        _stateHistory.AddAnimBool(animProp, value);
    }

    /// <summary>
    /// Set a trigger in the animator
    /// </summary>
    /// <param name="rememberState"> Should the trigger be recorded for playback in the ghost later?</param>
    private void SetAnimTrigger(string triggerName, bool rememberState = true)
    {
        _animator.SetTrigger(triggerName);
        _stateHistory.AddAnimTrigger(triggerName);
    }
}