using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    [SerializeField] PlayerNum _playerNum;
    public Health PlayerHealth;
    
    [Header("Input")]
    public string HorizontalAxis;
    public string VerticalAxis;
    public string AimHorizontalAxis;
    public string AimVerticalAxis;
    public string JumpButton;
    public string ThrowButton;

    private static readonly Dictionary<PlayerNum, PlayerConfig> ConfigDict = new Dictionary<PlayerNum, PlayerConfig>();

    private void Awake() => ConfigDict[_playerNum] = this;

    public static PlayerConfig GetConfig(PlayerNum playerNum) => ConfigDict[playerNum];
}