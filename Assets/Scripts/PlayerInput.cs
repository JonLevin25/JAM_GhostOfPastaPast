using Rewired;
using UnityEngine;

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


public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerNum _playerNum;
    [SerializeField] private MouseAimInput _mouseAim;
    [SerializeField] private bool _useMouseForAim;
    
    public const string HorizontalAxis = "MoveHor";
    public const string AimHorizontalAxis = "AimHor";
    public const string AimVerticalAxis = "AimVer";
    public const string JumpButton = "Jump";
    public const string ThrowButton = "Throw";
    
    private bool JoystickActive;

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
        if (!hasJoystick && _useMouseForAim) return _mouseAim.GetAim();
        
        var rewiredAim = new Vector2(
            rePlayer.GetAxis(AimHorizontalAxis), 
            rePlayer.GetAxis(AimVerticalAxis));
        return rewiredAim;
    }
}
