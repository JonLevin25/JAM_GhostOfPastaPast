using Rewired;
using UnityEngine;

public static class Extensions
{
    public static RunDirection GetDirection(this Vector2 velocity)
    {
        if (Mathf.Approximately(velocity.x, 0f)) return RunDirection.Idle;
        return velocity.x > 0f ? RunDirection.Right : RunDirection.Left;
    }
    
    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static Rewired.Player GetPlayer(this PlayerNum playerNum)
    {
        return ReInput.players.GetPlayer((int) playerNum);
    }
}