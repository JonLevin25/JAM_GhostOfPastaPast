using UnityEngine;

public class Catch : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField] private Transform _catchPosition;
    [SerializeField] private LayerMask _throwableLayerMask;

    [Space]
    public Throwable HeldItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var isThrowable = ContainsLayer(_throwableLayerMask, other.gameObject.layer);
        if (!isThrowable) return;
        
        var throwable = other.GetComponent<Throwable>();
        if (!CanCatchItem(throwable)) return;

        CatchItem(throwable);
    }

    private void CatchItem(Throwable throwable)
    {
        HeldItem = throwable;
        throwable.transform.SetParent(_catchPosition, worldPositionStays: false);
        throwable.transform.localPosition = Vector3.zero;
        throwable.transform.rotation = Quaternion.identity;
        throwable.rigidBody.velocity = Vector2.zero;
        throwable.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<CharacterController2D>().item = HeldItem;
    }

    private bool CanCatchItem(Throwable throwable)
    {
        if (throwable == null) return false;
        if (HeldItem != null) return false; // If we already have an item in-hand
        if (throwable.IsHeldByPlayer) return false; // If the throwable is already held by a player

        return true;
    }

    private bool ContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}