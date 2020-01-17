using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _rend;
    [SerializeField] private CharHistoryBinder _historyBinder;
    
    public void Init(CharStateHistory history, Color color, float secsDelay)
    {
        _historyBinder.SetHistorySource(history);
        _rend.color = color;
        _historyBinder.SetDelay(secsDelay);
    }
}