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
			velocity.y = 0;
			
			if (Input.GetButtonDown("Jump"))
			{
				// Calculate the velocity required to achieve the target jump height.
				velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
			}
			
		}
		
		// fall down when in the air
		velocity.y += Physics2D.gravity.y * Time.deltaTime;
		
		// horizontal movement
		float acceleration = grounded ? walkAcceleration : airAcceleration;
		float deceleration = grounded ? groundDeceleration : 0;

		// Update the velocity assignment statements to use our selected
		// acceleration and deceleration values.
		if (moveInput != 0)
		{
			velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
		}
		else
		{
			velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
		}
		transform.Translate(velocity * Time.deltaTime);
		
		// collision detection
		Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
		
		grounded = false;
		foreach (Collider2D hit in hits)
		{
			if (hit == boxCollider)
				continue;

			ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

			if (colliderDistance.isOverlapped)
			{
				// stop moving on collision
				transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
				
				// detect grounding:  angle between the collision normal and the world up is below 90 and veolcity is downwards
				if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
				{
					grounded = true;
				}
			}
		}
    }
}
