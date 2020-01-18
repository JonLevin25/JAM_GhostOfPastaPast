using System.Collections;
using UnityEngine;

public class GhostInstantiator : MonoBehaviour
{
    [SerializeField] private PlayerNum _playerNum;
    [SerializeField] private float _secsDelay;
    [SerializeField] private GhostController _ghostPrefab;
    [SerializeField] private CharStateHistory _stateHistorySource;

    private void Awake()
    {
        PlayersManager.OnPlayerEnteredGame += OnPlayerEnteredGame;
    }
    
    private void OnDestroy()
    {
        PlayersManager.OnPlayerEnteredGame -= OnPlayerEnteredGame;
    }

    private void OnPlayerEnteredGame(PlayerNum playerNum)
    {
        if (playerNum != _playerNum) return;
        StartCoroutine(WaitThenInstantiate());
    }

    private IEnumerator WaitThenInstantiate()
    {
        yield return new WaitForSeconds(_secsDelay);
        var ghost = Instantiate(_ghostPrefab, transform.parent);
        
        Debug.Log($"Instantiated ghost ({ghost.name})");
        ghost.Init(_stateHistorySource, _secsDelay);
    }
}