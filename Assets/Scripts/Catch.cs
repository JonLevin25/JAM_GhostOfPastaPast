using UnityEngine;

public class Catch : MonoBehaviour
{
    [SerializeField]
    CharacterController2D player;
    [SerializeField] private Transform _catchPosition;
    [SerializeField] private LayerMask _throwableLayerMask;

    [Space]
    public Throwable HeldItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var isThrowable = _throwableLayerMask.ContainsLayer(other.gameObject.layer);
        if (!isThrowable) return;
        
        var throwable = other.GetComponent<Throwable>();
        if (!CanCatchItem(throwable)) return;

        CatchItem(throwable);
    }

    private void CatchItem(Throwable throwable)
    {
        HeldItem = throwable;
        throwable.Caught(_catchPosition);
        player.ItemCaught(throwable);
    }

    private bool CanCatchItem(Throwable throwable)
    {
        if (throwable == null) return false;
        if (HeldItem != null) return false; // If we already have an item in-hand
        if (throwable.IsHeldByPlayer) return false; // If the throwable is already held by a player

        return true;
    }

    public void SetAimDirection(Vector2 direction) {
        _catchPosition.localPosition = new Vector3(direction.x, direction.y, 0);
    }
}