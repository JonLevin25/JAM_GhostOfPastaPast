using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

	[SerializeField]
	float throwForce = 5;

	[SerializeField]
	float crosshairDistance = 4;

	[SerializeField]
	GameObject crosshair;

	[SerializeField]
	LayerMask platformLayerMask;

	[SerializeField]
	Rigidbody2D body;

	[SerializeField]
	Transform legs;

	Throwable item;
	
    private BoxCollider2D boxCollider;

    private Vector2 velocity;
	
	private bool grounded;

	private Vector2 aimDirection;

    private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
		float moveInput = Input.GetAxis("Horizontal");
		Vector2 newVelocity = new Vector2(0f, body.velocity.y);
		
		Vector2 newAimDirection = new Vector2(Input.GetAxis("aimHorizontal"), Input.GetAxis("aimVertical"));
		if (newAimDirection.x != 0 || newAimDirection.y != 0) {
			newAimDirection.Normalize();
			aimDirection = newAimDirection;
			crosshair.transform.localPosition = new Vector3(aimDirection.x * crosshairDistance, aimDirection.y * crosshairDistance, 0);
		}

		if (Input.GetButtonDown("Throw"))
		{
			if (item) 
			{
				item.Throw(aimDirection * throwForce);
				item = null;
			}
		}

		// jump
		if (grounded)
		{
			if (Input.GetButtonDown("Jump"))
			{
				// Calculate the velocity required to achieve the target jump height.
				newVelocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
			}
		}
		
		// horizontal movement
		float acceleration = grounded ? walkAcceleration : airAcceleration;
		float deceleration = grounded ? groundDeceleration : 0;

		// Update the velocity assignment statements to use our selected
		// acceleration and deceleration values.
		newVelocity.x =  speed * moveInput;
		body.velocity = newVelocity;
		
		// collision detection
		Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0, platformLayerMask);
		
		grounded = Physics2D.Raycast(legs.position, legs.up * -1.0f, 0.1f, platformLayerMask);
    }
}
