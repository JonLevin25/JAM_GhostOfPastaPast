using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private PlayerNum _playerNum;
    [SerializeField] private LayerMask _trapLayerMask;
    private Health _health;

    private void Start()
    {
        var config = PlayerConfig.GetConfig(_playerNum);
        _health = config.PlayerHealth;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var isTrap = _trapLayerMask.ContainsLayer(other.gameObject.layer);
        if (!isTrap) return;
        
        _health.TakeDamage(1);
    }
}
