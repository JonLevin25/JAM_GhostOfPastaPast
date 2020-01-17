using System.Collections.Generic;
using UnityEngine;

public class AnimCommand
{
    public readonly string PropName;
    public readonly bool PropValue;
    public readonly bool IsTrigger;

    public AnimCommand(string propName, bool propValue, bool isTrigger)
    {
        PropName = propName;
        PropValue = propValue;
        IsTrigger = isTrigger;
    }
}

public class CharState
{
    public readonly Vector3 LocalPosition;
    public readonly Quaternion LocalRotation;
    public readonly Vector3 LocalScale;
    public float Time;
    private List<AnimCommand> _commands = new List<AnimCommand>();
    public IEnumerable<AnimCommand> Commands => _commands;

    public CharState(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, float time)
    {
        LocalPosition = localPosition;
        LocalRotation = localRotation;
        LocalScale = localScale;
        Time = time;
    }

    public void AddCommand(AnimCommand command)
    {
        _commands.Add(command);
    }
    
    public void AddAnimCommands(IEnumerable<AnimCommand> command)
    {
        _commands.AddRange(command);
    }
}

public class CharStateHistory : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    public Queue<CharState> StateHistory = new Queue<CharState>();

    private List<AnimCommand> _lastFrameAnimCommands = new List<AnimCommand>();
    
    public void AddAnimBool(string boolName, bool value)
    {
        var command = new AnimCommand(boolName, value, isTrigger: false);
        _lastFrameAnimCommands.Add(command);
    }

    public void AddAnimTrigger(string triggerName)
    {
        var command = new AnimCommand(triggerName, true, isTrigger: true);
        _lastFrameAnimCommands.Add(command);
    }

    private void Update()
    {
        var newState = new CharState(
            _transform.localPosition, _transform.localRotation, _transform.localScale, Time.time);
        
        // Add animation commands
        newState.AddAnimCommands(_lastFrameAnimCommands);
        _lastFrameAnimCommands.Clear();

        StateHistory.Enqueue(newState);
    }
}