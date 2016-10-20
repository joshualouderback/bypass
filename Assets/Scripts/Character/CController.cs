using UnityEngine;
using UnityEngine.InputNew;
using System.Collections;
using System.Collections.Generic;

/*
 * Wants:
 * 	- Single ground jump
 * 	- Possible Wall Jumping
 *  - One Jetpack Air Dash
 * 		(When in air, if the player holds the jump button they will freeze in the air. 
 * 		 If they then release the jump button they will "air dash" in that direction.
 * 		 If the player hits the Ball, then they will recieve another charge to air dash again.)
 * 		Note: We may want to let the player automatically grab or hit the ball, if they dash into it.
 * 
 * Questions:
 * 	- Do we want to the ball to stick to a player if they run into it?
 * 		(Current feeling: yes)
 * 
*/
public class CController : MonoBehaviour {

	public float GroundMoveForce 	= 14.0f;
	public float AirMoveForce	  	= 8.0f;
	public float BonusAirMoveForce 	= 6.0f;
	public float JumpForce		  	= 14.0f;
	public float JetpackDashForce 	= 16.0f;
	public float HitForce 			= 16.0f;

	public float FallDelay = 0.1f;

	Rigidbody rb_;
	Rigidbody ball_;
	PlayerInput pInput_;
	ControlsActionMap controls_;
	Collision inCollision_;

	Coroutine HitBallRoutine;
	Coroutine DashRoutine;
	Coroutine GroundMoveRoutine;
	Coroutine AirMoveRoutine;
	Coroutine JumpRoutine;

	ActionSequence FallingSeq;
	bool canJump_ = false;
	bool canDash_ = true;

	public bool OnGround()
	{
		return canJump_;
	}
		
	void OnScore(ScoreEvent e)
	{
		Destroy(this.gameObject);
	}

	void OnEnable()
	{
		EventManager.Instance.Connect<ScoreEvent>(OnScore, null);
	}

	void OnDisable()
	{
		EventManager.Instance.Disconnect<ScoreEvent>(OnScore, null);
	}


	PlayerHandle handle;

