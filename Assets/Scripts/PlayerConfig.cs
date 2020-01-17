using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    [SerializeField] PlayerNum _playerNum;
    [SerializeField] private MouseAimInput _mouseAim;
    public Health PlayerHealth;
    public bool UseMouseForAim;
    
    public const string HorizontalAxis = "MoveHor";
    public const string AimHorizontalAxis = "AimHor";
    public const string AimVerticalAxis = "AimVer";
    public const string JumpButton = "Jump";
    public const string ThrowButton = "Throw";

    private static readonly Dictionary<PlayerNum, PlayerConfig> ConfigDict = new Dictionary<PlayerNum, PlayerConfig>();

    private void Awake() => ConfigDict[_playerNum] = this;

    public static PlayerConfig GetConfig(PlayerNum playerNum) => ConfigDict[playerNum];

    public PlayerInputPayload GetInput()
    {
        var rePlayer = _playerNum.GetPlayer();
        var move = rePlayer.GetAxis(HorizontalAxis);
        var aim = GetAim(rePlayer);
        var jump = rePlayer.GetButtonDown(JumpButton);
        var @throw = rePlayer.GetButtonDown(ThrowButton);
        
        return new PlayerInputPayload(move, aim, jump, @throw);
    }

    private Vector2 GetAim(Player rePlayer)
    {
        if (UseMouseForAim && _mouseAim != null) return _mouseAim.GetAim();

        return new Vector2(
            rePlayer.GetAxis(AimHorizontalAxis), 
            rePlayer.GetAxis(AimVerticalAxis));
    }
}

public struct PlayerInputPayload
{
    public readonly float MoveHorizontal;
    public readonly Vector2 Aim;
    public readonly bool Jump;
    public readonly bool Throw;

    public PlayerInputPayload(float moveHorizontal, Vector2 aim, bool jump, bool @throw)
    {
        MoveHorizontal = moveHorizontal;
        Aim = aim;
        Jump = jump;
        Throw = @throw;
    }
}