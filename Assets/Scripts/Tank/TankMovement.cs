using System;
using UnityEngine;

public class TankMovement
{
	public class EngineAudio : Audio
	{
		public EngineAudio(AudioSource source, params AudioClip[] sounds) : base(source, sounds) {
			if (sounds.Length != 2)
				throw new ArgumentException("EngineAudio requires 2 sounds!");
		}

		public enum SoundID
		{
			Idling,
			Driving
		}

		public void ChangeCurrentSound(SoundID soundID) {
			this.ChangeCurrentSound((int)soundID);
			this.PlayCurrentSound();
		}

		public bool IsCurrentSound(SoundID engineSoundID) => this.IsCurrentSound((int)engineSoundID);
	}


	public readonly float speedPerFrame = 12f;

	public uint playerNumber = 1;

	private readonly EngineAudio engineAudio;

	private readonly GameObject tank;
	private Rigidbody rigidbody;

#if UNITY_ANDROID || UNITY_IOS
	private Vector2 touchStartPosition;
	private Vector2 movementDirection;
#elif UNITY_WEBPLAYER || UNITY_STANDALONE
	private struct Input
	{
		public Input(string axisName, float maxValue) {
			this.axisName = axisName;
			this.value = 0f;
			this.maxValue = maxValue;
		}

		public readonly string axisName;
		public readonly float maxValue;
		public float value;
	}


	private Input movementInput;
	private Input turnInput;
#endif


	public TankMovement(GameObject tank, EngineAudio engineAudio) {
		this.tank = tank;
		this.engineAudio = engineAudio;

		this.movementInput = new Input("Vertical" + this.playerNumber, 1f);
		this.turnInput = new Input("Horizontal" + this.playerNumber, 180f);
	}

	public void Awake() => this.rigidbody = this.tank.GetComponent<Rigidbody>();


	public void OnEnable() {
		this.rigidbody.isKinematic = false;
		
		this.ResetInput();
	}


	public void OnDisable() => this.rigidbody.isKinematic = true;


	public void Update() {
		// Store the player's input and make sure the audio for the engine is playing.

		this.GetInput();
		this.UpdateEngineAudio();
	}


	private void UpdateEngineAudio() {
		// Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

		if (this.IsMoving()) {
			if (this.engineAudio.IsCurrentSound(EngineAudio.SoundID.Idling))
				this.engineAudio.ChangeCurrentSound(EngineAudio.SoundID.Driving);
		}
		else {
			Debug.Assert(!this.IsMoving());

			if (this.engineAudio.IsCurrentSound(EngineAudio.SoundID.Driving))
				this.engineAudio.ChangeCurrentSound(EngineAudio.SoundID.Idling);
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

	public void FixedUpdate() {
		this.Move();
		this.Turn();
	}


	private void ResetInput() {
		this.movementInput.value = 0f;
		this.turnInput.value = 0f;
	}


	private void GetInput() {
		this.movementInput.value = UnityEngine.Input.GetAxis(this.movementInput.axisName);
		this.turnInput.value = UnityEngine.Input.GetAxis(this.turnInput.axisName);
	}


	private void Move() {
		// Adjust the position of the tank based on the player's input.

		float speed = Mathf.Min(this.movementInput.value, this.movementInput.maxValue);
		float distancePerFrame = speed * this.speedPerFrame * Time.deltaTime;
		Vector3 movementPerFrame = this.tank.transform.forward * distancePerFrame;

		this.rigidbody.MovePosition(this.rigidbody.position + movementPerFrame);
	}


	private void Turn() {
		// Adjust the rotation of the tank based on the player's input.
	
		float rotationPerFrame = this.turnInput.value * this.turnInput.maxValue * Time.deltaTime;
		var turn = Quaternion.Euler(0f, rotationPerFrame, 0f);

		this.rigidbody.MoveRotation(this.rigidbody.rotation * turn);
	}


	private bool IsMoving() => Mathf.Abs(this.movementInput.value) > .1f || Mathf.Abs(this.turnInput.value) > .1f;

#endif
}