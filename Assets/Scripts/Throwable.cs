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
    }
}