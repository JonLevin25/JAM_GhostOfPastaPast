using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

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

	private Vector2 velocity;
	
	private bool grounded;

	private Vector2 aimDirection;

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
		
		// Update the velocity assignment statements to use our selected
		// acceleration and deceleration values.
		newVelocity.x =  speed * moveInput;
		body.velocity = newVelocity;
		
		grounded = Physics2D.Raycast(legs.position, -legs.up, 0.1f, platformLayerMask);
    }
}
