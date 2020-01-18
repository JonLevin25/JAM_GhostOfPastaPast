using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] 
    public Rigidbody2D rigidBody;
    public bool IsHeldByPlayer;

    public void Throw(Vector2 throwVector) 
    {
        rigidBody.isKinematic = false;
        rigidBody.velocity = throwVector;
        transform.SetParent(null, worldPositionStays: true);
        IsHeldByPlayer = false;
    }

    public void Caught(Transform newParent) 
    {
        transform.SetParent(newParent, worldPositionStays: false);
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rigidBody.velocity = Vector2.zero;
        rigidBody.isKinematic = true;
        IsHeldByPlayer = true;
    }
}