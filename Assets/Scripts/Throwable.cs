using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] 
    private Rigidbody2D rigidBody;
    public bool IsHeldByPlayer;

    public void Throw(Vector2 throwVector) 
    {
        rigidBody.velocity = throwVector;
    }
}