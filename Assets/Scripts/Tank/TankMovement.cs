using UnityEngine;

public class TankMovement : MonoBehaviour
{
	public readonly float speedPerFrame = 12f;

	public int playerNumber = 1;

	public AudioSource movementAudio;
	public AudioClip engineIdling;
	public AudioClip engineDriving;


	private new Rigidbody rigidbody;

#if UNITY_ANDROID || UNITY_IOS
	private Vector2 touchStartPosition;
	private Vector2 movementDirection;
#elif UNITY_WEBPLAYER || UNITY_STANDALONE
	public readonly float maxSpeed = 1f;
	public readonly float turnSpeed = 180f;

	private string movementAxisName;
	private string turnAxisName;

	private float movementInputValue;
	private float turnInputValue;
#endif


	private void Awake() => this.rigidbody = this.GetComponent<Rigidbody>();


	private void OnEnable() {
		this.rigidbody.isKinematic = false;
		
		this.ResetInput();
	}


	private void OnDisable() => this.rigidbody.isKinematic = true;


	private void Update() {
		// Store the player's input and make sure the audio for the engine is playing.

		this.GetInput();
		this.UpdateEngineAudio();
	}


	private void UpdateEngineAudio() {
		// Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

		if (this.IsMoving()) {
			if (this.movementAudio.clip == this.engineIdling) {
				this.movementAudio.clip = this.engineDriving;
				this.movementAudio.Play();
			}
		}
		else {
			if (this.movementAudio.clip == this.engineDriving) {
				this.movementAudio.clip = this.engineIdling;
				this.movementAudio.Play();
			}
		}
	}


#if UNITY_ANDROID || UNITY_IOS
	private void FixedUpdate() {
		if (this.IsMoving()) {
			float distancePerFrame = this.speedPerFrame * Time.deltaTime;
			Vector3 movement = new Vector3(this.movementDirection.x, 0f, this.movementDirection.y) * distancePerFrame;
			this.rigidbody.MovePosition(this.rigidbody.position + movement);

			float angleFromStraight = Vector2.SignedAngle(this.movementDirection, Vector2.up);
			this.rigidbody.MoveRotation(Quaternion.Euler(0f, angleFromStraight, 0f));
		}
	}


	private void ResetInput() {
		this.touchStartPosition = Vector2.zero;
		this.movementDirection = Vector2.zero;
	}


	private void GetInput() {
		Touch? touchInput = this.GetTouch();

		if (touchInput.HasValue) {
			Touch touch = touchInput.Value;

			if (touch.phase == TouchPhase.Began)
				this.touchStartPosition = touch.position;
			else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
				this.movementDirection = (touch.position - this.touchStartPosition).normalized;
			else if (touch.phase == TouchPhase.Ended)
				this.ResetInput();
			else
				Debug.Assert(touch.phase == TouchPhase.Canceled);
		}
		else {
			Debug.Assert(!touchInput.HasValue);

			this.ResetInput();
		}
	}


	private Touch? GetTouch() {
		for (int i = 0; i < Input.touchCount; ++i) {
			Touch touch = Input.GetTouch(i);

			if (touch.position.x < Screen.width / 2)
				return touch;
		}

		return null;
	}


	private bool IsMoving() => this.movementDirection.magnitude > 0f;

#elif UNITY_WEBPLAYER || UNITY_STANDALONE
	private void Start() {
		this.movementAxisName = "Vertical" + this.playerNumber;
		this.turnAxisName = "Horizontal" + this.playerNumber;
	}


	private void FixedUpdate() {
		this.Move();
		this.Turn();
	}


	private void ResetInput() {
		this.movementInputValue = 0f;
		this.turnInputValue = 0f;
	}


	private void GetInput() {
		this.movementInputValue = Input.GetAxis(this.movementAxisName);
		this.turnInputValue = Input.GetAxis(this.turnAxisName);
	}


	private void Move() {
		// Adjust the position of the tank based on the player's input.
	
		float distancePerFrame = this.movementInputValue * this.speed * Time.deltaTime;
		Vector3 movement = this.transform.forward * distancePerFrame;

		this.rigidbody.MovePosition(this.rigidbody.position + movement);
	}


	private void Turn() {
		// Adjust the rotation of the tank based on the player's input.
	
		float rotationPerFrame = this.turnInputValue * this.turnSpeed * Time.deltaTime;
		var turn = Quaternion.Euler(0f, rotationPerFrame, 0f);

		this.rigidbody.MoveRotation(this.rigidbody.rotation * turn);
	}


	private bool IsMoving() => Mathf.Abs(this.movementInputValue) > .1f || Mathf.Abs(this.turnInputValue) > .1f;

#endif
}