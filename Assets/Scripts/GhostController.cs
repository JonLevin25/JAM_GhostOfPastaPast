using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _rend;
    [SerializeField] private CharHistoryBinder _historyBinder;
    [SerializeField] private LayerMask _throwableLayerMask;
    [SerializeField] private PlayerNum _playerNum;

    public Color TEST_COlor;
    
    private PlayerConfig _playerConfig;

    private void Start()
    {
        _playerConfig = PlayerConfig.GetConfig(_playerNum);
        SetColor(_playerConfig.PlayerColor, 0.4f);
    }

    public void Init(CharStateHistory history, float secsDelay)
    {
        _historyBinder.SetHistorySource(history);
        _historyBinder.SetDelay(secsDelay);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var isThrowable = _throwableLayerMask.ContainsLayer(other.gameObject.layer);
        if (!isThrowable) return;
        
        var throwable = other.GetComponent<Throwable>();
        if (!throwable.IsHeldByPlayer)
        {
            _playerConfig.PlayerHealth.TakeDamage(1);
        }
    }

    private void SetColor(Color color, float alpha)
    {
        color.a = alpha;
        TEST_COlor = color;
        _rend.color = color;
    }
}