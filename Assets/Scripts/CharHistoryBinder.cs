using System.Collections.Generic;
using UnityEngine;

public class CharHistoryBinder : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private CharStateHistory _stateHistory;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _rend;
    
    [Space]
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

    public void SetDelay(float delay) => _secsDelay = delay;

    private void Bind(CharState state)
    {
        _transform.localPosition = state.LocalPosition;
        _transform.localRotation = state.LocalRotation;
        _transform.localScale = state.LocalScale;
    }

    /// <summary>
    /// Get Most recent state
    /// ALSO ACTIVATES ANIM COMMANDS ALONG THE WAY
    /// </summary>
    private CharState MostRecentStateBefore(Queue<CharState> queue, float targetTime)
    {
        if (queue.Count == 0 || queue.Peek().Time > targetTime) return null; 
            
        CharState state = null;
        while (queue.Count > 0)
        {
            state = queue.Dequeue();
            HandleAnimCommnads(state.Commands);
            HandleRunDirection(state.Velocity);
            
            var nextTime = state.Time;
            if (nextTime < targetTime) break;
        }

        return state;
    }

    private void HandleRunDirection(Vector2 velocity)
    {
        var dir = velocity.GetDirection();
        if (dir == RunDirection.Idle) return;
        _rend.flipX = dir == RunDirection.Left;
    }

    private void HandleAnimCommnads(IEnumerable<AnimCommand> stateCommands)
    {
        if (stateCommands == null) return;
        foreach (var command in stateCommands)
        {
            var propName = command.PropName;
            if (command.IsTrigger)
            {
                if (command.PropValue) _animator.SetTrigger(propName);
                else _animator.ResetTrigger(propName);
            }
            else
            {
                _animator.SetBool(command.PropName, command.PropValue);
            }
        }
    }

    public void SetHistorySource(CharStateHistory history)
    {
        _stateHistory = history;
    }
}