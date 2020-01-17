﻿using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private PlayerNum _playerNum;
	
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

	[SerializeField]
	float throwForce = 20;

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

	[SerializeField] 
	private CharAnimatorController _animController;

	[SerializeField]
	Catch _catch;


	public Throwable item;
	
    private BoxCollider2D boxCollider;

	private Vector2 velocity;

	private Vector2 aimDirection = new Vector2(-1,0);
	private string horAxis;
//	private string verAxis;
	private string aimHorAxis;
	private string aimVerAxis;
	private string jumpButton;
	private string throwButton;
	
	private void Start()
	{
		var config = PlayerConfig.GetConfig(_playerNum);
		horAxis = config.HorizontalAxis;
//		verAxis = config.VerticalAxis;
		aimHorAxis = config.AimHorizontalAxis;
		aimVerAxis = config.AimVerticalAxis;
		jumpButton = config.JumpButton;
		throwButton = config.ThrowButton;
	}

	private void Update()
    {
		float moveInput = Input.GetAxis(horAxis);
		Vector2 newVelocity = new Vector2(0f, body.velocity.y);
		
		Vector2 newAimDirection = new Vector2(Input.GetAxis(aimHorAxis), Input.GetAxis(aimVerAxis));
		if (newAimDirection.x != 0 || newAimDirection.y != 0) {
			newAimDirection.Normalize();
			aimDirection = newAimDirection;
			crosshair.transform.localPosition = new Vector3(aimDirection.x * crosshairDistance, aimDirection.y * crosshairDistance, 0);
			_catch.SetAimDirection(aimDirection);
		}

		if (Input.GetButtonDown(throwButton))
		{
			if (item) 
			{
				item.Throw(aimDirection * throwForce);
				item = null;
				_catch.HeldItem = null;
			}
		}
		
		var grounded = Physics2D.Raycast(legs.position, -legs.up, 0.1f, platformLayerMask);
		if (grounded && Input.GetButtonDown(jumpButton))
		{
			// Calculate the velocity required to achieve the target jump height.
			newVelocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
			_animController.OnJump();
		}

		// Update the velocity assignment statements to use our selected
		// acceleration and deceleration values.
		newVelocity.x =  speed * moveInput;
		body.velocity = newVelocity;
		
		// Set Animation state
		_animController.SetGrounded(grounded);
		_animController.ConfigByVelocity(newVelocity);
    }

	public void ItemCaught(Throwable caughtItem) {
		item = caughtItem;
		_catch.SetAimDirection(aimDirection);
	}
}
