using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
	public int playerNumber = 1;
	public Rigidbody shell;
	public Transform fireTransform;
	public Slider aimSlider;
	public AudioSource shootingAudio;
	public AudioClip chargingClip;
	public AudioClip fireClip;
	public float minLaunchForce = 15f;
	public float maxLaunchForce = 30f;
	public float maxChargeTime = 0.75f;


	private string fireButton;
	private float currentLaunchForce;
	private float chargeSpeed;
	private bool fired;


	private void OnEnable() {
		this.currentLaunchForce = this.minLaunchForce;
		this.aimSlider.value = this.minLaunchForce;
	}


	private void Start() {
		this.fireButton = "Fire" + this.playerNumber;

		this.chargeSpeed = (this.maxLaunchForce - this.minLaunchForce) / this.maxChargeTime;
	}


	private void Update() {
		// Track the current state of the fire button and make decisions based on the current launch force.

		this.aimSlider.value = this.minLaunchForce;

		if (this.currentLaunchForce >= this.maxLaunchForce && !this.fired) {
			this.currentLaunchForce = this.maxLaunchForce;
			this.Fire();
		}
		else if (Input.GetButtonDown(this.fireButton)) {
			this.fired = false;
			this.currentLaunchForce = this.minLaunchForce;

			this.shootingAudio.clip = this.chargingClip;
			this.shootingAudio.Play();
		}
		else if (Input.GetButton(this.fireButton) && !this.fired) {
			this.currentLaunchForce += this.chargeSpeed * Time.deltaTime;
			this.aimSlider.value = this.currentLaunchForce;
		}
		else if (Input.GetButtonUp(this.fireButton) && !this.fired) {
			this.Fire();
		}
	}


	private void Fire() {
		// Instantiate and launch the shell.

		this.fired = true;

		Rigidbody shellInstance = Instantiate(this.shell, this.fireTransform.position, this.fireTransform.rotation);
		shellInstance.velocity = this.currentLaunchForce * this.fireTransform.forward;

		this.shootingAudio.clip = this.fireClip;
		this.shootingAudio.Play();

		this.currentLaunchForce = this.minLaunchForce;
	}
}