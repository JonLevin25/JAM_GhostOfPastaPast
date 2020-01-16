using System.Collections.Generic;
using UnityEngine;

public class CharHistoryBinder : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private CharStateHistory _stateHistory;
    [SerializeField] private float _secsDelay;

    private void Update()
    {
        var queue = _stateHistory.StateHistory;
        // Get Most recent update that's at least <_secsDelay> behind time
        var targetTime = Time.time - _secsDelay;
        var state = MostRecentStateBefore(queue, targetTime);
        if (state != null)
        {
            Bind(state);
        }
    }

    private void Bind(CharState state)
    {
        _transform.localPosition = state.LocalPosition;
        _transform.localRotation = state.LocalRotation;
        _transform.localScale = state.LocalScale;
    }

    private static CharState MostRecentStateBefore(Queue<CharState> queue, float targetTime)
    {
        if (queue.Count == 0 || queue.Peek().Time > targetTime) return null; 
            
        CharState state = null;
        while (queue.Count > 0)
        {
            state = queue.Dequeue();

            var nextTime = state.Time;
            if (nextTime < targetTime) break;
        }

        return state;
    }
}