using UnityEngine;
using UnityEngine.Networking;

public class TankMovement : NetworkBehaviour
{
	public EngineAudio engineAudio;

	readonly float speedPerFrame;

	Rigidbody rigidbody;
#if UNITY_ANDROID || UNITY_IOS
	Vector2 touchStartPosition;
	Vector2 movementDirection;
#elif UNITY_WEBPLAYER || UNITY_STANDALONE
	Input movementInput;
	Input turnInput;
#endif


	public TankMovement() {
		this.speedPerFrame = 12f;
		this.movementInput = new Input(axisName: "Vertical", maxValue: 1f);
		this.turnInput = new Input(axisName: "Horizontal", maxValue: 180f);
	}


	public void Awake() => this.rigidbody = this.GetComponent<Rigidbody>();


	public void OnEnable() {
		this.rigidbody.isKinematic = false;
		
		this.ResetInput();
	}


	public void OnDisable() => this.rigidbody.isKinematic = true;


	public void Update() {
		// Store the player's input and make sure the audio for the engine is playing.

		if (this.isLocalPlayer) {
			this.GetInput();
			this.UpdateEngineAudio();
		}
	}


	void UpdateEngineAudio() {
		// Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

		if (this.IsMoving()) {
			if (this.engineAudio.IsCurrentSound(this.engineAudio.engineIdling))
				this.engineAudio.ChangeCurrentSound(this.engineAudio.engineDriving);
		}
		else {
			Debug.Assert(!this.IsMoving());

			if (this.engineAudio.IsCurrentSound(this.engineAudio.engineDriving))
				this.engineAudio.ChangeCurrentSound(this.engineAudio.engineIdling);
		}
	}


#if UNITY_ANDROID || UNITY_IOS
	void FixedUpdate() {
		if (this.IsMoving()) {
			float distancePerFrame = this.speedPerFrame * Time.deltaTime;
			Vector3 movement = new Vector3(this.movementDirection.x, 0f, this.movementDirection.y) * distancePerFrame;
			this.rigidbody.MovePosition(this.rigidbody.position + movement);

			float angleFromStraight = Vector2.SignedAngle(this.movementDirection, Vector2.up);
			this.rigidbody.MoveRotation(Quaternion.Euler(0f, angleFromStraight, 0f));
		}
	}


	void ResetInput() {
		this.touchStartPosition = Vector2.zero;
		this.movementDirection = Vector2.zero;
	}


	void GetInput() {
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


	Touch? GetTouch() {
		for (int i = 0; i < UnityEngine.Input.touchCount; ++i) {
			Touch touch = UnityEngine.Input.GetTouch(i);

			if (touch.position.x < Screen.width / 2)
				return touch;
		}

		return null;
	}


	bool IsMoving() => this.movementDirection.magnitude > 0f;
#elif UNITY_WEBPLAYER || UNITY_STANDALONE
	public void FixedUpdate() {
		this.Move();
		this.Turn();
	}


	void ResetInput() {
		this.movementInput.value = 0f;
		this.turnInput.value = 0f;
	}


	void GetInput() {
		this.movementInput.value = UnityEngine.Input.GetAxis(this.movementInput.axisName);
		this.turnInput.value = UnityEngine.Input.GetAxis(this.turnInput.axisName);
	}


	void Move() {
		// Adjust the position of the tank based on the player's input.

		float speed = Mathf.Min(this.movementInput.value, this.movementInput.maxValue);
		float distancePerFrame = speed * this.speedPerFrame * Time.deltaTime;
		Vector3 movementPerFrame = this.gameObject.transform.forward * distancePerFrame;

		this.rigidbody.MovePosition(this.rigidbody.position + movementPerFrame);
	}


	void Turn() {
		// Adjust the rotation of the tank based on the player's input.
	
		float rotationPerFrame = this.turnInput.value * this.turnInput.maxValue * Time.deltaTime;
		var turn = Quaternion.Euler(0f, rotationPerFrame, 0f);

		this.rigidbody.MoveRotation(this.rigidbody.rotation * turn);
	}


	bool IsMoving() => Mathf.Abs(this.movementInput.value) > .1f || Mathf.Abs(this.turnInput.value) > .1f;
#endif
}