	// Use this for initialization
	void Start () 
	{
		// Components
		rb_ = this.GetComponent<Rigidbody>();
		pInput_ = this.GetComponent<PlayerInput>();
		controls_ = pInput_.GetActions<ControlsActionMap>();

		/*
		PlayerHandle globalHandle = PlayerHandleManager.GetNewPlayerHandle();
		globalHandle.global = true;
		controls_.TryInitializeWithDevices(globalHandle.GetApplicableDevices());
		globalHandle.maps.Add(pInput_.actionMaps);

		List<InputDevice> devices = globalHandle.GetActions(controls_.actionMap).GetCurrentlyUsedDevices();
		// These are the devices currently active in the global player handle.
		handle = PlayerHandleManager.GetNewPlayerHandle();

		foreach (var device in devices)
		{
			handle.AssignDevice(device, true);
		}
		*/

		// Vars
		FallingSeq = new ActionSequence(this.gameObject);
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.tag == "Environment")
		{
			canJump_ = canDash_ = true;
			inCollision_ = collision;

			if(AirMoveRoutine != null)
			{
				StopCoroutine(AirMoveRoutine);
			}
	
			GroundMoveRoutine = StartCoroutine(GroundMovement());
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if(collision == inCollision_)
		{
			inCollision_ = null;
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.tag == "Ball")
		{
			ball_ = collider.GetComponentInParent<Rigidbody>();
			HitBallRoutine = StartCoroutine(HitBall());
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if(collider.transform.tag == "Ball")
		{
			ball_ = null;
			if(HitBallRoutine != null)
			{
				StopCoroutine(HitBallRoutine);
				HitBallRoutine = null;
			}
		}
	}

	IEnumerator WaitUntilFalling()
	{
		yield return new WaitUntil(() => (rb_.velocity.y < 0));
	}

	void TriggerFallingSequence()
	{
		rb_.useGravity = true;
		FallingSeq.Cancel();
		if(DashRoutine != null)
		{
			StopCoroutine(DashRoutine);
		}

		new ActionDelay(FallingSeq, 0.1f);
		new ActionCall(FallingSeq,
			() =>
			{
				DashRoutine = StartCoroutine(Dashing());
			});
		new ActionRoutine(FallingSeq, WaitUntilFalling);
		new ActionCall(FallingSeq, 
			() => 
			{
				rb_.useGravity = false;
			});
		new ActionDelay(FallingSeq, FallDelay);
		new ActionCall(FallingSeq, 
			() =>
			{
				rb_.useGravity = true;
			});
	}

	IEnumerator HitBall()
	{
		while(true)
		{
			if(controls_.tossPunch.wasJustPressed)
			{
				ball_.velocity = Vector3.zero;
				ball_.isKinematic = false;
				ball_.transform.SetParent(null);
				ball_.AddForce(new Vector3(controls_.move.value, controls_.upDown.value, 0) * HitForce, ForceMode.VelocityChange);
				canDash_ = true;
				yield return new WaitForSeconds(0.25f);
			}

			yield return null;
		}
	}

	//////////// MOVEMENT ////////////

	IEnumerator Dashing()
	{
		// As long as we are air borne
		while(!canJump_)
		{
			// If they press the jump button
			while(canDash_ && controls_.jumpDash.wasJustPressed)
			{
				// Cancel all other movements
				FallingSeq.Cancel();
				StopCoroutine(AirMoveRoutine);

				// As long as jump is held, they will hold in space
				while(controls_.jumpDash.isHeld)
				{
					rb_.useGravity = false;
					rb_.velocity = Vector3.zero;
					yield return null;
				}
					
				// Once released, push them in the aimed direction
				rb_.AddForce(new Vector3(controls_.move.value, controls_.upDown.value, 0).normalized * JetpackDashForce, ForceMode.VelocityChange);
				rb_.useGravity = true;
				canDash_ = false;

				// Re-enable air movement
				AirMoveRoutine = StartCoroutine(AirMovement());

				yield return null;
			}

			yield return null;
		}
	}

	IEnumerator Jumping()
	{
		while(canJump_)
		{
			if(controls_.jumpDash.wasJustPressed)
			{
				Vector3 jumpDirection = this.transform.up;

				// If we are on a wall
				if(inCollision_ != null)
				{
					jumpDirection += inCollision_.contacts[0].normal;
					jumpDirection.Normalize();
				}

				
				rb_.AddForce(jumpDirection * JumpForce, ForceMode.VelocityChange);
				canJump_ = false;
				TriggerFallingSequence();
				AirMoveRoutine = StartCoroutine(AirMovement());
			}

			yield return null;
		}
	}

	IEnumerator AirMovement()
	{
		int prevDir = 0;

		while(!canJump_)
		{
			float step = 0.0f;
			float toAdd = Time.deltaTime * 0.125f;
		
			while((int)(controls_.move.value) + prevDir != 0)
			{
				float moveFactor = AirMoveForce + (BonusAirMoveForce * step);
				rb_.velocity = new Vector3(Mathf.Lerp(rb_.velocity.x, controls_.move.value * moveFactor, step), rb_.velocity.y, rb_.velocity.z);
				prevDir = (int)(controls_.move.value);
				step += toAdd;
				Mathf.Min(step, 1.0f);

				yield return null;
			}

			yield return null;
		}
	}

	IEnumerator GroundMovement()
	{
		JumpRoutine = StartCoroutine(Jumping());

		while(canJump_)
		{
			rb_.velocity = new Vector3(controls_.move.value * GroundMoveForce, rb_.velocity.y, rb_.velocity.z);

			yield return null;
		}
	}

	// Update is called once per frame
	void Update () 
	{
	}


}
