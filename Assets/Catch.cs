using System;
using UnityEngine;

public class Catch : MonoBehaviour
{
    [SerializeField] private Transform _catchPosition;
    [SerializeField] private LayerMask _throwableLayerMask;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var isThrowable = ContainsLayer(_throwableLayerMask, other.gameObject.layer);
        if (!isThrowable) return;
        
        var throwable = other.GetComponent<Throwable>();
        if (throwable.IsHeldByPlayer) return;
        
        // TODO: WIP
    }

    private bool ContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}