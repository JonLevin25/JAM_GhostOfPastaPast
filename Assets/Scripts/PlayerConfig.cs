using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    [SerializeField] PlayerNum _playerNum;
    [SerializeField] private MouseAimInput _mouseAim;
    [SerializeField] private Color _color;
    
    public GameObject PlayerRootObject;
    public Health PlayerHealth;
    public PlayerInput PlayerInput;
    public Color PlayerColor => _color;
    
    public PlayerNum PlayerNum => _playerNum;
    
    
    private static readonly Dictionary<PlayerNum, PlayerConfig> _configDict = new Dictionary<PlayerNum, PlayerConfig>();

    private void Awake()
    {
        _configDict[_playerNum] = this;
    }

    public static PlayerConfig GetConfig(PlayerNum playerNum) => _configDict[playerNum];

 
}
