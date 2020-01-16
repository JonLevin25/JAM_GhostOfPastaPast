using System.Collections.Generic;
using UnityEngine;

public class CharState
{
    public readonly Vector3 LocalPosition;
    public readonly Quaternion LocalRotation;
    public readonly Vector3 LocalScale;
    public float Time;

    public CharState(Vector3 localPosition, Quaternion localRotation, Vector3 localScale, float time)
    {
        LocalPosition = localPosition;
        LocalRotation = localRotation;
        LocalScale = localScale;
        Time = time;
    }
}

public class CharStateHistory : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    
    public Queue<CharState> StateHistory = new Queue<CharState>();

    private void Update()
    {
        var newState = new CharState(
            _transform.localPosition, 
            _transform.localRotation, 
            _transform.localScale, 
            Time.time);
        
        StateHistory.Enqueue(newState);
    }
}