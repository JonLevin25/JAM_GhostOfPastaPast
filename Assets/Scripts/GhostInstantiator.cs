using System.Collections;
using UnityEngine;

public class GhostInstantiator : MonoBehaviour
{
    [SerializeField] private float _secsDelay;
    [SerializeField] private Color _ghostColor;
    [SerializeField] private GhostController _ghostPrefab;
    [SerializeField] private CharStateHistory _stateHistorySource;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_secsDelay);
        var ghost = Instantiate(_ghostPrefab, transform.parent);
        ghost.Init(_stateHistorySource, _ghostColor, _secsDelay);
    }
}