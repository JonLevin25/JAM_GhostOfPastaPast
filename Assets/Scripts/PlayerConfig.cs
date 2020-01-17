using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    [SerializeField] PlayerNum _playerNum;
    [SerializeField] private MouseAimInput _mouseAim;
    [SerializeField] private Color _color;

    public Health PlayerHealth;
    public bool UseMouseForAim;
    public Color PlayerColor => _color;
    
    public const string HorizontalAxis = "MoveHor";
    public const string AimHorizontalAxis = "AimHor";
    public const string AimVerticalAxis = "AimVer";
    public const string JumpButton = "Jump";
    public const string ThrowButton = "Throw";

    private static readonly Dictionary<PlayerNum, PlayerConfig> ConfigDict = new Dictionary<PlayerNum, PlayerConfig>();

    private bool JoystickActive;

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
        var hasJoystick = rePlayer.controllers.joystickCount > 0;
        if (!hasJoystick && UseMouseForAim) return _mouseAim.GetAim();
        
        var rewiredAim = new Vector2(
            rePlayer.GetAxis(AimHorizontalAxis), 
            rePlayer.GetAxis(AimVerticalAxis));
        return rewiredAim;
    }

    private static Vector2 ComponentAbsMax(Vector2 a, Vector2 b)
    {
        var x = Mathf.Abs(a.x) > Mathf.Abs(b.x) ? a.x : b.x; 
        var y = Mathf.Abs(a.y) > Mathf.Abs(b.y) ? a.y : b.y; 
        return new Vector2(x, y);
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