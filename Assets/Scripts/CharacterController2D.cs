using System;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
	[Header("Basic Config")]
	[SerializeField] private PlayerNum _playerNum;
	[SerializeField] private LayerMask platformLayerMask;
	
	[Header("References")]
	[SerializeField] private Rigidbody2D body;
	[SerializeField] private Transform legs;
	[SerializeField] private CharAnimatorController _animController;
	[SerializeField] private Catch _catch;
	[SerializeField] private GameObject crosshair;
	
	[Header("Configuration")]
	[Tooltip("Max speed, in units per second, that the character moves.")]
    [SerializeField] float speed = 9;
	[Tooltip("Max height the character will jump regardless of gravity")]
    [SerializeField] float jumpHeight = 4;
    [SerializeField] float throwForce = 20;
    
	[SerializeField] float crosshairDistance = 4;


	public Throwable item;
	
    private BoxCollider2D boxCollider;

	private Vector2 velocity;

	private Vector2 aimDirection = new Vector2(-1,0);
	private PlayerConfig playerConfig;
	private bool dead;

	private void Start()
	{
		playerConfig = PlayerConfig.GetConfig(_playerNum);
		playerConfig.PlayerHealth.OnDeath += OnDeath;
	}

	private void Update()
	{
		 if (dead) return;
		var inputPayload = playerConfig.PlayerInput.GetInput();
		var newVelocity = new Vector2(0f, body.velocity.y);
		
		var newAimDirection = inputPayload.Aim;
		if (newAimDirection.x != 0 || newAimDirection.y != 0) {
			newAimDirection.Normalize();
			aimDirection = newAimDirection;
			crosshair.transform.localPosition = new Vector3(aimDirection.x * crosshairDistance, aimDirection.y * crosshairDistance, 0);
			_catch.SetAimDirection(aimDirection);
		}

		if (inputPayload.Throw)
		{
			if (item) 
			{
				_animController.OnThrow();
				item.Throw(aimDirection * throwForce);
				item = null;
				_catch.HeldItem = null;
			}
		}
		
		var grounded = Physics2D.Raycast(legs.position, -legs.up, 0.1f, platformLayerMask);
		if (grounded && inputPayload.Jump)
		{
			// Calculate the velocity required to achieve the target jump height.
			newVelocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
			_animController.OnJump();
		}

		// Update the velocity assignment statements to use our selected
		// acceleration and deceleration values.
		newVelocity.x =  speed * inputPayload.MoveHorizontal;
		body.velocity = newVelocity;
		
		// Set Animation state
		_animController.SetGrounded(grounded);
		_animController.ConfigByVelocity(newVelocity);
    }

	private void OnDeath()
	{
		body.velocity = Vector2.zero;
		dead = true;
	}

	public void ItemCaught(Throwable caughtItem) {
		item = caughtItem;
		_catch.SetAimDirection(aimDirection);
	}
}
