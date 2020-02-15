using UnityEngine;

public class TankMovement : MonoBehaviour
{
	public readonly float speed = 12f;
	public readonly float turnSpeed = 180f;

	public int playerNumber = 1;
	public AudioSource movementAudio;
	public AudioClip engineIdling;
	public AudioClip engineDriving;


	private string movementAxisName;
	private string turnAxisName;
	private new Rigidbody rigidbody;
	private float movementInputValue;
	private float turnInputValue;


	private void Awake() => this.rigidbody = this.GetComponent<Rigidbody>();


	private void OnEnable() {
		this.rigidbody.isKinematic = false;
		this.movementInputValue = 0f;
		this.turnInputValue = 0f;
	}


	private void Start() {
		this.movementAxisName = "Vertical" + this.playerNumber;
		this.turnAxisName = "Horizontal" + this.playerNumber;
	}


	private void OnDisable() => this.rigidbody.isKinematic = true;

	private void Update() {
		// Store the player's input and make sure the audio for the engine is playing.

		this.movementInputValue = Input.GetAxis(this.movementAxisName);
		this.turnInputValue = Input.GetAxis(this.turnAxisName);

		this.UpdateEngineAudio();
	}


	private void FixedUpdate() {
		// Move and turn the tank.

		this.Move();
		this.Turn();
	}


	private void UpdateEngineAudio() {
		// Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

		if (Mathf.Abs(this.movementInputValue) > .1f || Mathf.Abs(this.turnInputValue) > .1f) {
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
}