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

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _jumpAudio;
    [SerializeField] private AudioClip _landAudio;
    [SerializeField] private AudioClip _throwAudio;
    [SerializeField] private AudioClip _hitAudio;
    [SerializeField] private AudioClip _deathAudio;
    

    
    public const string AnimHit = "Hit";
    private const string AnimJump = "Jump";
    private const string AnimThrow = "Throw";
    private const string AnimAirborne = "Airborne";
    private const string AnimRun = "Run";
    private const string AnimDeath = "Death";

    private PlayerConfig _playerConfig;

    private void Awake()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
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
        PlaySFX(_jumpAudio);
    }

    public void SetGrounded(bool grounded)
    {
        var justLanded = grounded && _animator.GetBool(AnimAirborne);
        if (justLanded) PlaySFX(_landAudio);
        SetAnimBool(AnimAirborne, !grounded);
    }

    public void OnThrow()
    {
        SetAnimTrigger(AnimThrow);
        PlaySFX(_throwAudio);
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
        PlaySFX(_hitAudio);
    }

    private void OnDeath()
    {
        SetAnimTrigger(AnimDeath);
        PlaySFX(_deathAudio);
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
        if (rememberState) _stateHistory.AddAnimTrigger(triggerName);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        _audioSource.clip = clip;
        _audioSource.Play();
    }
}