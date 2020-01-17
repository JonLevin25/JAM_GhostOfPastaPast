using System;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _rend;
    [SerializeField] private CharHistoryBinder _historyBinder;
    [SerializeField] private LayerMask _throwableLayerMask;
    [SerializeField] private PlayerNum _playerNum;
    
    private Health _health;

    private void Start()
    {
        _health = PlayerConfig.GetConfig(_playerNum).PlayerHealth;
    }
    
    public void Init(CharStateHistory history, Color color, float secsDelay)
    {
        _historyBinder.SetHistorySource(history);
        _rend.color = color;
        _historyBinder.SetDelay(secsDelay);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var isThrowable = _throwableLayerMask.ContainsLayer(other.gameObject.layer);
        if (!isThrowable) return;
        
        var throwable = other.GetComponent<Throwable>();
        if (!throwable.IsHeldByPlayer)
        {
            _health.TakeDamage(1);
        }
    }
}